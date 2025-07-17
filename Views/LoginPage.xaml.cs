using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.


namespace DeadmansFileshareAppCSharp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void TryLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input from the Username and Password fields
            string username = Username.Text;
            string password = Password.Password; // Assuming Password is a PasswordBox

            // Basic validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                // Show an error message if fields are empty
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Login Error",
                    Content = "Please enter both username and password.",
                    CloseButtonText = "OK"
                };
                _ = errorDialog.ShowAsync();
                return;
            }

            // Placeholder for authentication logic
            bool isAuthenticated = await AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                // Navigate to the Home page upon successful login
                Frame.Navigate(typeof(Home));
            }
            else
            {
                // Show an error message if authentication fails
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Login Failed",
                    Content = "Invalid username or password.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        // Example authentication method (replace with actual logic)
        private Task<bool> AuthenticateUser(string username, string password)
        {
            return LoginAsync(username, password);
        }

        private async Task<bool> LoginAsync(string username, string password)
        {
            using var client = new HttpClient();

            string API_URI = AppConfig.API_URI;

            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"JSON Payload: {JsonSerializer.Serialize(payload)}");
            System.Diagnostics.Debug.WriteLine(API_URI);

            var response = await client.PostAsync(API_URI + "/users/login", content);

            return response.IsSuccessStatusCode;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartScreen));
        }

        private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Check if the pressed key is Enter
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // Trigger the TryLoginButton_Click method
                TryLoginButton_Click(sender, e);
            }
        }
    }
}
