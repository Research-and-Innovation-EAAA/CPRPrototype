using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CprPrototype;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage
    {
        #region Properties

        private ViewModel.BaseViewModel _viewmodel = ViewModel.BaseViewModel.Instance;
        private Database.DatabaseHelper _database = Database.DatabaseHelper.Instance;

        #endregion

        #region Constructor

        public LogPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = _viewmodel;
            DataTemplate template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "HistoryName");
            template.SetBinding(TextCell.DetailProperty, "CPRHistoryTotalCycles");
            template.SetValue(TextCell.TextColorProperty,Color.Black);
            loglist.ItemTemplate = template;
            loglist.BindingContext = _viewmodel;

            GetCPRHistory();
        }

        #endregion

        #region Methods & Events

        private async void GetCPRHistory()
        {
            loglist.ItemsSource = await _database.GetCPRHistoriesAsync();
        }


        /// <summary>
        /// Handler for when an item is selected on the listview. Handles both selection/deselection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void  OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem == null) // Do nothing on deselect
            {
                return;
            }

            var temp = e.SelectedItem as Service.CPRHistory;
            int intTemp = temp.Id;
            // Insert call for all connected to CPRHIstory here.

            await Navigation.PushAsync(new LogDetailPage(intTemp));

        }

        #endregion
    }
}