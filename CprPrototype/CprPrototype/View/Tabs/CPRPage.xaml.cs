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

        private object _syncLock = new object();
        bool _isInCall = false;

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

            // Binding all labels and their visibleproperty
            lblTotalElapsedCycles.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblTotalTime.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblHeart.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblStepDescription.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblStepTime.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblMedicinReminders.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
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
            lock (_syncLock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }

            try
            {

                if (_viewModel.TotalElapsedCycles == 0)
                {
                    _viewModel.History.AttemptStarted = DateTime.Now;
                }
                //DatabaseTest();
                _viewModel.History.AddItem("Rytme vurderet - Stødbar", "syringe.png");

                var answer = await CheckShockGivenActionSheet();
                _viewModel.IsDoneAvailable = true;
                _viewModel.IsLogAvailable = false;
                _viewModel.EnableDisableUI = true;
                _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.Shockable);
                _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.Shockable);
                _viewModel.AdvanceAlgorithm(answer);
                RefreshStepTime();
            }
            finally
            {
                lock (_syncLock)
                {
                    _isInCall = false;
                }
            }
        }

        /// <summary>
        /// Occures when the NonShockable Button is clicked.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private async void NShockableButton_Clicked(object sender, EventArgs e)
        {
            lock (_syncLock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }

            try
            {
                if (_viewModel.TotalElapsedCycles == 0)
                {
                    _viewModel.History.AttemptStarted = DateTime.Now;
                }
                _viewModel.History.AddItem("Rytme vurderet - Ikke-Stødbar", "syringe.png");

                var answer = await CheckShockGivenActionSheet();
                _viewModel.IsDoneAvailable = true;
                _viewModel.IsLogAvailable = false;
                _viewModel.EnableDisableUI = true;
                _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.NonShockable);
                _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.NonShockable);
                _viewModel.AdvanceAlgorithm(answer);
                RefreshStepTime();
            }
            finally
            {
                lock (_syncLock)
                {
                    _isInCall = false;
                }
            }
        }

        //private void RemoveNotifications()
        //{
        //    _viewModel.AlgorithmBase.RemoveDrugsFromQueue(_viewModel.NotificationQueue);
        //}

        // Tester method for database, which should be deleted upon deployment
        private async void DatabaseTest()
        {
            var db = _databaseHelper.DBConnection;
            // Create tables:
            await _databaseHelper.CreateTablesAsync();

            // Create CPRHistory:
            var firstHistory = new Service.CPRHistory();
            var secondHistory = new Service.CPRHistory();

            await _databaseHelper.InsertCPRHistoryAsync(firstHistory);
            await _databaseHelper.InsertCPRHistoryAsync(secondHistory);

            // Create CPRHistoryEntries:
            Service.CPRHistoryEntry entry1 = new Service.CPRHistoryEntry { Name = "Dummy1", CPRHistoryId = firstHistory.Id };
            Service.CPRHistoryEntry entry2 = new Service.CPRHistoryEntry { Name = "Dummy2", CPRHistoryId = firstHistory.Id };
            Service.CPRHistoryEntry entry3 = new Service.CPRHistoryEntry { Name = "Dummy3", CPRHistoryId = firstHistory.Id };

            await _databaseHelper.InsertCPREntryAsync(entry1);
            await _databaseHelper.InsertCPREntryAsync(entry2);
            await _databaseHelper.InsertCPREntryAsync(entry3);

            Service.CPRHistoryEntry entry4 = new Service.CPRHistoryEntry { Name = "Dummy4", CPRHistoryId = secondHistory.Id };
            Service.CPRHistoryEntry entry5 = new Service.CPRHistoryEntry { Name = "Dummy5", CPRHistoryId = secondHistory.Id };
            Service.CPRHistoryEntry entry6 = new Service.CPRHistoryEntry { Name = "Dummy6", CPRHistoryId = secondHistory.Id };

            await _databaseHelper.InsertCPREntryAsync(entry4);
            await _databaseHelper.InsertCPREntryAsync(entry5);
            await _databaseHelper.InsertCPREntryAsync(entry6);

            var temp = await _databaseHelper.GetEntriesConnectedToCPRHistoryAsync(firstHistory.Id);

        }
        #endregion
    }
}
