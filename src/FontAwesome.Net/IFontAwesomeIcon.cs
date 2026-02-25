namespace FontAwesome.Net
{
    public interface IFontAwesomeIcon
    {
        int Id { get; }

        string Name { get; }

        IFontAwesomeIconStyle[] Styles { get; }
    }

    public interface IFontAwesomeIconStyle
    {
        string Name { get; }
    }
}
