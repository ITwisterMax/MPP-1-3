using System.Windows;
using AssemblyBrowserView.ViewModel;

namespace AssemblyBrowserView
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AssemblyViewModel();
        }
    }
}
