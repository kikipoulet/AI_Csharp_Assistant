using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AI_CSharp_Assistant.Lib;
using AI_CSharp_Assistant.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SukiUI.Controls;

namespace AI_CSharp_Assistant.ViewModels;

public class SimpleChatVM : ReactiveObject
{
    [Reactive]
    public ObservableCollection<string> AvailableModels { get; set; } = new ObservableCollection<string>()
        { "ChatGPT Turbo", "ChatGPT 4", "ChatGPT 4 32k" };
    public string AccueilMessage { get; set; } = "Hi, I am your C# assistant. How can I help you ?";
    public string PresentationText { get; set; } = "Ready to help you for C# programming specific questions.";
    [Reactive] public string InitialInstruction { get; set; } = "You are a C# expert. Every programming question asked is about C# by default. Every UI development is about AvaloniaUI or WPF by default. Format your answer with MarkDown.";
   [Reactive] public string GPTModel { get; set; } = "ChatGPT Turbo";
    public ScrollViewer MessagesScrollViewer { get; set; } = null;
    [Reactive] public string CurrentMessage { get; set; } = "";
    [Reactive] public bool Sending { get; set; } = false;
    [Reactive] public ObservableCollection<Message> Messages { get; set; }

    [Reactive] public Conversation chat { get; set; }= null;

    public string GetString()
    {
        var list = new List<string>(){"test"};
        return list.Where(t => t.Length > 6).First();
    }

    public Model GetCurrentModel()
    {
        switch (GPTModel)
        {
            case "ChatGPT Turbo":
                return Model.ChatGPTTurbo0301;
            case "ChatGPT 4" :
                return Model.GPT4; 
            case "ChatGPT 4 32k":
                return Model.GPT4_32k_Context;
        }

        return Model.ChatGPTTurbo0301;
    }
    public SimpleChatVM()
    {
        this.WhenAnyValue(t => t.InitialInstruction).Subscribe((s =>
        {
            Messages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    IsAI = true, Content = new TextBlock(){Text = AccueilMessage}
                }
            };
        }));
    }
    public void Refresh()
    {
        chat = ChatGPT_Engine.Instance.GPT.Chat.CreateConversation(new ChatRequest()
        {
            Model = GetCurrentModel()
        });
        chat.AppendSystemMessage(InitialInstruction);

        if(Messages.Count > 1)
            Messages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    IsAI = true, Content = new TextBlock(){Text = AccueilMessage}
                }
            };
    }
    public void SendMessage()
    {
        if(chat == null)
            Refresh();
        
        Sending = true;
        Messages.Add(new Message()
        {
            IsAI= false,
            Content = new TextBlock(){TextWrapping = TextWrapping.Wrap, Text = CurrentMessage}
        });

        var message = CurrentMessage;
        CurrentMessage = "";
        
        ScrollToBottomDelayed(200);

        AskResponse(message);
    }

    private void AskResponse(string message)
    {
        TextBlock _textBlock = null;

        _textBlock = new TextBlock() { TextWrapping = TextWrapping.Wrap };

        var NewMessage = new Message()
        {
            IsAI = true, Content = _textBlock, IsWriting = true
        };
        
        
        Task.Run(async () =>
        {
            try
            {
                Messages.Add(NewMessage);

                chat.AppendUserInput(message);

                await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        _textBlock.Text += res;
                        MessagesScrollViewer?.ScrollToEnd();
                    });
                }

                NewMessage.FormatTextBlock();
                NewMessage.IsWriting = false;
                Sending = false;
                ScrollToBottom();
            }
            catch (Exception exc)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    InteractiveContainer.ShowToast(new TextBlock() {Margin = new Thickness(5),Text = exc.Message }, 7);
                    NewMessage.IsWriting = false;
                    Sending = false;
                });
            }
        });
    }
    

    private void ScrollToBottom()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            MessagesScrollViewer?.ScrollToEnd();
        });
    }

    private void ScrollToBottomDelayed(int millis)
    {
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(millis);
            Dispatcher.UIThread.Invoke(() =>
            {
                MessagesScrollViewer?.ScrollToEnd();
            });
        });
    }
}