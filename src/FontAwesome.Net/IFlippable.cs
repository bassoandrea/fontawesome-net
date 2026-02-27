namespace FontAwesome.Net
{
    public interface IFlippable
    {
        FlipOrientation FlipOrientation { get; }
    }

    public enum FlipOrientation
    {
        None,
        Horizontal,
        Vertical,
    }
}
