using AI_CSharp_Assistant.Lib;
using ChatGPT.Net;
using OpenAI_API;

namespace AI_CSharp_Assistant.Models;

public class ChatGPT_Engine : Singleton<ChatGPT_Engine>
{
    public OpenAIAPI GPT { get; set; } = new OpenAIAPI(new APIAuthentication(Settings.Instance.API_KEY, Settings.Instance.ORG_ID));
}