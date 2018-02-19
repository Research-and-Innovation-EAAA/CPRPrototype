using System.IO;
using CprPrototype.Droid.Helpers;
using CprPrototype.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace CprPrototype.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}