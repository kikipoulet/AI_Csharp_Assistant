using System.IO;
using System.Text.Json;
using ReactiveUI;

namespace AI_CSharp_Assistant.Lib;

public class Singleton<T> : ReactiveObject where T : class, new()
{
    private static T _instance;

    public static T Instance 
    {
        get
        {
            if (_instance == null)
                _instance = new T();

            return _instance;
        }
    }
}

public class PersistentSingleton<T> : ReactiveObject where T : class, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                
                var path = "DB" + Path.DirectorySeparatorChar +   typeof(T).Name;

                var file = new FileInfo(path);
                if (file.Exists)
                {
                    _instance = JsonSerializer.Deserialize<T>(File.ReadAllText(path));
                }
                else
                {
                    _instance = new T(); 
                }
            }
            return _instance;
        }
    }

    public void Save()
    {
        var dir = new DirectoryInfo("DB");

        if (!dir.Exists)
            dir.Create();

        var path = "DB" + Path.DirectorySeparatorChar +  typeof(T).Name;

        var file = new FileInfo(path);
        if(file.Exists)
            file.Delete();

        File.WriteAllText(path, JsonSerializer.Serialize(_instance));
    }
}