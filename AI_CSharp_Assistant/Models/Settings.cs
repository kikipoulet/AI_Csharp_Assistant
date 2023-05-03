using AI_CSharp_Assistant.Lib;
using Avalonia;
using Avalonia.Controls;
using OpenAI_API;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SukiUI.Controls;
using Console = System.Console;


namespace AI_CSharp_Assistant.Models;

public class Settings : PersistentSingleton<Settings>
{
   
    [Reactive] public bool DarkMode { get; set; } = false;

    [Reactive] public string API_KEY { get; set; } = "No API Key";
    [Reactive] public string ORG_ID { get; set; } = "No Org ID";

    public void SaveSettiings()
    {
        if(DarkMode)
            SukiUI.ColorTheme.LoadDarkTheme(Application.Current);
        else
            SukiUI.ColorTheme.LoadLightTheme(Application.Current);

        ChatGPT_Engine.Instance.GPT = new OpenAIAPI(new APIAuthentication(API_KEY, ORG_ID));
        
        Save();
        InteractiveContainer.CloseDialog();
        InteractiveContainer.ShowToast(new TextBlock()
        {
            Margin = new Thickness(15,7), Text = "Settings Saved !"
        }, 7);
    }
}