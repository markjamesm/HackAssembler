namespace HackAssembler;

public static class FileWriter
{
    public static void SaveMachineInstructionsToFile(string filename, List<ushort> machineInstructions)
    {
        using var writeFile = new StreamWriter($"{filename}.hack");
        foreach (var instruction in machineInstructions)
        {
            writeFile.WriteLine($"{instruction:B16}");
        }
        
        Console.WriteLine($"Successfully wrote {filename}.hack to disk");
    }
}