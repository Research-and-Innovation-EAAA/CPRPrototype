using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPRPage : ContentPage
    {
        #region Properties
        private BaseViewModel viewModel = BaseViewModel.Instance;
        #endregion

        #region Construction & Initialisation
        public CPRPage()
        {
            InitializeComponent();
            BindingContext = viewModel;

            // ListView
            DataTemplate template = new DataTemplate(typeof(DrugCell));
            template.SetBinding(DrugCell.NameProperty, "DrugDoseString");
            template.SetBinding(DrugCell.TimeRemainingProperty, "TimeRemainingString");
            template.SetBinding(DrugCell.ButtonCommandProperty, "DrugCommand");
            template.SetBinding(DrugCell.TextColorProperty, "TextColor");
            template.SetBinding(DrugCell.BackgroundColorProperty, "BackgroundColor");

            listView.HasUnevenRows = true;
            listView.ItemTemplate = template;
            listView.ItemsSource = viewModel.DoseQueue;
            listView.BindingContext = viewModel;

            // Initialize Algorithm and UI:
            viewModel.InitAlgorithmBase();
        }
        #endregion

        #region Methods & Event Handlers

        /// <summary>
        /// Refreshes the two minute timespan for HLR-countdown.
        /// </summary>
        private void RefreshStepTime()
        {
            viewModel.AlgorithmBase.StepTime = TimeSpan.FromMinutes(2);
            viewModel.StepTime = viewModel.AlgorithmBase.StepTime;
        }

        /// <summary>
        /// Used for enabling the UI after the first click 
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
        /// [Event Handler] - Shockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void ShockableButton_Clicked(object sender, EventArgs e)
        {
            if(viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }
            viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.Shockable);
            viewModel.AlgorithmBase.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.Shockable);
            viewModel.AdvanceAlgorithm();
            RefreshStepTime();
        }

        /// <summary>
        /// [Event Handler] - NonShockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void NShockableButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }
            viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.NonShockable);
            viewModel.AlgorithmBase.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.NonShockable);
            viewModel.AdvanceAlgorithm();
            RefreshStepTime();
        }

        #endregion
    }
}