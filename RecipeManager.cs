using System;
using System.IO;

public class RecipeManager
{
    private static readonly Lazy<RecipeManager> lazyInstance = new(() => new RecipeManager());
    private string? _ImageFolderPath;
    private static RecipeContext _DB = new();
 

    private RecipeManager(){
        _DB.Database.EnsureCreated();
        getImageFolder();
    }

    public string? ImageFolderPath{
        get => _ImageFolderPath;
        set {
            if (Directory.Exists(value)){
                _ImageFolderPath = value;
            }
            else {
                Console.WriteLine("Invalid Image Path");
            }
        }
    }

    public static RecipeContext DB{
        get => _DB;
    }

    private void getImageFolder(){
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        string SettingsPath = Path.Join(path, "RecipeBrowser/RecipeBrowserSettings.txt");
        if (!File.Exists(SettingsPath)){
            ImageFolderPath = null;
            StreamWriter sw = File.AppendText(SettingsPath);
            sw.Close();
            Console.WriteLine("Created Settings File");
            return;
        }
        StreamReader file = new(SettingsPath);
        string? imageUrl = file.ReadLine();
        if (Directory.Exists(imageUrl)){
            ImageFolderPath = imageUrl;
        } else {
            ImageFolderPath = null;
            Console.WriteLine("Invalid Image Path");
        }
        file.Close();
    }


    public static RecipeManager Instance{
            get
            {
                return lazyInstance.Value;
            }
    }
}
