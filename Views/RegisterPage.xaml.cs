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
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeadmansFileshareAppCSharp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private bool ValidateRegistrationInput(string? firstName,  string? lastName, string? email, string? username,
                string? password, string? confirmPassword)
        {
            bool isValid = true;

            if(string.IsNullOrWhiteSpace(firstName))
            {
                FirstNameIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                LastNameIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if(string.IsNullOrWhiteSpace(email))
            {
                EmailAddressIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                UsernameIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if(string.IsNullOrWhiteSpace(password))
            {
                PasswordIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if(string.IsNullOrWhiteSpace(confirmPassword))
            {
                ConfirmPasswordFieldIsNullError.Visibility = Visibility.Visible;
                isValid = false;
            }

            return isValid;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // reset error box visibility
            FirstNameIsNullError.Visibility = Visibility.Collapsed;
            LastNameIsNullError.Visibility = Visibility.Collapsed;
            EmailAddressIsNullError.Visibility = Visibility.Collapsed;
            UsernameIsNullError.Visibility = Visibility.Collapsed;
            PasswordIsNullError.Visibility = Visibility.Collapsed;
            ConfirmPasswordFieldIsNullError.Visibility = Visibility.Collapsed;

            string firstName = FirstName.Text;
            string lastName = LastName.Text;
            string email = EmailAddress.Text;
            string username = Username.Text;
            string password = Password.Password;
            string confirmPassword = ConfirmPassword.Password;

            bool isValid = ValidateRegistrationInput(firstName, lastName, email, username, password, confirmPassword);

            if (!isValid) return;

            if(!password.Equals(confirmPassword))
            {
                // password and confirmPassword do not match error
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Registration Error",
                    Content = "Password and Confirm Password fields do not match.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
                return;
            }

            var RegistrationResponse = await RegisterAsync(email, username, password);

            string jsonResponse = await RegistrationResponse.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);

            if(RegistrationResponse.IsSuccessStatusCode)
            {
                ContentDialog verifyEmail = new ContentDialog
                {
                    Title = "Registration Successful",
                    Content = $"We've sent a verification email to {email}, please click the link to verify your account.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await verifyEmail.ShowAsync();

                // get the _id and username from the response and store that in our session

                return;
            }
            else
            {
                string? error = doc.RootElement.GetProperty("error").GetString();
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = $"Registration failed\nERROR: {error}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
                return;
            }
        }

        private async Task<HttpResponseMessage> RegisterAsync(string email, string username, string password)
        {
            using var client = new HttpClient();

            string API_URI = AppConfig.API_URI;

            var payload = new { email, username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"JSON Payload: {content}");

            var response = await client.PostAsync(API_URI + "/users/addUser", content);

            return response;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartScreen));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
