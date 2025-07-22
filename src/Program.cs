namespace HackAssembler;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: HackAssembler <filename>");
        }
        
        if (args.Length == 1)
        {
            var currDir = Directory.GetCurrentDirectory();
            var asmFile = $"{currDir}/{args[0]}";
            var asmInstructions = File.ReadLines(asmFile);
        
            Console.WriteLine($"Parsing ASM file: {args[0]}");
            var filename = args[0].Split('.')[0];
            var parser = new Parser(filename, asmInstructions);
            parser.Parse();   
        }
    }
}