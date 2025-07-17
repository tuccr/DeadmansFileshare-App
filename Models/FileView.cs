using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace DeadmansFileshareAppCSharp.Models
{
    internal class FileView
    {
        public ObservableCollection<File> Files { get; set; } = new ObservableCollection<File>();

        public FileView()
        {
        }
    }
}
