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
        public TextCell  SpecialCases {get; set;}
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

            DataTemplate datatemple = new DataTemplate(typeof(TextCell));
            datatemple.SetBinding(TextCell.TextProperty, "Text");
            datatemple.SetValue(TextCell.TextColorProperty, Color.Black);
            CauseList.ItemTemplate = datatemple;
            //CauseList.ItemsSource = GetSpecialCases();
        }
      /*  public List<TextCell> GetSpecialCases()
        {
            List<TextCell> SpecialCaseList = new List<TextCell>()
            {
                new TextCell(){Text = SpecialCasesH.Hyperkalæmi.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesH.Hypotermi.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesH.Hypovolæmi.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesH.Hypoxi.ToString(), TextColor = Color.Black },
                new TextCell(){Text = SpecialCasesT.Tamponade.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesT.Toksisk.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesT.Trombose.ToString(), TextColor = Color.Black},
                new TextCell(){Text = SpecialCasesT.Trykneumothorax.ToString(), TextColor = Color.Black}
            };
            return SpecialCaseList;
        } */

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ListView l = (ListView)sender;
        }
    }
}