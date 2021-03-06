﻿using CprPrototype.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CprPrototype;
using CprPrototype.Service;
using System;
using System.Threading.Tasks;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        private BaseViewModel _viewModel = BaseViewModel.Instance;
        private Database.DatabaseHelper _database = Database.DatabaseHelper.Instance;

        public OverviewPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = _viewModel;
            DataTemplate template = new DataTemplate(typeof(ImageCell));
            template.SetBinding(ImageCell.ImageSourceProperty, "ImageSource");
            template.SetBinding(ImageCell.TextProperty, "Name");
            template.SetBinding(ImageCell.DetailProperty, "DateTimeString");
            template.SetValue(ImageCell.TextColorProperty, Color.Black);
            listView.ItemTemplate = template;
            listView.BindingContext = _viewModel;
            listView.ItemsSource = _viewModel.History.Entries;


            btnlog.SetBinding(IsVisibleProperty, nameof(_viewModel.IsLogAvailable));
            btnRUC.SetBinding(IsVisibleProperty, nameof(_viewModel.IsDoneAvailable));
            btnDoed.SetBinding(IsVisibleProperty, nameof(_viewModel.IsDoneAvailable));
        }
        public async void BtnRUC_Clicked(object sender, EventArgs e)
        {
            await ConnectToDB();
            _viewModel.EndAlgorithm();
        }
        public async void BtnDoed_Clicked(object sender, EventArgs e)
        {
            await ConnectToDB();
            _viewModel.EndAlgorithm();
        }
        public async void GoToLogPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogPage());
        }
        private async Task ConnectToDB()
        {
            await _database.CreateTablesAsync();
            _viewModel.History.CPRHistoryTotalCycles = "Antal cyklusser: " + _viewModel.TotalElapsedCycles;
            _viewModel.History.AttemptFinished = DateTime.Now;
            _viewModel.History.HistoryName = _viewModel.History.AttemptStarted.Date.ToString("dd/MM/yy ") + " " + _viewModel.History.AttemptStarted.ToString(" HH:mm") + " - " + _viewModel.History.AttemptFinished.ToString("HH:mm");
            await _database.InsertCPRHistoryAsync(_viewModel.History);

            // Insert Entries into DB
            List<CPRHistoryEntry> updatedList = new List<CPRHistoryEntry>(_viewModel.History.Entries);
            foreach (var item in updatedList)
            {
                item.CPRHistoryId = _viewModel.History.Id;
            }

            await _database.InsertListOfEntries(updatedList);
        }
    }
}