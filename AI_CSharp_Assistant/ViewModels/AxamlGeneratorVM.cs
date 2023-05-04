using System.Threading.Tasks;
using AI_CSharp_Assistant.Models;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OpenAI_API.Chat;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SukiUI.Controls;

namespace AI_CSharp_Assistant.ViewModels;

public class AxamlGeneratorVM : ReactiveObject
{
    [Reactive] public Control DemoContent { get; set; } = new TextBlock(){Text = "Previewer", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
    [Reactive] public string CurrentMessage { get; set; } = "";
    [Reactive] public string OutputCode { get; set; } = "No Xaml Yet.";
    [Reactive] public bool Sending { get; set; } = false;
    
    [Reactive] public Conversation chat { get; set; }= null;

    public void Refresh()
    {
        chat = ChatGPT_Engine.Instance.GPT.Chat.CreateConversation();
        chat.AppendSystemMessage("You are a C# expert working with AvaloniaUI. I will send you the description of an UI layout code and you will answer with XAML code for Avalonia of that UI. I don't want nothing more in your answer that the XAML code. The description of the UI is always my whole message, and your answer must only contain the XAML code of that UI. I don't want Markdown syntax, I don't want these characters ```. I don't want my description to be encapsulated inside a window, I just want the xaml code of what I ask you; no more. I do not want a window, I want what I ask only. If you answer me with anything more than the xaml code I will be very sad.");
        chat.AppendUserInput("a button centered on the screen with the word test");
        chat.AppendExampleChatbotOutput("<Button HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" ><TextBlock Text=\"test\"></TextBlock></Button>");
    }
    public async Task SendMessage()
    {
        Sending = true;
        if(chat == null)
            Refresh();
        
        chat.AppendUserInput(CurrentMessage);
        OutputCode = "";
        await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                OutputCode += res;
            });
        }

        OutputCode = OutputCode.Replace("```xaml", "");
        OutputCode = OutputCode.Replace("```", "");
        OutputCode = OutputCode;

        string PreviewCode = "<Grid xmlns='https://github.com/avaloniaui' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>" +
                             OutputCode + "</Grid>";

        try
        {
            DemoContent = AvaloniaRuntimeXamlLoader.Parse<Grid>(PreviewCode);
        }
        catch
        {
            DemoContent = new TextBlock(){Text = "Failed to load previewer", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
        }

        Sending = false;
    }
}