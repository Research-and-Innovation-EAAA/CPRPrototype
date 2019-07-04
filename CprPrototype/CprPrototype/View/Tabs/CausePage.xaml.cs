using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CprPrototype.Model;
using System;
using CprPrototype.Service;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CausePage : ContentPage
    {

        public CausePage()
        {
            InitializeComponent();

            var CaseH = Enum.GetValues(typeof(SpecialCasesH));
            var CaseT = Enum.GetValues(typeof(SpecialCasesT));

            List<TextCell> HList = new List<TextCell>();
            List<TextCell> TList = new List<TextCell>();

            foreach (var a in CaseH)
            {
                string value = Translator.Instance.Translate(a.ToString()).ToString();
                HList.Add(new TextCell { Text = value, TextColor = Color.Black });
            }
            foreach (var b in CaseT)
            {
                string value = Translator.Instance.Translate(b.ToString()).ToString();
                TList.Add(new TextCell { Text = value, TextColor = Color.Black });
            }
            sectionH.Add(HList);
            sectionT.Add(TList);
        }
    }
}