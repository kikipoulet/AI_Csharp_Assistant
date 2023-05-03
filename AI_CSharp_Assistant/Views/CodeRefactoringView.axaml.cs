using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AI_CSharp_Assistant.Views;

public partial class CodeRefactoringView : UserControl
{
    public CodeRefactoringView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}