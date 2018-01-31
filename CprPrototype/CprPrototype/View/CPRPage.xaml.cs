using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPRPage : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance;

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
            UpdateUI();
        }

        /// <summary>
        /// Updates UI elements based on the current step in the algorithm.
        /// </summary>
        private void UpdateUI()
        {

                lblName.IsVisible = true;
                lblDescription.IsVisible = false;
                lblStepTime.IsVisible = true;

            if (viewModel.CurrentPosition.RythmStyle == Model.RythmStyle.NonShockable)
            {
                if (viewModel.TotalElapsedCycles > 0)
                {
                    lblName.IsVisible = false;
                }

                lblName.IsVisible = true;
                lblDescription.IsVisible = false;
                lblStepTime.IsVisible = true;
            }

            if (viewModel.Algorithm.FirstStep == viewModel.Algorithm.CurrentStep)
            {
                lblName.IsVisible = true;
                lblDescription.IsVisible = true;
                btnShockable.IsVisible = true;
                btnNShockable.IsVisible = true;
                btnNextStep.IsVisible = false;
                lblStepTime.IsVisible = false;
            }
            else
            {
                btnShockable.IsVisible = false;
                btnNShockable.IsVisible = false;
                btnNextStep.IsVisible = true;
            }

        }

        private void HelperMethodRefresh()
        {
            viewModel.Algorithm.StepTime = TimeSpan.FromMinutes(2);
            viewModel.StepTime = viewModel.Algorithm.StepTime;
        }

        /// <summary>
        /// Handler for Silent Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void ShockableButton_Clicked(object sender, EventArgs e)
        {
            viewModel.Algorithm.BeginSequence(Model.RythmStyle.Shockable);
            viewModel.Algorithm.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.Shockable);
            viewModel.AdvanceAlgorithm();
            HelperMethodRefresh();
            UpdateUI();
        }

        /// <summary>
        /// Handler for NonShockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void NShockableButton_Clicked(object sender, EventArgs e)
        {
            viewModel.Algorithm.BeginSequence(Model.RythmStyle.NonShockable);
            viewModel.Algorithm.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.NonShockable);
            viewModel.AdvanceAlgorithm();
            HelperMethodRefresh();
            UpdateUI();
        }

        /// <summary>
        /// Handler for Shockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void RhythmButton_Clicked(object sender, EventArgs e)
        {
            viewModel.AdvanceAlgorithm();
            UpdateUI();
        }
    }
}