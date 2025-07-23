namespace HackAssembler;

public class Parser
{
    private readonly string _filename;
    private readonly IEnumerable<string> _asmInstructions;
    private readonly List<ushort> _machineInstructions = [];
    private readonly Dictionary<string, ushort> _symbolTable = new()
    {
        {"R0", 0x0},
        {"R1", 0x1},
        {"R2", 0x2},
        {"R3", 0x3},
        {"R4", 0x4},
        {"R5", 0x5},
        {"R6", 0x6},
        {"R7", 0x7},
        {"R8", 0x8},
        {"R9", 0x9},
        {"R10", 0xA},
        {"R11", 0xB},
        {"R12", 0xC},
        {"R13", 0xD},
        {"R14", 0xE},
        {"R15", 0xF},
        {"SCREEN", 0x4000},
        {"KBD", 0x6000},
        {"SP", 0x0},
        {"LCL", 0x1},
        {"ARG", 0x2},
        {"THIS", 0x3},
        {"THAT", 0x4}
    };
    
    // Start at 16 because of R0 - R15
    private int _variableCount = 16;

    public Parser(string filename, IEnumerable<string> asmInstructions)
    {
        _filename = filename;
        _asmInstructions = asmInstructions;
    }
    
    public void Parse()
    {
        FirstPass();
        SecondPass();
    }

    // Add the label symbols
    private void FirstPass()
    {
        var lineCount = 0;
        
        foreach (var line in _asmInstructions)
        {
            var trimmedLine = line.Trim();
            
            if (trimmedLine.StartsWith("//"))
            {
               Console.WriteLine($"Skipping line (comment): {trimmedLine}");
               continue;
            }

            if (trimmedLine == string.Empty)
            {
                Console.WriteLine("Skipping line: whitespace");
                continue;
            }

            if (trimmedLine.StartsWith('@'))
            {
                lineCount++;
            }

            if (trimmedLine.Contains('=') || trimmedLine.Contains(';'))
            {
                lineCount++;
            }
            
            if (trimmedLine.StartsWith('('))
            {
                ParseLabel(trimmedLine, lineCount);
            }
        }
    }

    // Add A-Instructions and C-Instructions
    private void SecondPass()
    {
        foreach (var line in _asmInstructions)
        {
            var trimmedLine = line.Trim();
            
            if (trimmedLine.StartsWith("//"))
            {
                Console.WriteLine($"Skipping line (comment): {trimmedLine}");
                continue;
            }

            if (trimmedLine == string.Empty)
            {
                Console.WriteLine("Skipping line: whitespace");
                continue;
            }

            if (trimmedLine.StartsWith('@'))
            {
                ParseAInstruction(trimmedLine);
            }

            if (trimmedLine.Contains('=') || trimmedLine.Contains(';'))
            {
                ParseCInstruction(trimmedLine);
            }
        }

        FileWriter.SaveMachineInstructionsToFile(_filename,  _machineInstructions);
    }
    
    private void ParseLabel(string line, int lineCount)
    {
        var start = line.IndexOf('(') + 1;
        var end =  line.IndexOf(')');
        var labelName = line.Substring(start, end - start);

        if (_symbolTable.TryGetValue(labelName, out var address))
        {
            Console.WriteLine($"Label {labelName} exists in symbol table. Address: {address}");
        }
        
        if (!_symbolTable.TryGetValue(labelName, out _))
        {
            _symbolTable.Add(labelName, (ushort)lineCount);
            Console.WriteLine($"Added missing label {labelName} to symbol table. Address: {lineCount}");
        }
    }
    
    private void ParseAInstruction(string line)
    {
        var registerValue = line.Split("@")[1];
        var isNumeric = ushort.TryParse(registerValue, out var registerValueInt);

        if (isNumeric)
        {
            _machineInstructions.Add((ushort)(0 + registerValueInt));
            Console.WriteLine($"Numeric A instruction added: {registerValueInt}");
        }
        
        if (!isNumeric)
        {
            if (_symbolTable.TryGetValue(registerValue, out var existingValue))
            {
                Console.WriteLine($"Symbol exists in table: {registerValue}, Symbol value: {existingValue}");
                
                // A-instructions start with 0
                _machineInstructions.Add((ushort)(0 + existingValue));
            }

            if (!_symbolTable.TryGetValue(registerValue, out _))
            {
                _symbolTable.Add(registerValue, (ushort)_variableCount);
                Console.WriteLine($"Missing symbol added: {registerValue}");
                
                // A-instructions start with 0
                _machineInstructions.Add((ushort)(0 + _variableCount));
                
                _variableCount++;
            }
        }
    }

    private void ParseCInstruction(string line)
    {
        byte? destinationBits = null;
        byte? jumpBits = null;
        Comp? compBits = null;

        if (line.Contains('=') && !line.Contains(';'))
        {
            var splitLine = line.Split('=');
            var dest = splitLine[0];
            destinationBits = Bits.GetDestinationBits(dest);

            var comp = splitLine[1];
            compBits = Bits.GetCompBits(comp);
        }

        if (line.Contains(';') && !line.Contains('='))
        {
            var splitLine = line.Split(';');
            var comp = splitLine[0];
            compBits = Bits.GetCompBits(comp);
            
            var jumpInstruction = splitLine[1];
            jumpBits = Bits.GetJumpBits(jumpInstruction);
        }

        if (line.Contains('=') && line.Contains(';'))
        {
            var destVal =  line.Split('=');
            destinationBits = Bits.GetDestinationBits(destVal[0]);

            var compVal = line.Split('=', ';')[1];
            compBits = Bits.GetCompBits(compVal);

            var jumpVal = line.Split(';')[1];
            jumpBits = Bits.GetJumpBits(jumpVal);
        }
        
        var instruction = BuildCInstruction(destinationBits, compBits?.AFlag, compBits?.CBits, jumpBits);
        _machineInstructions.Add(instruction);
    }
    
    private static ushort BuildCInstruction(byte? destinationBits, byte? aFlag, byte? compBits, byte? jumpBits)
    {
        var instruction = (ushort)((0b111 << 13) | ((aFlag ?? 0) << 12) | ((compBits ?? 000000) << 6) | ((destinationBits ?? 000) << 3) | (jumpBits ?? 000));
        return instruction;
    }
}