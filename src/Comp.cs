namespace HackAssembler;

public class Comp
{
    public byte? AFlag { get; }
    public byte? CBits { get; }

    public Comp(byte aFlag, byte cBits)
    {
        AFlag = aFlag;
        CBits = cBits;
    }
}