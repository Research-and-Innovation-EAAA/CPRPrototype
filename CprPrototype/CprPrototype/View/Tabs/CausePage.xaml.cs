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
        public TextCell  SpecialCases {get; set;}
        public CausePage()
        {
            InitializeComponent();
            DataTemplate datatemple = new DataTemplate(typeof(TextCell));
            datatemple.SetBinding(TextCell.TextProperty, "Text");
            datatemple.SetValue(TextCell.TextColorProperty, Color.Black);
            CauseList.ItemTemplate = datatemple;
            CauseList.ItemsSource = GetSpecialCases();
        }
        public List<TextCell> GetSpecialCases()
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
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ListView l = (ListView)sender;
        }
    }
}