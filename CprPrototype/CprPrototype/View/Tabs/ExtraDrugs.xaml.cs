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
        private const string shockGiven = " givet -";
        private const string imagesource = "icon_medicin.png";

        public ExtraDrugs()
        {
            InitializeComponent();
        }

        private void btnAdrenalin_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Adrenalin + shockGiven,imagesource);
        }

        private void btnAmiodaron_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Amiodaron + shockGiven, imagesource);
        }

        private void btnBikarbonat_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Bikarbonat + shockGiven, imagesource);
        }

        private void btnCalcium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Calcium + shockGiven, imagesource);
        }

        private void btnMagnesium_Clicked(object sender, EventArgs e)
        {
            viewModel.History.AddItem(DrugType.Magnesium + shockGiven, imagesource);
        }
    }
}