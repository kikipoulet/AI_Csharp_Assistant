using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AI_CSharp_Assistant.Lib;
using AI_CSharp_Assistant.Models;
using Avalonia.Media.Imaging;
using OpenAI_API.Images;
using ReactiveUI.Fody.Helpers;

namespace AI_CSharp_Assistant.ViewModels;

public class ImageGenVM : Singleton<ImageGenVM>
{
        [Reactive] public string InputString { get; set; } = "";

        [Reactive] public string Url { get; set; } = "";
        [Reactive] public bool Sending { get; set; } = false;
        
        public async void GenerateImage()
        {
                Sending = true;
                var result = await ChatGPT_Engine.Instance.GPT.ImageGenerations.CreateImageAsync(new ImageGenerationRequest(InputString, 1, ImageSize._1024, null, ImageResponseFormat.Url));
                Url = result.ToString();
                Sending = false;
        }

        public void SaveImage()
        {
                Task.Run(() =>
                {
                        Sending = true;
                        var dir = new DirectoryInfo("OutputImages");
                        if (!dir.Exists)
                                dir.Create();

                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(Url);
                        Image image = Image.FromStream(stream);
                        image.Save("OutputImages\\" + Random.Shared.NextInt64().ToString() + ".png");
                        stream.Flush();
                        stream.Close();
                        client.Dispose();
                        Sending = false;
                });
        }
}