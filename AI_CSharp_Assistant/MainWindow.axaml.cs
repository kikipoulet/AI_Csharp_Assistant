using AI_CSharp_Assistant.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;

namespace AI_CSharp_Assistant.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        if(Settings.Instance.DarkMode)
            SukiUI.ColorTheme.LoadDarkTheme(Application.Current);
    }
    

    private void ShowSettingsDialog(object? sender, RoutedEventArgs e)
    {
        InteractiveContainer.ShowDialog(new SettingsView());
    }
}