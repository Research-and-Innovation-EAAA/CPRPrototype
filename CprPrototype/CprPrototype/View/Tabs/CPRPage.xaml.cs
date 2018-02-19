﻿using CprPrototype.ViewModel;
using System;
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
            _viewModel.History.AddItem("Rytme vurderet - Stødbar");

            var answer = await CheckShockGivenActionSheet();
            _viewModel.DoneIsAvailable = true;
            _viewModel.LogIsAvailable = false;
            _viewModel.EnableDisableUI = true;
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

            _viewModel.History.AddItem("Rytme vurderet - Ikke-Stødbar");

            var answer = await CheckShockGivenActionSheet();
            _viewModel.DoneIsAvailable = true;
            _viewModel.LogIsAvailable = false;
            _viewModel.EnableDisableUI = true;
            _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.NonShockable);
            _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.NonShockable);
            _viewModel.AdvanceAlgorithm(answer);
            RefreshStepTime();
        }

        #endregion
    }
}