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
        }

        private void HelperMethodRefresh()
        {
            viewModel.Algorithm.StepTime = TimeSpan.FromMinutes(2);
            viewModel.StepTime = viewModel.Algorithm.StepTime;
        }

        private void EnableUI()
        {
            lblTotalElapsedCycles.IsVisible = true;
            lblTotalTime.IsVisible = true;
            lblHeart.IsVisible = true;
            lblName.IsVisible = true;
            lblStepDescription.IsVisible = true;
            lblStepTime.IsVisible = true;
            lblMedicinReminders.IsVisible = true;


        }

        /// <summary>
        /// Handler for Silent Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void ShockableButton_Clicked(object sender, EventArgs e)
        {
            if(viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }
            viewModel.Algorithm.BeginSequence(Model.RythmStyle.Shockable);
            viewModel.Algorithm.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.Shockable);
            viewModel.AdvanceAlgorithm();
            HelperMethodRefresh();
        }

        /// <summary>
        /// Handler for NonShockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void NShockableButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.TotalElapsedCycles == 0)
            {
                EnableUI();
            }
            viewModel.Algorithm.BeginSequence(Model.RythmStyle.NonShockable);
            viewModel.Algorithm.AddDrugsToQueue(viewModel.DoseQueue, Model.RythmStyle.NonShockable);
            viewModel.AdvanceAlgorithm();
            HelperMethodRefresh();
        }
    }
}