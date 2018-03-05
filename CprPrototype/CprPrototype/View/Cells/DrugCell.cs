using System;
using System.Diagnostics;
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

        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(DrugCell));
        public static readonly BindableProperty TimeRemainingStringProperty = BindableProperty.Create(nameof(TimeRemainingString), typeof(string), typeof(DrugCell), null, propertyChanged: OnTimeRemainingStringChanged);
        public static readonly BindableProperty ButtonCommandInjectedProperty = BindableProperty.Create(nameof(DrugInjectedCommand), typeof(ICommand), typeof(DrugCell));
        public static readonly BindableProperty ButtonCommandIgnoreProperty = BindableProperty.Create(nameof(DrugIgnoredCommand), typeof(ICommand), typeof(DrugCell));
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(DrugCell), Color.LightGray);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(DrugCell), Color.Black);

        /// <summary>
        /// Gets or sets the name of the bound value // Ikke god
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public ICommand DrugInjectedCommand
        {
            get { return (ICommand)GetValue(ButtonCommandInjectedProperty); }
            set { SetValue(ButtonCommandInjectedProperty, value); }
        }

        public ICommand DrugIgnoredCommand
        {
            get { return (ICommand)GetValue(ButtonCommandIgnoreProperty); }
            set { SetValue(ButtonCommandIgnoreProperty, value); }
        }

        public string TimeRemainingString
        {
            get { return (string)GetValue(TimeRemainingStringProperty); }
            set { SetValue(TimeRemainingStringProperty, value); }
        }

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
        public DrugCell()
        {

            drugCellLayout = new StackLayout
            {
                Spacing = 2,
                Margin = new Thickness(3),
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

            btnIgnore = new Button
            {
                Image = "cross.png",
                WidthRequest = 70,
                HorizontalOptions = LayoutOptions.End
            };
            
            drugCellLayout.Children.Add(btnInjected);
            drugCellLayout.Children.Add(btnIgnore);

            View = drugCellLayout;
        }

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

        static void OnTimeRemainingStringChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var drugCell = bindable as DrugCell;
            drugCell.lblTime.Text = (string)newValue;
        }

        #endregion

    }
}
