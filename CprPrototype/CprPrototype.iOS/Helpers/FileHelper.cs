using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CprPrototype.Helpers;
using Foundation;
using UIKit;

namespace CprPrototype.iOS.Helpers
{
    class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}