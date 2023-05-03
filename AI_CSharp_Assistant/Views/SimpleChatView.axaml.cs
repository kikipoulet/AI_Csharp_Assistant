using System;
using AI_CSharp_Assistant.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AI_CSharp_Assistant.Views;

public partial class SimpleChatView : UserControl
{
    public SimpleChatView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void StyledElement_OnDataContextChanged(object? sender, EventArgs e)
    {
        ((SimpleChatVM)DataContext).MessagesScrollViewer = this.FindControl<ScrollViewer>("MonScrollViewer");
    }
}