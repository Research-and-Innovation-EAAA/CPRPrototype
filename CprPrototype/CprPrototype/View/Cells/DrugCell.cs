using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CprPrototype.View
{
    public class DrugCell : ViewCell
    {
        #region Properties 

        Label lblName, lblTime;
        Button btnCommand;
        StackLayout labelLayout, labelBtnLayout;

        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(DrugCell));
        public static readonly BindableProperty TimeRemainingProperty = BindableProperty.Create(nameof(Time), typeof(string), typeof(DrugCell));
        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(nameof(DrugInjectedCommand), typeof(ICommand), typeof(DrugCell));
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(DrugCell), Color.LightGray);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(DrugCell), Color.Black);


        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public ICommand DrugInjectedCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public string Time
        {
            get { return (string)GetValue(TimeRemainingProperty); }
            set { SetValue(TimeRemainingProperty, value); }
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

        #region Construct

        public DrugCell()
        {
            // Init Views
            lblName = new Label();
            lblTime = new Label();
            lblTime.TextColor = TextColor;
            btnCommand = new Button();
            btnCommand.Text = "Givet";
            btnCommand.WidthRequest = 70;
            //btnCommand.HeightRequest = 15;
            btnCommand.HorizontalOptions = LayoutOptions.End;

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

            labelBtnLayout = new StackLayout
            {
                Spacing = 2,
                Margin = new Thickness(3),
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = BackgroundColor
            };
            labelBtnLayout.Children.Add(labelLayout);
            labelBtnLayout.Children.Add(btnCommand);

            // Set bindings
            //lblName.SetBinding(Label.TextProperty, nameof(Name));
            //lblTime.SetBinding(Label.TextProperty, nameof(Time), BindingMode.OneWay, null, "{0:mm\\:ss}");
            //btnCommand.SetBinding(Button.CommandProperty, nameof(DrugInjectedCommand));

            View = labelBtnLayout;
        }

        #endregion

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                lblName.Text = Name;
                lblName.TextColor = TextColor;
                lblTime.Text = Time;
                lblTime.TextColor = TextColor;
                btnCommand.Command = DrugInjectedCommand;
                labelLayout.BackgroundColor = BackgroundColor;
                labelBtnLayout.BackgroundColor = BackgroundColor;
            }
        }
    }
}
