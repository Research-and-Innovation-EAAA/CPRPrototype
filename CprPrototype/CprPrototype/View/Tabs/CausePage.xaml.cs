using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CprPrototype.Model;
using System;

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
                HList.Add(new TextCell { Text = a.ToString(), TextColor = Color.Black });
            }
            foreach (var b in CaseT)
            {
                TList.Add(new TextCell { Text = b.ToString(), TextColor = Color.Black });
            }
            sectionH.Add(HList);
            sectionT.Add(TList);
        }
    }
}