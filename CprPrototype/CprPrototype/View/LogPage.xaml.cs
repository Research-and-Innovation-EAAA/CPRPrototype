﻿using Xamarin.Forms;
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

            BindingContext = _viewmodel;
            DataTemplate template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "HistoryName");
            template.SetBinding(TextCell.DetailProperty, "CPRHistoryTotalCycles");
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

        private async Task Find()
        {

        }

        /// <summary>
        /// Handler for when an item is selected on the listview. Handles both selection/deselection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem == null) // Do nothing on deselect
            {
                return;
            }

            var temp = e.SelectedItem as Service.CPRHistory;
            int intTemp = temp.Id;

            App.Current.MainPage = new LogDetailPage(intTemp);

        }

        #endregion
    }
}