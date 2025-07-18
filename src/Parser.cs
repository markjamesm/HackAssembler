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
        
        var instruction = BuildCInstruction(destinationBits, compBits, jumpBits);
        _machineInstructions.Add(instruction);
    }

    private static byte GetDestinationBits(string dest)
    {
        switch (dest)
        {
            case "null":
                return 0x0;
            case "M":
                return 0x1;
            case "D":
                return 0x2;
            case "MD":
                return 0x3;
            case "A":
                return 0x4;
            case "AM":
                return 0x5;
            case "AD":
                return 0x6;
            case "AMD":
                return 0x7;
            default:
                throw new ArgumentException("Error: destination not recognized");
        }
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
            "D&M" => new Comp(0x1, 0x13),
            "D|M" => new Comp(0x1, 0x15),
            _ => throw new ArgumentException("Error: Comp value not recognized")
        };
    }

    private static ushort BuildCInstruction(byte? destinationBits, Comp comp, byte? jumpBits)
    {
        var instruction = (ushort)(0b111 << 13 | ((comp.AFlag ?? 0) << 12) | ((comp.CBits ?? 000000) << 6) | ((destinationBits ?? 000) << 3) | jumpBits ?? 000);
        
        // Console.WriteLine($"{instruction:B16}");
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