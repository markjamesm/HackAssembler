namespace HackAssembler;

public class Parser
{
    private IEnumerable<string> _programLines;
    
    public Parser(IEnumerable<string> programLines)
    {
        _programLines = programLines;
    }

    public void Parse()
    {
        var machineProgram = new List<ushort>();
        
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
                var value = line.Skip(1);
                var strValue = new string(value.Select(x => x).ToArray());
                var int16Val = Convert.ToUInt16(strValue);
                machineProgram.Add((ushort)(0 + int16Val));

                // 0 000000000010101
            }
        }

        foreach (var line in machineProgram)
        {
            Console.WriteLine($"@ value machine instruction: {line:B16}");
        }
    }
}