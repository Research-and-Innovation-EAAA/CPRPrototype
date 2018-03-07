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
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    NavigationPage.SetHasNavigationBar(this, true);
                    break;
                case Device.Android:
                    NavigationPage.SetHasNavigationBar(this, false);
                    break;
            }

            BindingContext = _viewmodel;
            DataTemplate template = new DataTemplate(typeof(ImageCell));
            template.SetBinding(ImageCell.ImageSourceProperty, "");
            template.SetBinding(ImageCell.TextProperty, "HistoryName");
            template.SetBinding(ImageCell.DetailProperty, "CPRHistoryTotalCycles");
            template.SetValue(ImageCell.TextColorProperty,Color.Black);
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
            if (e.SelectedItem == null) // Do nothing on deselect
            {
                return;
            }

            var temp = e.SelectedItem as Service.CPRHistory;
            int intTemp = temp.Id;

            // deselects the item from the list
            var listview = (ListView)sender;
            listview.SelectedItem = null;
            // Insert call for all connected to CPRHIstory here.

            await Navigation.PushAsync(new LogDetailPage(intTemp));

        }

        #endregion
    }
}