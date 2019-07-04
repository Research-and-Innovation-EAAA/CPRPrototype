using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace CprPrototype.Service
{
    public class Translator
    {
        private static readonly Lazy<Translator>
        lazy =
        new Lazy<Translator>
            (() => new Translator());

        public static Translator Instance { get { return lazy.Value; } }

        readonly CultureInfo ci = null;
        const string ResourceId = "CprPrototype.Resx.AppResources"; 
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(Translator)).Assembly));

        public Translator()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Translate(string Text)
        {
            if (Text == null)
                return string.Empty;

            var translation = ResMgr.Value.GetString(ci.Name + ":" + Text, ci);
            if (translation == null)
            {
                translation = ResMgr.Value.GetString(Text, ci);
                if (translation == null)
                    translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
            }
            return translation;
        }
    }
}
