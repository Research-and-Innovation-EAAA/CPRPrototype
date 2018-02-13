using CprPrototype.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        private BaseViewModel viewModel = BaseViewModel.Instance;

        public OverviewPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
            DataTemplate template = new DataTemplate(typeof(ImageCell));
            template.SetBinding(ImageCell.ImageSourceProperty,"ImageSource");
            template.SetBinding(ImageCell.TextProperty, "Name");
            template.SetBinding(ImageCell.DetailProperty, "Date");
            template.SetValue(ImageCell.TextColorProperty, Color.Black);
            listView.ItemTemplate = template;
            listView.BindingContext = viewModel;
            listView.ItemsSource = viewModel.History.Records;
        }
    }
}