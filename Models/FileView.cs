using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Microsoft.Security;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DeadmansFileshareAppCSharp.Views;

namespace DeadmansFileshareAppCSharp.Models
{
    internal class FileView
    {
        public ObservableCollection<File>? Files { get; set; }

        public FileView()
        {
            LoadFiles();
        }

        private async void LoadFiles()
        {
            String CRED_NAME = AppConfig.CRED_NAME;
            String userName = AppConfig.userName;

            if(String.IsNullOrEmpty(userName))
            {
                return;
            }

            var vault = new PasswordVault();
            var cred = vault.Retrieve(CRED_NAME, userName);

            if(cred == null)
            {
                // error, force logout
                return;
            }

            cred.RetrievePassword();

            await GetFiles(cred.Password);
        }

        private async Task<bool> GetFiles(String token)
        {
            String API_URI = AppConfig.API_URI;

            using var client = new HttpClient();

            var payload = new { token };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(API_URI + "/files/getAllFiles", content);

            if(response.IsSuccessStatusCode)
            {
                String jsonResponse = await response.Content.ReadAsStringAsync();
                List<File>? fileList = JsonSerializer.Deserialize<List<File>>(jsonResponse);

                if (fileList == null || fileList.Count == 0) return false;

                System.Diagnostics.Debug.WriteLine(jsonResponse);


                Files = new ObservableCollection<File>(fileList);

                Console.WriteLine(jsonResponse);
                return true;
            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Couldn't read files...");

                return false;
            }
        }

    }
}
