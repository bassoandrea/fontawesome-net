using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FontAwesome.Net.Generators;

namespace FontAwesome.Net.Wpf.ExplorerApp;

internal class MainWindowViewModel
    : ObservableObject
{
    private MainWindowViewModel()
    {
        Icons = Enumeration.GetAll<FontAwesomeIcon>().ToList();
        Icon = FontAwesomeIcon.None;
        IconColor = Color.FromRgb(0,0,0);
    }

    public List<FontAwesomeIcon> Icons { get; }

    public FontAwesomeIcon Icon
    {
        get;
        set
        {
            if (!SetProperty(ref field, value))
                return;

            IconStyle = IconStyles.FirstOrDefault();
            OnPropertyChanged(nameof(IconStyles));
        }
    }

    public IFontAwesomeIconStyle[] IconStyles
        => Icon.Styles;

    public IFontAwesomeIconStyle? IconStyle
    {
        get;
        set => SetProperty(ref field, value);
    }

    public double Rotation
    {
        get;
        set => SetProperty(ref field, value);
    }

    public FlipOrientation FlipOrientation
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool Spin
    {
        get;
        set => SetProperty(ref field, value);
    }

    public double SpinDuration
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool ReverseSpinDirection
    {
        get;
        set => SetProperty(ref field, value);
    }

    public Color IconColor
    {
        get;
        set => SetProperty(ref field, value);
    }

    #region -- Singleton Pattern Implementation --

    public static MainWindowViewModel Instance { get; }

    static MainWindowViewModel()
    {
        Instance = new MainWindowViewModel();
    }

    #endregion
}
