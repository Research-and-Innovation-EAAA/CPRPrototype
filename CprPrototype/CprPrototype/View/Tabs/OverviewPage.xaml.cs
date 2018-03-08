using CprPrototype.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CprPrototype;
using CprPrototype.Service;
using System;
using System.Threading.Tasks;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        #region Properties


        private BaseViewModel _viewModel = BaseViewModel.Instance;
        private Database.DatabaseHelper _database = Database.DatabaseHelper.Instance;

        private object _synclock = new object();
        bool _isInCall = false;

        #endregion

        #region Contructor

        public OverviewPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = _viewModel;
            DataTemplate template = new DataTemplate(typeof(ImageCell));
            template.SetBinding(ImageCell.ImageSourceProperty, "ImageSource");
            template.SetBinding(ImageCell.TextProperty, "Name");
            template.SetBinding(ImageCell.DetailProperty, "DateTimeString");
            template.SetValue(ImageCell.TextColorProperty, Color.Black);
            listView.ItemTemplate = template;
            listView.BindingContext = _viewModel;
            listView.ItemsSource = _viewModel.History.Entries;


            btnlog.SetBinding(IsVisibleProperty, nameof(_viewModel.IsLogAvailable));
            btnROSC.SetBinding(IsVisibleProperty, nameof(_viewModel.IsDoneAvailable));
            btnDoed.SetBinding(IsVisibleProperty, nameof(_viewModel.IsDoneAvailable));
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// Inserts a <see cref="CPRHistory"/> into the database.
        /// </summary>
        /// <returns></returns>
        private async Task InsertCPRHistoryIntoDB()
        {
            await _database.CreateTablesAsync();
            _viewModel.History.CPRHistoryTotalCycles = "Antal cyklusser: " + _viewModel.TotalElapsedCycles;
            _viewModel.History.AttemptFinished = DateTime.Now;
            _viewModel.History.HistoryName = _viewModel.History.AttemptStarted.Date.ToString("dd/MM/yy ") + " " + _viewModel.History.AttemptStarted.ToString(" HH:mm") + " - " + _viewModel.History.AttemptFinished.ToString("HH:mm");
            await _database.InsertCPRHistoryAsync(_viewModel.History);

            // Insert Entries into DB
            List<CPRHistoryEntry> updatedList = new List<CPRHistoryEntry>(_viewModel.History.Entries);
            foreach (var item in updatedList)
            {
                item.CPRHistoryId = _viewModel.History.Id;
            }

            await _database.InsertListOfEntries(updatedList);

        }

        /// <summary>
        /// Occures when the user pushes the RUC button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void BtnROSC_Clicked(object sender, EventArgs e)
        {
            lock (_synclock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }

            try
            {
                _viewModel.History.AddItem("Genoplivning Afsluttet - Patient ROSC","icon_alive.png");
                _viewModel.History.ImageSource = "icon_alive.png";
                await InsertCPRHistoryIntoDB();
                _viewModel.EndAlgorithm();
            }
            finally
            {
                lock (_synclock)
                {
                    _isInCall = false;
                }
            }
        }

        /// <summary>
        /// Occures when the user declares the patient dead.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void BtnDoed_Clicked(object sender, EventArgs e)
        {
            lock (_synclock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }

            try
            {
                _viewModel.History.AddItem("Genoplivning Afsluttet - Patient DØD","icon_dead.png");
                _viewModel.History.ImageSource = "icon_dead.png";
                await InsertCPRHistoryIntoDB();
                _viewModel.EndAlgorithm();
            }
            finally
            {
                lock (_synclock)
                {
                    _isInCall = false;
                }
            }

        }

        /// <summary>
        /// Occures when the user pushes the log-button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void GoToLogPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogPage());
        }

        #endregion
    }
}