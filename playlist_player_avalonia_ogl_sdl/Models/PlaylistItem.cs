public class PlaylistItem
{
    public int Pos {  get; set; }
    public double? InSec { get; set; }
    public string OpenUrl { get; set; } = string.Empty;
    public double? OutSec { get; set; }
    public bool IsBreak { get; set; }
    public double? FixedPosSec { get; set; }
}
