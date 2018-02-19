using CprPrototype.Database;
using CprPrototype.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPRPage : ContentPage
    {
        #region Properties

        private BaseViewModel _viewModel = BaseViewModel.Instance;
        private DatabaseHelper _databaseHelper = DatabaseHelper.Instance;

        private const string actionSheetTitle = "STØD GIVET?";
        private const string shockGiven = "GIVET";
        private const string shockNotGiven = "IKKE-GIVET";

        
        #endregion

        #region Construction & Initialisation

        public CPRPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;

            // ListView
            DataTemplate template = new DataTemplate(typeof(DrugCell));
            template.SetBinding(DrugCell.NameProperty, "DrugDoseString");
            template.SetBinding(DrugCell.TimeRemainingProperty, "TimeRemainingString");
            template.SetBinding(DrugCell.ButtonCommandInjectedProperty, "DrugInjectedCommand");
            template.SetBinding(DrugCell.ButtonCommandIgnoreProperty, "DrugIgnoredCommand");
            template.SetBinding(DrugCell.TextColorProperty, "TextColor");
            template.SetBinding(DrugCell.BackgroundColorProperty, "BackgroundColor");
            

            listView.HasUnevenRows = true;
            listView.ItemTemplate = template;
            listView.ItemsSource = _viewModel.NotificationQueue;
            listView.BindingContext = _viewModel;

            // Initialize Algorithm and UI:
            _viewModel.InitAlgorithmBase();
        }
        
        #endregion

        #region Methods & Event Handlers

        /// <summary>
        /// Refreshes the two minute timespan for HLR-countdown.
        /// </summary>
        private void RefreshStepTime()
        {
            _viewModel.AlgorithmBase.StepTime = TimeSpan.FromMinutes(2);
            _viewModel.StepTime = _viewModel.AlgorithmBase.StepTime;
        }

        /// <summary>
        /// Enables the UI after the first click 
        /// by making the hidden elements visible.
        /// </summary>
        private void EnableUI()
        {
            lblTotalElapsedCycles.IsVisible = true;
            lblTotalTime.IsVisible = true;
            lblHeart.IsVisible = true;
            lblStepDescription.IsVisible = true;
            lblStepTime.IsVisible = true;
            lblMedicinReminders.IsVisible = true;
        }

        /// <summary>
        /// Displays an actionsheet and presents the user for possible choices.
        /// Loops until the user has chosen one of the outcomes.
        /// </summary>
        /// <returns>returns the answer the user has given in form of a string</returns>
        private async Task<string> CheckShockGivenActionSheet()
        {
            string answer = null;

            while (answer == null)
            {
                answer = await DisplayActionSheet(actionSheetTitle, null, null, shockGiven, shockNotGiven);
            }
            
            return answer;
        }

        /// <summary>
        /// Occures when Shockable Button is clicked.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private async void ShockableButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }

            DatabaseTest();

            _viewModel.History.AddItem("Rytme vurderet - Stødbar");


            var answer = await CheckShockGivenActionSheet();

            _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.Shockable);
            _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.Shockable);
            _viewModel.AdvanceAlgorithm(answer);
            RefreshStepTime();
        }

        /// <summary>
        /// Occures when the NonShockable Button is clicked.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private async void NShockableButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }
            _viewModel.History.AddItem("Rytme vurderet - Ikke-Stødbar");

            var answer = await CheckShockGivenActionSheet();

            _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.NonShockable);
            _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.NonShockable);
            _viewModel.AdvanceAlgorithm(answer);
            RefreshStepTime();
        }


        private async void DatabaseTest()
        {
            var db = _databaseHelper.DBConnection;
            // Create tables:
            //await db.CreateTableAsync<Service.CPRHistory>();
            //await db.CreateTableAsync<Service.CPRHistoryEntry>();

            await _databaseHelper.CreateTablesAsync();

            // Create CPRHistory:
            var firstHistory = new Service.CPRHistory();
            var secondHistory = new Service.CPRHistory();
            //_databaseHelper.InsertCPRHistory(firstHistory);

            await _databaseHelper.InsertCPRHistory(firstHistory);
            await _databaseHelper.InsertCPRHistory(secondHistory);

            // Create CPRHistoryEntries:
            Service.CPRHistoryEntry entry1 = new Service.CPRHistoryEntry { Name = "Dummy1", CPRHistoryId = firstHistory.Id};
            Service.CPRHistoryEntry entry2 = new Service.CPRHistoryEntry { Name = "Dummy2", CPRHistoryId = firstHistory.Id };
            Service.CPRHistoryEntry entry3 = new Service.CPRHistoryEntry { Name = "Dummy3", CPRHistoryId = firstHistory.Id };

            await _databaseHelper.InsertCPREntry(entry1);
            await _databaseHelper.InsertCPREntry(entry2);
            await _databaseHelper.InsertCPREntry(entry3);

            //await db.InsertAsync(entry1);
            //await db.InsertAsync(entry2);
            //await db.InsertAsync(entry3);


            Service.CPRHistoryEntry entry4 = new Service.CPRHistoryEntry { Name = "Dummy4", CPRHistoryId = secondHistory.Id };
            Service.CPRHistoryEntry entry5 = new Service.CPRHistoryEntry { Name = "Dummy5", CPRHistoryId = secondHistory.Id };
            Service.CPRHistoryEntry entry6 = new Service.CPRHistoryEntry { Name = "Dummy6", CPRHistoryId = secondHistory.Id };

            await _databaseHelper.InsertCPREntry(entry4);
            await _databaseHelper.InsertCPREntry(entry5);
            await _databaseHelper.InsertCPREntry(entry6);

            var temp = await _databaseHelper.GetEntriesConnectedToCPRHistory(firstHistory.Id);

            List<Service.CPRHistoryEntry> list = await _databaseHelper.GetAllEntriesFromCPRHistoryAsync();


            Debug.WriteLine(list.Capacity);

            foreach (var entry in list)
            {
                Debug.WriteLine(entry.Name);
            }
        }
        #endregion
    }
}
