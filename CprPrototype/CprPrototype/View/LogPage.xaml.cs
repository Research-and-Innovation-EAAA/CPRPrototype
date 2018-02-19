using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CprPrototype;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage
    {
        private ViewModel.BaseViewModel _viewmodel = ViewModel.BaseViewModel.Instance;

        public LogPage()
        {
            InitializeComponent();
            
        }
    }
}