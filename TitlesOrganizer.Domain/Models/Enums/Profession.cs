namespace TitlesOrganizer.Domain.Models.Enums
{
    [Flags]
    public enum Profession
    {
        Undefined = 0,
        Author = 0x1,
        Director = 0x2
        //Actor = 0x4,
        //Translator = 0x8,
        //Composer = 0x10,
        //Ilustrator = 0x20
    }
}
