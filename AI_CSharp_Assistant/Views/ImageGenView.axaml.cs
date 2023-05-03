using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AI_CSharp_Assistant.Views;

public partial class ImageGenView : UserControl
{
    public ImageGenView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}