using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Material.Icons;
using Material.Icons.Avalonia;

namespace AI_CSharp_Assistant.Styles;

public partial class CodeView : UserControl
{
 

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private string _text = "";
    private TextBlock _textBlock;

    public CodeView()
    {
        InitializeComponent();
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        var lineNumberTextBlock = new TextBlock()
        {
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            Margin = new Thickness(10,5,5,0),
            Foreground = Brushes.Gray
        };
        Grid.SetColumn(lineNumberTextBlock, 0);

        _textBlock = new TextBlock()
        {
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            Margin = new Thickness(10,5,5,0),
            FontFamily = new FontFamily("Consolas")
        };
        Grid.SetColumn(_textBlock, 1);

        grid.Children.Add(lineNumberTextBlock);
        grid.Children.Add(_textBlock);

        var button = new Button()
        {
            Classes = { "Accent" },
            Content = new MaterialIcon() { Kind = MaterialIconKind.ContentCopy, Foreground = new Avalonia.Media.SolidColorBrush((Avalonia.Media.Color)Application.Current.FindResource("SukiText"))},
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
        };

        button.Click += (sender, args) =>
        {
            TopLevel.GetTopLevel(((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime)
                .MainWindow).Clipboard.SetTextAsync(Text);
        };
        
        Grid.SetColumn(button,1);
        grid.Children.Add(button);

        Content = grid;

        this.AttachedToVisualTree += (s, e) => UpdateText();
    }

    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                UpdateText();
            }
        }
    }
    
    public static readonly DirectProperty<CodeView, string> TextProperty =
        AvaloniaProperty.RegisterDirect<CodeView, string>(
            nameof(Text),
            o => o.Text,
            (o, v) => o.Text = v,
            defaultBindingMode: BindingMode.OneWay,
            enableDataValidation: true);

    private void UpdateText()
    {
        try
        {
            var lines = _text.Split('\n');
            var lineNumberText = "";
            for (int i = 1; i <= lines.Length; i++)
            {
                lineNumberText += $"{i}\n";
            }

            (_textBlock.Parent as Grid).RowDefinitions.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                (_textBlock.Parent as Grid).RowDefinitions.Add(new RowDefinition());
            }


            (_textBlock.Parent as Grid).ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Auto);
            (_textBlock.Parent as Grid).ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

            (_textBlock.Parent as Grid).Children[0].SetValue(Grid.RowSpanProperty, lines.Length);
            (_textBlock.Parent as Grid).Children[0].SetValue(TextBlock.TextProperty, lineNumberText);

            _textBlock.SetValue(Grid.ColumnProperty, 1);
            _textBlock.SetValue(Grid.RowProperty, 0);
            _textBlock.SetValue(Grid.RowSpanProperty, lines.Length);
            _textBlock.Text = _text.TrimEnd();
        }catch(Exception exc){}
    }
}