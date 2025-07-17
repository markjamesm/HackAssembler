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
                Console.WriteLine($"Skipping comment: {line}");
            }

            if (line == string.Empty)
            {
                Console.WriteLine("Skipping whitespace line");
            }

            if (line.StartsWith('@'))
            {
                ParseAInstruction(line);
            }

            if (line.Contains('='))
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
        var idx = line.IndexOf('=');
        var dest = line.Substring(0, idx);

        byte destByte;
        
        switch (dest)
        {
            case "null":
                Console.WriteLine("C-Instruction: NULL");
                destByte = 0x0;
                break;
            case "M":
                Console.WriteLine("C-Instruction: M Dest");
                destByte = 0x1;
                break;
            case "D":   
                destByte = 0x2;
                Console.WriteLine($"C-Instruction: D Dest {destByte:B3}");
                break;
            case "MD":
                Console.WriteLine("C-Instruction: MD Dest");
                destByte = 0x3;
                break;
            case "A":
                Console.WriteLine("C-Instruction: A Dest");
                destByte = 0x4;
                break;
            case "AM":
                Console.WriteLine("C-Instruction: AM Dest");
                destByte = 0x5;
                break;
            case "AD":
                Console.WriteLine("C-Instruction: AD Dest");
                destByte = 0x6;
                break;
            case "AMD":
                Console.WriteLine("C-Instruction: AMD Dest");
                destByte = 0x7;
                break;
        }
    }
    
    // C-Instructions
    // 1 1 1 a c1 c2 c3 c4 c5 c6 d1 d2 d3 j1 j2 j3
    
    // dest = null, M, D, MD, A, AM, AD, AMD
    // d1 d2 d3

    private void PrintMachineInstructions()
    {
        foreach (var line in _machineInstructions)
        {
            Console.WriteLine($"@ value machine instruction: {line:B16}");
        }
    }
}