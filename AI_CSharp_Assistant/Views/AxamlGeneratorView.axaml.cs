using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AI_CSharp_Assistant.Views;

public partial class AxamlGeneratorView : UserControl
{
    public AxamlGeneratorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}