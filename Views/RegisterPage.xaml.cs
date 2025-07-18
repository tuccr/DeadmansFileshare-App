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
