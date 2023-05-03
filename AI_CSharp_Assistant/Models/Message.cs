using AI_CSharp_Assistant.Styles;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AI_CSharp_Assistant.Models;

public class Message : ReactiveObject
{
    [Reactive] public bool IsAI { get; set; }
    [Reactive] public Control Content { get; set; }
    [Reactive] public bool IsWriting { get; set; } = false;
    
    public void Copy(string message)
    {
        TopLevel.GetTopLevel(((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow).Clipboard.SetTextAsync(message);
    }

    public void FormatTextBlock()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var _textBlock = (TextBlock)Content;
            var message = _textBlock.Text;
            StackPanel stack = new StackPanel(){Margin = new Thickness(0,5,0,0)};
            var parts = message.Split("```");
            for (var i = 0; i < parts.Length; i++)
            {
                if (i % 2 == 1)
                {
                    stack.Children.Add(new CodeView()
                    {
                        Margin = new Thickness(0,0,0,-50),
                        Text = parts[i]
                    });
                  //  stack.Children.Add(new TextBlock() { TextWrapping = TextWrapping.Wrap, Foreground = Brushes.Brown, Text =  parts[i]});
                }
                else
                {
                    stack.Children.Add(new TextBlock() { TextWrapping = TextWrapping.Wrap,  Text =  parts[i]});
                }
            }

            Content = stack;
        });
    }
}