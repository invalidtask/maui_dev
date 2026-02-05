using VaughnLive.Views;

namespace VaughnLive;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute("stream", typeof(StreamPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("editprofile", typeof(EditProfilePage));
        Routing.RegisterRoute("category", typeof(CategoryPage));
    }
}
