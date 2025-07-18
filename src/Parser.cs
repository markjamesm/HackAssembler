namespace HackAssembler;

public class Parser
{
    private readonly IEnumerable<string> _programLines;
    private readonly List<ushort> _machineInstructions = [];
    
    public Parser(IEnumerable<string> programLines)
    {
        _programLines = programLines;
    }

    public void Parse()
    {
        foreach (var line in _programLines)
        {
            if (line.StartsWith("//"))
            {
                Console.WriteLine($"Skipping line (comment): {line}");
                continue;
            }

            if (line == string.Empty)
            {
                Console.WriteLine("Skipping line: whitespace");
            }

            if (line.StartsWith('@'))
            {
                ParseAInstruction(line);
            }

            if (line.Contains('=') || line.Contains(';'))
            {
                ParseCInstruction(line);
            }
        }
        
        PrintMachineInstructions();
    }

    private void ParseAInstruction(string line)
    {
        var value = line.Skip(1);
        var strValue = new string(value.Select(x => x).ToArray());
        var int16Val = Convert.ToUInt16(strValue);
        _machineInstructions.Add((ushort)(0 + int16Val));
    }

    private void ParseCInstruction(string line)
    {
        byte? destValue;
        byte? jumpInstruction;
        
        if (line.Contains('='))
        {
            var splitLine = line.Split('=');
            var dest = splitLine[0];

            destValue = GetDestinationByte(dest);
        }
        
        if (line.Contains(';'))
        {
            var splitLine = line.Split(';');
            var lhs = splitLine[0];
            var rhs = splitLine[1];
            
            Console.WriteLine($"lhs: {lhs}, rhs: {rhs}");
        }
    }
    
    // C-Instructions
    // 1 1 1 a c1 c2 c3 c4 c5 c6 d1 d2 d3 j1 j2 j3
    
    // dest = null, M, D, MD, A, AM, AD, AMD
    // d1 d2 d3

    private static byte GetDestinationByte(string dest)
    {
        switch (dest)
        {
            case "null":
                Console.WriteLine("C-Instruction: NULL");
                return 0x0;
            case "M":
                Console.WriteLine($"C-Instruction: M Dest {0x1:B3}");
                return 0x1;
            case "D":   
                Console.WriteLine($"C-Instruction: D Dest {0x2:B3}");
                return 0x2;
            case "MD":
                Console.WriteLine("C-Instruction: MD Dest");
                return 0x3;
            case "A":
                Console.WriteLine("C-Instruction: A Dest");
                return 0x4;
            case "AM":
                Console.WriteLine("C-Instruction: AM Dest");
                return 0x5;
            case "AD":
                Console.WriteLine("C-Instruction: AD Dest");
                return 0x6;
            case "AMD":
                Console.WriteLine("C-Instruction: AMD Dest");
                return 0x7;
            default:
                Console.WriteLine("Destination not recognized");
                throw new ArgumentException("Error: destination not recognized");
        }
    }

    /*
    private byte GetCompByte(string comp)
    {
        
    } */

    private void PrintMachineInstructions()
    {
        foreach (var line in _machineInstructions)
        {
            Console.WriteLine($"@ value machine instruction: {line:B16}");
        }
    }
}