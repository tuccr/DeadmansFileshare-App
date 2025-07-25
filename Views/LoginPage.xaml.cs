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
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Security.Credentials;

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
            UsernameIsNullError.Visibility = Visibility.Collapsed;
            PasswordIsNullError.Visibility = Visibility.Collapsed;

            string username = Username.Text;
            string password = Password.Password;

            // Basic validation
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(username))
            {
                UsernameIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!isValid) return;

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

        private Task<bool> AuthenticateUser(string username, string password)
        {
            return LoginAsync(username, password);
        }

        private async Task<bool> LoginAsync(string username, string password)
        {
            // Make HTTP connection for sending requests
            using var client = new HttpClient();

            // Load secret data
            string API_URI = AppConfig.API_URI;
            string CRED_NAME = AppConfig.CRED_NAME;

            // Create JSON payload to send to server
            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Debugging information (REMOVE LATER IF YOU SEE THIS AND REMEMBER)
            System.Diagnostics.Debug.WriteLine($"JSON Payload: {JsonSerializer.Serialize(payload)}");
            System.Diagnostics.Debug.WriteLine(API_URI);

            // POST backend server
            var response = await client.PostAsync(API_URI + "/users/login", content);

            // If our POST is successful, parse the respnse and save our received token
            if (response.IsSuccessStatusCode)
            {
                // start reading json content
                string jsonResponse = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);

                string? token = doc.RootElement.GetProperty("token").GetString();
                string? userName = doc.RootElement.GetProperty("username").GetString();

                // if we don't have a token, something went wrong and we shouldn't be here
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("TOKEN IS NULL");
                }
                else
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        return false;
                    }
                    AppConfig.userName = userName;
                    await StoreToken(token);
                }
                
                return true;
            }
            else
            {
                // could replace this if else and change function to just return the json and parse it inside the function (that way the proper error message can be displayed)
                return false;
            }
        }

        public static Task StoreToken(string token)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(AppConfig.CRED_NAME, AppConfig.userName, token));
                System.Diagnostics.Debug.WriteLine($"Token added: {token}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        public string? LoadToken(string userName)
        {
            try
            {
                var vault = new PasswordVault();
                var cred = vault.Retrieve(AppConfig.CRED_NAME, userName);
                cred.RetrievePassword();
                return cred.Password;
            }
            catch
            {
                return null;
            }
        }

        public static Task RemoveToken(string userName)
        {
            try
            {
                var vault = new PasswordVault();
                var cred = vault.Retrieve(AppConfig.CRED_NAME, AppConfig.userName);

                vault.Remove(cred);
            }
            catch
            {
                return Task.FromResult(false);
            }
            return Task.CompletedTask;
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
