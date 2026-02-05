namespace VaughnLive.Controls;

public partial class ChatWebView : ContentView
{
    public static readonly BindableProperty ChatUrlProperty =
        BindableProperty.Create(nameof(ChatUrl), typeof(string), typeof(ChatWebView),
            propertyChanged: OnChatUrlChanged);

    public string ChatUrl
    {
        get => (string)GetValue(ChatUrlProperty);
        set => SetValue(ChatUrlProperty, value);
    }

    public ChatWebView()
    {
        InitializeComponent();
        ChatWeb.Navigating += OnNavigating;
    }

    private static void OnChatUrlChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ChatWebView webView && newValue is string url && !string.IsNullOrEmpty(url))
        {
            webView.ChatWeb.Source = new UrlWebViewSource { Url = url };
        }
    }

    private void OnNavigating(object? sender, WebNavigatingEventArgs e)
    {
        // Handle JavaScript callbacks from chat
        if (e.Url.StartsWith("vaughnlive://"))
        {
            e.Cancel = true;
            HandleChatCallback(e.Url);
        }
    }

    private void HandleChatCallback(string url)
    {
        var uri = new Uri(url);

        switch (uri.Host)
        {
            case "emote-picker":
                // Open native emote picker
                break;
            case "notification":
                // Handle chat notification
                break;
        }
    }

    public async Task SendMessageAsync(string message)
    {
        var escapedMessage = message.Replace("'", "\\'");
        await ChatWeb.EvaluateJavaScriptAsync($"sendChatMessage('{escapedMessage}')");
    }
}
