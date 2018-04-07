using System.Windows.Input;
using Xamarin.Forms;

namespace CprPrototype.View
{
    public class DrugCell : ViewCell
    {
        #region Properties 

        Label lblName, lblTime;
        Button btnInjected, btnIgnore;
        StackLayout labelLayout, drugCellLayout;

        // Bindable properties connected to the different elements in the DrugCell
        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(DrugCell), null, propertyChanged: OnDrugDoseStringChanged);
        public static readonly BindableProperty TimeRemainingStringProperty = BindableProperty.Create(nameof(TimeRemainingString), typeof(string), typeof(DrugCell), null, propertyChanged: OnTimeRemainingStringChanged);
        public static readonly BindableProperty ButtonCommandInjectedProperty = BindableProperty.Create(nameof(DrugInjectedCommand), typeof(ICommand), typeof(DrugCell));
        public static readonly BindableProperty ButtonCommandIgnoreProperty = BindableProperty.Create(nameof(DrugIgnoredCommand), typeof(ICommand), typeof(DrugCell));
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(DrugCell), Color.LightGray, propertyChanged: OnBackgroundColorChanged);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(DrugCell), Color.Black);

        /// <summary>
        /// Gets or sets the Name connected with the BindableProperty (ex. DrugDoseString)
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Time remaining connected to the the BindableProperty (ex. TimeRemainingString)
        /// </summary>
        public string TimeRemainingString
        {
            get { return (string)GetValue(TimeRemainingStringProperty); }
            set { SetValue(TimeRemainingStringProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ICommand value connected to BindableProperty 
        /// </summary>
        public ICommand DrugInjectedCommand
        {
            get { return (ICommand)GetValue(ButtonCommandInjectedProperty); }
            set { SetValue(ButtonCommandInjectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ICommand value connected to BindableProperty
        /// </summary>
        public ICommand DrugIgnoredCommand
        {
            get { return (ICommand)GetValue(ButtonCommandIgnoreProperty); }
            set { SetValue(ButtonCommandIgnoreProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Textcolor connected to BindableProperty
        /// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        #endregion

        #region Constructor & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugCell"/> class
        /// </summary>
        /// <remarks>
        /// {PWM} - Do notice that the construction of the viewcell could also be done in xaml.</remarks>
        public DrugCell()
        {

            drugCellLayout = new StackLayout
            {
                Spacing = 2,
                Margin = new Thickness(1),
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = BackgroundColor
            };

            //========================================================================
            // Left side of drugCellLayout:
            //========================================================================

            lblName = new Label()
            {
                FontAttributes = FontAttributes.Bold
            };

            lblTime = new Label
            {
                TextColor = TextColor
            };

            labelLayout = new StackLayout
            {
                Spacing = 2,
                Margin = new Thickness(15, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = BackgroundColor

            };
            labelLayout.Children.Add(lblName);
            labelLayout.Children.Add(lblTime);

            drugCellLayout.Children.Add(labelLayout);

            //========================================================================
            // Right side of drugCellLayout:
            //========================================================================

            btnInjected = new Button
            {
                Text = "Giv",
                WidthRequest = 70,
                HorizontalOptions = LayoutOptions.End
            };
            if (Device.RuntimePlatform == Device.iOS) { btnInjected.BackgroundColor = Color.FromHex("#E0E0E0"); btnInjected.TextColor = Color.Black;}


            btnIgnore = new Button
            {
                Image = "cross.png",
                WidthRequest = 70,
                HorizontalOptions = LayoutOptions.End
            };
            if (Device.RuntimePlatform == Device.iOS) { btnIgnore.BackgroundColor = Color.FromHex("#E0E0E0"); btnIgnore.TextColor = Color.Black; }

            drugCellLayout.Children.Add(btnInjected);
            drugCellLayout.Children.Add(btnIgnore);

            View = drugCellLayout;
        }
        #endregion

        #region Events

        /// <summary>
        /// Binds the layout to the code behind, and updates if changes are made to the values
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                lblName.Text = Name;
                lblName.TextColor = TextColor;
                lblTime.Text = TimeRemainingString;
                lblTime.TextColor = TextColor;
                btnInjected.Command = DrugInjectedCommand;
                btnIgnore.Command = DrugIgnoredCommand;
                labelLayout.BackgroundColor = BackgroundColor;
                drugCellLayout.BackgroundColor = BackgroundColor;
            }
        }

        static void OnDrugDoseStringChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var drugCell = bindable as DrugCell;
            drugCell.lblName.Text = (string)newValue;
        }

        /// <summary>
        /// Occures when the timeRemainingString is changed
        /// </summary>
        /// <remarks>
        /// {PWM} - This is done for each instance of the drugcell created.
        /// </remarks>
        /// <param name="bindable">The <see cref="DrugCell"/> itself.</param>
        /// <param name="oldValue">The value the <see cref="DrugCell"/> holds on eventcall.</param>
        /// <param name="newValue">The new value that triggered the eventcall.</param>
        static void OnTimeRemainingStringChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DrugCell drugCell)
            {
                if (newValue is string val)
                    drugCell.lblTime.Text = val;
            }
        }


        static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var drugCell = bindable as DrugCell;
            drugCell.drugCellLayout.BackgroundColor = (Color)newValue;
            drugCell.labelLayout.BackgroundColor = (Color)newValue;
        }

        #endregion

    }
}
