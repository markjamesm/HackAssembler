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
        {"KBD", 0x6000}
    };

    public Parser(string filename, IEnumerable<string> asmInstructions)
    {
        _filename = filename;
        _asmInstructions = asmInstructions;
    }

    public void Parse()
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
            }

            if (trimmedLine.StartsWith('@'))
            {
                ParseAInstruction(trimmedLine);
            }

            if (trimmedLine.Contains('=') || trimmedLine.Contains(';'))
            {
                ParseCInstruction(trimmedLine);
            }
            
            if (trimmedLine.StartsWith('('))
            {
                ParseLabel(trimmedLine);
            }
        }

        FileWriter.SaveMachineInstructionsToFile(_filename,  _machineInstructions);
        // PrintMachineInstructions();
    }

    private void ParseAInstruction(string line)
    {
        var registerValue = line.Split("@")[1];
        var isNumeric = ushort.TryParse(registerValue, out var registerValueInt);

        if (!isNumeric)
        {
            if (_symbolTable.TryGetValue(registerValue, out var existingValue))
            {
                Console.WriteLine($"Symbol exists in table: {registerValue}");
                
                // A-instructions start with 0
                _machineInstructions.Add((ushort)(0 + existingValue));
            }

            if (!_symbolTable.TryGetValue(registerValue, out _))
            {
                // Implement check to ensure memory is free
                var randomRegister = Random.Shared.Next(16, 16383);
                _symbolTable.Add(registerValue, (ushort)randomRegister);
                
                Console.WriteLine($"Symbol added to table: {registerValue}");
                
                // A-instructions start with 0
                _machineInstructions.Add((ushort)(0 + randomRegister));
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
            destinationBits = GetDestinationBits(dest);

            var comp = splitLine[1];
            compBits = GetCompBits(comp);
        }

        if (line.Contains(';') && !line.Contains('='))
        {
            var splitLine = line.Split(';');
            var comp = splitLine[0];
            compBits = GetCompBits(comp);
            
            var jumpInstruction = splitLine[1];
            jumpBits = GetJumpBits(jumpInstruction);
        }

        if (line.Contains('=') && line.Contains(';'))
        {
            var destVal =  line.Split('=');
            destinationBits = GetDestinationBits(destVal[0]);

            var compVal = line.Split('=', ';')[1];
            compBits = GetCompBits(compVal);

            var jumpVal = line.Split(';')[1];
            jumpBits = GetJumpBits(jumpVal);
        }
        
        var instruction = BuildCInstruction(destinationBits, compBits?.AFlag, compBits?.CBits, jumpBits);
        _machineInstructions.Add(instruction);
    }
    
    private void ParseLabel(string line)
    {
        var start = line.IndexOf('(') + 1;
        var end =  line.IndexOf(')');
        var labelName = line.Substring(start, end - start);

        if (_symbolTable.TryGetValue(labelName, out var existingValue))
        {
            Console.WriteLine($"Label exists in table: {labelName}");
         //   _machineInstructions.Add(existingValue);
        }
        /*
        if (!_symbolTable.TryGetValue(labelName, out _))
        {
            Console.WriteLine($"Label doesn't exist: {labelName}");
            
            var randomRegister = Random.Shared.Next(16, 16383);
            _symbolTable.Add(labelName, (ushort)randomRegister);
            
            _machineInstructions.Add((ushort)(0 + randomRegister));
        } */
    }

    private static byte GetDestinationBits(string dest)
    {
        return dest switch
        {
            "null" => 0x0,
            "M" => 0x1,
            "D" => 0x2,
            "MD" => 0x3,
            "A" => 0x4,
            "AM" => 0x5,
            "AD" => 0x6,
            "AMD" => 0x7,
            _ => throw new ArgumentException("Error: destination not recognized")
        };
    }

    private static byte GetJumpBits(string jumpInstruction)
    {
        return jumpInstruction switch
        {
            "null" => 0x0,
            "JGT" => 0x1,
            "JEQ" => 0x2,
            "JGE" => 0x3,
            "JLT" => 0x4,
            "JNE" => 0x5,
            "JLE" => 0x6,
            "JMP" => 0x7,
            _ => throw new ArgumentException("Error: jump instruction not recognized")
        };
    }

    private static Comp GetCompBits(string comp)
    {
        return comp switch
        {
            "0" => new Comp(0x0, 0x2A),
            "1" => new Comp(0x0, 0x3F),
            "-1" => new Comp(0x0, 0x3A),
            "D" => new Comp(0x0, 0xC),
            "A" => new Comp(0x0, 0x30),
            "!D" => new Comp(0x0, 0xD),
            "!A" => new Comp(0x0, 0x31),
            "-D" => new Comp(0x0, 0xF),
            "-A" => new Comp(0x0, 0x33),
            "D+1" => new Comp(0x0, 0x1F),
            "A+1" => new Comp(0x0, 0x37),
            "D-1" => new Comp(0x0, 0xE),
            "A-1" => new Comp(0x0, 0x32),
            "D+A" => new Comp(0x0, 0x2),
            "D-A" => new Comp(0x0, 0x13),
            "A-D" => new Comp(0x0, 0x7),
            "D&A" => new Comp(0x0, 0x0),
            "D|A" => new Comp(0x0, 0x15),
            "M" => new Comp(0x1, 0x30),
            "!M" => new Comp(0x1, 0x31),
            "M+1" => new Comp(0x1, 0x37),
            "M-1" => new Comp(0x1, 0x32),
            "D+M" => new Comp(0x1, 0x2),
            "D-M" => new Comp(0x1, 0x13),
            "M-D" => new Comp(0x1, 0x7),
            "D&M" => new Comp(0x1, 0x0),
            "D|M" => new Comp(0x1, 0x15),
            _ => throw new ArgumentException("Error: Comp value not recognized")
        };
    }

    private static ushort BuildCInstruction(byte? destinationBits, byte? aFlag, byte? compBits, byte? jumpBits)
    {
        var instruction = (ushort)((0b111 << 13) | ((aFlag ?? 0) << 12) | ((compBits ?? 000000) << 6) | ((destinationBits ?? 000) << 3) | (jumpBits ?? 000));
        return instruction;
    }

    private void PrintMachineInstructions()
    {
        var lineNumber = 0;
        foreach (var line in _machineInstructions)
        {
            Console.WriteLine($"{lineNumber} - Machine instruction: {line:B16}");
            lineNumber++;
        }
    }
}