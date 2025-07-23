namespace HackAssembler;

public static class Bits
{
    public static byte GetDestinationBits(string dest)
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

    public static byte GetJumpBits(string jumpInstruction)
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

    public static Comp GetCompBits(string comp)
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
}