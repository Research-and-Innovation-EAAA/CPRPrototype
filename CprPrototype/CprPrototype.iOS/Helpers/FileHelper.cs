using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CprPrototype.Helpers;
using CprPrototype.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace CprPrototype.iOS.Helpers
{
    class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(path, "..", "Library");

            if(!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}