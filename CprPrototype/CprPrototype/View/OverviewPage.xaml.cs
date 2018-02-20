using CprPrototype.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CprPrototype;
using System;

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
            template.SetBinding(ImageCell.ImageSourceProperty, "ImageSource");
            template.SetBinding(ImageCell.TextProperty, "Name");
            template.SetBinding(ImageCell.DetailProperty, "Date");
            template.SetValue(ImageCell.TextColorProperty, Color.Black);
            listView.ItemTemplate = template;
            listView.BindingContext = viewModel;
            listView.ItemsSource = viewModel.History.Entries;

            btnlog.SetBinding(IsVisibleProperty, nameof(viewModel.IsLogAvailable));
            btnRUC.SetBinding(IsVisibleProperty, nameof(viewModel.IsDoneAvailable));
            btnDoed.SetBinding(IsVisibleProperty, nameof(viewModel.IsDoneAvailable));
        }
        public void BtnRUC_Clicked(object sender, EventArgs e)
        {
            viewModel.EndAlgorithm();
        }
        public void BtnDoed_Clicked(object sender, EventArgs e)
        {
            viewModel.EndAlgorithm();
        }
        public void GoToLogPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new LogPage();
        }
    }
}