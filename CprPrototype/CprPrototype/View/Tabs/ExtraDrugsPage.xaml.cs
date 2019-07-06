using CprPrototype.ViewModel;
using CprPrototype.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using CprPrototype.View;
using CprPrototype.Service;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDrugsPage : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance;
        private const string imagesource = "icon_medicin.png";
        public static BaseViewModel _bvm = BaseViewModel.Instance;
        ExDrugsModel vm;
        private object _syncLock = new object();
        bool _isInCall = false;

        public ExtraDrugsPage()
        {
            InitializeComponent();

            vm = new ExDrugsModel();
            lstDrugs.ItemsSource = vm.Exdrugs;
           
        }
        public async void Button_Clicked(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            var label = temp.Parent.FindByName<Label>("lblname");


            lock (_syncLock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }
            try
            {
                if (await CheckMedicinActionSheet() == "Confirm")
                {
                    viewModel.History.AddItem(label.Text + " " + Translator.Instance.Translate("given"), imagesource);
                }
            }
            finally
            {
                lock (_syncLock)
                {
                    _isInCall = false;
                }
            }

        }

        private string Translate(string text)
        {
            return CprPrototype.Service.Translator.Instance.Translate(text);
        }

        private async Task<string> CheckMedicinActionSheet()
        {
            string answer = null;

            while (answer == null)
            {
                string displayAnswer = await DisplayActionSheet(
                    Translate("QuestionMedicineGiven"), 
                    Translate("Cancel"), null,
                    Translate("Confirm"));

                if (displayAnswer == Translate("QuestionMedicineGiven"))
                    answer = "QuestionMedicineGiven";
                else if (displayAnswer == Translate("Cancel"))
                    answer = "Cancel";
                else if (displayAnswer == Translate("Confirm"))
                    answer = "Confirm";
            }

            return answer;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ListView l = (ListView)sender;
        }
    }
}