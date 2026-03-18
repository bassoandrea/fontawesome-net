using FontAwesome.Net.Generators;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Net.Wpf.ExplorerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var fontUri = new Uri("pack://application:,,,/FontAwesome.Net.Wpf.ExplorerApp;component/Resources/");

            FontAwesome.Net.Wpf.FontsManager.RegisterFont(FontAwesomeIconStyle.Solid, new FontFamily(fontUri, "./#Font Awesome 7 Free Solid"));
            FontAwesome.Net.Wpf.FontsManager.RegisterFont(FontAwesomeIconStyle.Brands, new FontFamily(fontUri, "./#Font Awesome 7 Brands Regular"));
            FontAwesome.Net.Wpf.FontsManager.RegisterFont(FontAwesomeIconStyle.Regular, new FontFamily(fontUri, "./#Font Awesome 7 Free Regular"));
        }
    }

}
