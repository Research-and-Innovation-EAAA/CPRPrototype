using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CPRPage : ContentPage
	{
        private BaseViewModel viewModel = BaseViewModel.Instance();
        private bool bigStepShocked = false;

		public CPRPage ()
		{
			InitializeComponent ();
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

           UpdateUI();
		}

        /// <summary>
        /// Updates UI elements based on the current step in the algorithm.
        /// </summary>
        private void UpdateUI()
        {
            var currStep = viewModel.CurrentPosition;

            if (viewModel.StepSize == Model.StepSize.Small)
            {
                if (currStep.Name == "Stød en gang")
                {
                    lblStepTime.IsVisible = false;
                    lblDescription.IsVisible = true;
                }
                else if (currStep.Name == "HLR 2 Minutter")
                {
                    lblDescription.IsVisible = false;
                    lblStepTime.IsVisible = true;
                }

                if (currStep.NextStep != null)
                {
                    btnNextStep.Text = currStep.NextStep.Name ?? "Vurder Rytmen";
                }
            }
            else if(viewModel.StepSize == Model.StepSize.Big)
            {
                //if (currStep.RythmStyle == Model.RythmStyle.Shockable)
                //{
                //    if (!bigStepShocked)
                //    {
                lblName.IsVisible = true;
                lblDescription.IsVisible = false;
                lblStepTime.IsVisible = true;
                //        btnNextStep.Text = "Stødt en gang";
                //    }
                //    else
                //    {
                //        lblName.IsVisible = false;
                //        lblDescription.IsVisible = false;
                //        lblStepTime.IsVisible = true;

                if (currStep.NextStep != null)
                {
                    btnNextStep.Text = currStep.NextStep.Name ?? "Vurder Rytmen";
                }
                //    } 
            }
                else if (currStep.RythmStyle == Model.RythmStyle.NonShockable)
                {
                    if (viewModel.Cycles > 0)
                    {
                        lblName.IsVisible = false;
                    }

                    lblName.IsVisible = true;
                    lblDescription.IsVisible = false;
                    lblStepTime.IsVisible = true;
                }
            //}

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
            UpdateUI();
        }

        /// <summary>
        /// Handler for Shockable Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void NextStepButton_Clicked(object sender, EventArgs e)
        {
            //if (bigStepShocked == false && viewModel.StepSize == Model.StepSize.Big 
            //    && viewModel.CurrentPosition.RythmStyle == Model.RythmStyle.Shockable)
            //{
            //    bigStepShocked = true;
            //    UpdateUI();
            //}
            //else
            //{
                viewModel.AdvanceAlgorithm();
                UpdateUI();
                bigStepShocked = false;
            //}
        }
        
        /// <summary>
        /// Handler for NStep button clicked, to handle big steps
        /// in shockable rythm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextStep_Clicked(object sender, EventArgs e)
        {
            UpdateUI();
        }
    }
}