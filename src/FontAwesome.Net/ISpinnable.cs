namespace FontAwesome.Net
{
    public interface ISpinnable
    {
        bool Spin { get; }

        double SpinDuration { get; }

        bool ReverseSpinDirection { get; }
    }
}
