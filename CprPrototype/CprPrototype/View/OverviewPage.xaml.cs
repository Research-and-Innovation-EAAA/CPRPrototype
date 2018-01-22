using CprPrototype.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance();

        public OverviewPage()
        {
            InitializeComponent();
            BindingContext = viewModel;

            DataTemplate template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "Name");
            template.SetBinding(TextCell.DetailProperty, "Date");
            template.SetValue(TextCell.TextColorProperty, Color.FromHex("A6CE38"));

            listView.ItemTemplate = template;
            listView.BindingContext = viewModel;
            listView.ItemsSource = viewModel.History.Records;
        }
    }
}