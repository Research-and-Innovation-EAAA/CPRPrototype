using CprPrototype.ViewModel;
using CprPrototype.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDrugs : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance;
        private const string shockGiven = " givet";
        private const string imagesource = "icon_medicin.png";

        public ExtraDrugs()
        {
            InitializeComponent();
        }

        private void BtnAdrenalin_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Adrenalin + shockGiven,imagesource);
        }

        private void BtnAmiodaron_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Amiodaron + shockGiven, imagesource);
        }

        private void BtnBikarbonat_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Bikarbonat + shockGiven, imagesource);
        }

        private void BtnCalcium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Calcium + shockGiven, imagesource);
        }

        private void BtnMagnesium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Magnesium + shockGiven, imagesource);
        }
    }
}