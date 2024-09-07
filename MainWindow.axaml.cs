using Avalonia.Controls;

namespace Recipe_Browser;
public partial class MainWindow : Window
{

    public RecipeContext db = new();

    public MainWindow()
    {
        InitializeComponent();

    }
}