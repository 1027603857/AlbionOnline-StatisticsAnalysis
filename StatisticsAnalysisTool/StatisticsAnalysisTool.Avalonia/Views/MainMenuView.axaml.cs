using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StatisticsAnalysisTool.Avalonia.Views
{
    public class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}