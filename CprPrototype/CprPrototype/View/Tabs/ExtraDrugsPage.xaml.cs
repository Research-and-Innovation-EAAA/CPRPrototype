using CprPrototype.ViewModel;
using CprPrototype.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using CprPrototype.View;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDrugsPage : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance;
        private const string shockGiven = " givet";
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
                if (await CheckMedicinActionSheet() == "Bekræft")
                {
                    viewModel.History.AddItem(label.Text + shockGiven, imagesource);
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

        private async Task<string> CheckMedicinActionSheet()
        {
            string answer = null;

            while (answer == null)
            {
                answer = await DisplayActionSheet("Medicin givet? ", "Anuller", null, "Bekræft");
            }

            return answer;
        }
    }
}