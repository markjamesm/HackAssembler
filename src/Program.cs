namespace HackAssembler;

class Program
{
    static void Main(string[] args)
    {
        var currDir = Directory.GetCurrentDirectory();
        var asmFile = $"{currDir}/{args[0]}";
        var lines = File.ReadLines(asmFile);
        
        Console.WriteLine($"Parsing ASM file: {currDir}/{args[0]}");
        var parser = new Parser(lines);
        parser.Parse();
    }
}