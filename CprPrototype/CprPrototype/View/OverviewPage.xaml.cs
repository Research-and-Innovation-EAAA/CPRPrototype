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

            // Under Contruction
            DataTemplate template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "Name");
            template.SetBinding(TextCell.DetailProperty, "Date");
            template.SetValue(TextCell.TextColorProperty, Color.FromHex("A6CE38"));
            //---
            List<Image> _imagelist = new List<Image>();
            Image s = new Image();
            s.Source = "icon_shockable.png";
            _imagelist.Add(s);
            listView.ItemTemplate = template;
            listView.BindingContext = viewModel;
            listView.ItemsSource = viewModel.History.Entries;
        }
    }
}