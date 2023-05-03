using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AI_CSharp_Assistant.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Material.Icons;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SukiUI.Controls;

namespace AI_CSharp_Assistant.ViewModels;


public class CodeRefactoringVM : ReactiveObject
{
   
    
    [Reactive] public Conversation chat { get; set; }= null;
    public string InitialInstruction { get; set; } = "";

    public Model GPTModel { get; set; } = Model.ChatGPTTurbo0301;

   [Reactive] public string ActionString { get; set; } = "Clean";
    [Reactive]public MaterialIconKind ActionIcon { get; set; } = MaterialIconKind.Broom;

    [Reactive] public string InputCode { get; set; } = "";
    [Reactive] public string OutputCode { get; set; } = "";
    [Reactive] public bool Sending { get; set; } = false;
    [Reactive] public bool IsCode { get; set; } = true;

    public void Refresh()
    {
        chat = ChatGPT_Engine.Instance.GPT.Chat.CreateConversation(new ChatRequest()
        {
            Model = GPTModel
        });
        chat.AppendSystemMessage(InitialInstruction);
        
      

    }

    public string SendMessage()
    {
        var list = new List<string>();
        return list.Where(t => t.Length > 4).First();
    }
    public void Send()
    {
        Sending = true;
        Task.Run(async () =>
        {
            try
            {
                Refresh();
                
                chat.AppendUserInput(InputCode);
                OutputCode = "";
                await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        OutputCode += res;
                    });
                }

                
            }
            catch (Exception exc)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    InteractiveContainer.ShowToast(new TextBlock() {Margin = new Thickness(5),Text = exc.Message }, 7);
                    
                });
            }

            Sending = false;
        });
    }
}