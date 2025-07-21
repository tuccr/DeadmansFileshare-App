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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeadmansFileshareAppCSharp.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Home : Page
{
    public Home()
    {
        InitializeComponent();
    }

    private async void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        await LogoutAsync();

        Frame.Navigate(typeof(StartScreen));
    }

    private static Task LogoutAsync()
    {
        RemoveToken(AppConfig.userName);
        AppConfig.userName = string.Empty;
        return Task.CompletedTask;
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
}
