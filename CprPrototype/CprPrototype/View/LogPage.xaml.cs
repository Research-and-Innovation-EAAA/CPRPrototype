using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CprPrototype;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage
    {
        private ViewModel.BaseViewModel _viewmodel = ViewModel.BaseViewModel.Instance;
        private Database.DatabaseHelper _database = Database.DatabaseHelper.Instance;

        public LogPage()
        {
            InitializeComponent();

            BindingContext = _viewmodel;
            DataTemplate template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "HistoryName");
            template.SetBinding(TextCell.DetailProperty, "CPRHistoryTotalCycles");
            template.SetValue(TextCell.TextColorProperty,Color.Black);
            loglist.ItemTemplate = template;
            loglist.BindingContext = _viewmodel;

            GetCPRHistory();
        }
        private async void GetCPRHistory()
        {
            loglist.ItemsSource = await _database.GetCPRHistoriesAsync();
        }
    }
}