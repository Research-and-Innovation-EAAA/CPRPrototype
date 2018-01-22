using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDrugs : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance();

        public ExtraDrugs()
        {
            InitializeComponent();
        }

        private void btnBikarbonat_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem("Bikarbonat");
        }

        private void btnCalcium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem("Calcium");
        }

        private void btnMagnesium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem("Magnesium");
        }
    }
}