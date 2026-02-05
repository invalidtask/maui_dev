namespace VaughnLive.Views;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage()
    {
        InitializeComponent();
    }

    private async void OnChangeAvatarClicked(object? sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select Avatar"
            });

            if (result != null)
            {
                AvatarImage.Source = ImageSource.FromFile(result.FullPath);
                // Upload avatar to server
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to select photo: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        // Save profile changes
        await DisplayAlert("Success", "Profile updated successfully", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
