using CprPrototype.Database;
using CprPrototype.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPRPage : ContentPage
    {
        #region Properties

        private BaseViewModel _viewModel = BaseViewModel.Instance;
        private DatabaseHelper _databaseHelper = DatabaseHelper.Instance;

        private const string actionSheetTitle = "QuestionShockGiven";
        private const string shockGiven = "AnswerShockGiven";
        private const string shockNotGiven = "AnswerShockNotGiven";
        private const string cancelAction = "AnswerCancellation";

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
            template.SetBinding(DrugCell.TimeRemainingStringProperty, "TimeRemainingString");
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
            lblStepTime.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));
            lblMedicinReminders.SetBinding(IsVisibleProperty, nameof(_viewModel.EnableDisableUI));

            _viewModel.CriticalTimeChanged += this.OnCriticalTimeChanged;
        }

        #endregion

        #region Methods & Event Handlers

        private string Translate(string text)
        {
            return CprPrototype.Service.Translator.Instance.Translate(text);
        }

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
            _viewModel.IsInCriticalTime = false;
            while (answer == null)
            {
                string displayAnswer = await DisplayActionSheet(
                    Translate(actionSheetTitle), 
                    Translate(cancelAction), null, 
                    Translate(shockGiven), 
                    Translate(shockNotGiven));
                if (displayAnswer == Translate(shockGiven))
                    answer = shockGiven;
                else if (displayAnswer == Translate(shockNotGiven))
                    answer = shockNotGiven;
                else if (displayAnswer == Translate(cancelAction))
                    answer = cancelAction;
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

                var answer = await CheckShockGivenActionSheet();

                if (answer == cancelAction)
                {
                    return;
                }
                else
                {
                    if (_viewModel.TotalElapsedCycles != 0)
                    {
                        _viewModel.History.AddItem(Translate("RhythmsAssessed") + " - " +
                            Translate("Shockable"), "cardiogram.png");
                    }
                    else
                    {
                        _viewModel.History.AttemptStarted = DateTime.Now;
                        _viewModel.History.AddItem(Translate("ResuscitationStarted") + " - " +
                            Translate("Shockable"), "icon_performcpr.png");
                    }

                    _viewModel.IsDoneAvailable = true;
                    _viewModel.IsLogAvailable = false;
                    _viewModel.EnableDisableUI = true;
                    _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.Shockable);
                    _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.Shockable);
                    _viewModel.AdvanceAlgorithm(answer);
                    RefreshStepTime();
                }
            }
            finally
            {
                lock (_syncLock)
                {
                    _viewModel.IsInCriticalTime = false;
                    lowerBlock.AbortAnimation("colorchange");
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
                var answer = await CheckShockGivenActionSheet();
                if (answer == cancelAction)
                {
                    return;
                }
                else
                {
                    if (_viewModel.TotalElapsedCycles != 0)
                    {
                        _viewModel.History.AddItem(Translate("RhythmsAssessed") + " - " +
                            Translate("NonShockable"), "cardiogram.png");
                    }
                    else
                    {
                        _viewModel.History.AttemptStarted = DateTime.Now;
                        _viewModel.History.AddItem(Translate("ResuscitationStarted") + " - " + 
                            Translate("NonShockable"), "icon_performcpr.png");
                    }
                    _viewModel.IsDoneAvailable = true;
                    _viewModel.IsLogAvailable = false;
                    _viewModel.EnableDisableUI = true;
                    _viewModel.AlgorithmBase.BeginSequence(Model.RythmStyle.NonShockable);
                    _viewModel.AlgorithmBase.AddDrugsToQueue(_viewModel.NotificationQueue, Model.RythmStyle.NonShockable);
                    _viewModel.AdvanceAlgorithm(answer);
                    RefreshStepTime();
                }
            }
            finally
            {
                lock (_syncLock)
                {
                    _viewModel.IsInCriticalTime = false;
                    lowerBlock.AbortAnimation("colorchange");
                    _isInCall = false;
                }
            }
        }

        public void OnCriticalTimeChanged(object source, TimeEventArgs args)
        {
            if (args.IsInCriticalTime)
                BlinkingBackgroundAnimationLowerBlock();
            else
                lowerBlock.AbortAnimation("colorchange");
        }

        void BlinkingBackgroundAnimationLowerBlock()
        {
            var isBackgroundColored = false;

            lowerBlock.Animate(
                "colorchange",
                x =>
                {
                    if (!isBackgroundColored)
                        lowerBlock.BackgroundColor = Color.Green;
                    else
                        lowerBlock.BackgroundColor = Color.Default;

                },
                length: 1000,
                finished: delegate (double d, bool b)
                {
                    lowerBlock.BackgroundColor = Color.Default;
                },
                repeat: () =>
                {
                    isBackgroundColored = !isBackgroundColored;
                    return true;
                }
            );
        }
        #endregion
    }
}
