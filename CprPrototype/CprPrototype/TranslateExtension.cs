using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype
{
    // You exclude the 'Extension' suffix when using in XAML
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public TranslateExtension()
        {
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return CprPrototype.Service.Translator.Instance.Translate(Text);
        }
    }
}