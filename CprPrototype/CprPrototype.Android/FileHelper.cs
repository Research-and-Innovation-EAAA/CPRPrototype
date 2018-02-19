using CprPrototype.Droid;
using CprPrototype.Helpers;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]

namespace CprPrototype.Droid
{
    /// <summary>
    /// Represents the implementation of the IFileHelper defined 
    /// in the sharedLibary for Android. 
    /// </summary>
    public class FileHelper : IFileHelper 
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}