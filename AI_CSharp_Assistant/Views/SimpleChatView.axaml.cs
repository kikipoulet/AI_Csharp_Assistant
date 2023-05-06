using System;
using AI_CSharp_Assistant.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
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

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        ToggleButton button = (ToggleButton)sender;
        if (button.IsChecked == true)
            ((SimpleChatVM)this.DataContext).CMode = ((TextBlock)button.Content).Text;
    }
}