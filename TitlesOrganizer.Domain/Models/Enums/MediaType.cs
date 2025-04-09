namespace TitlesOrganizer.Domain.Models.Enums
{
    [Flags]
    public enum MediaType
    {
        None = 0,
        Text = 0x1,
        Audio = 0x2,
        Video = 0x4
    }
}