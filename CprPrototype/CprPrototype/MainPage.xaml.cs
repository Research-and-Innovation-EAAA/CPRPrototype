using System;
using Xamarin.Forms;
using CprPrototype.View;
using CprPrototype.Model;
using CprPrototype.ViewModel;

namespace CprPrototype
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler for Silent Button clicked event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void SilentButton_Clicked(object sender, EventArgs e)
        {
            if(swSoundMode.IsToggled)
            {
                BaseViewModel.Instance().Mode = InteractionMode.Sound;
            }
            else
            {
                BaseViewModel.Instance().Mode = InteractionMode.Silent;
            }
            BaseViewModel.Instance().InitAlgorithmBase(StepSize.Small);
            Application.Current.MainPage = new MasterTabbedPage();
        }

        /// <summary>
        /// Handler for Sound button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundButton_Clicked(object sender, EventArgs e)
        {
            if (swSoundMode.IsToggled)
            {
                BaseViewModel.Instance().Mode = InteractionMode.Sound;
            }
            else
            {
                BaseViewModel.Instance().Mode = InteractionMode.Silent;
            }
            BaseViewModel.Instance().InitAlgorithmBase(StepSize.Big);
            Application.Current.MainPage = new MasterTabbedPage();
        }
    }
}
