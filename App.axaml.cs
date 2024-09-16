using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Recipe_Browser;

public partial class App : Application
{
    public override void Initialize()
    {
        PrepareDatabase();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static void PrepareDatabase()
    {
        RecipeContext db = new();

        db.Database.EnsureCreated();
        
    }
}