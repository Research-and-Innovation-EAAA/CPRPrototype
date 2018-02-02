using CprPrototype.Model;
using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Naxam.Controls.Forms;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTabbedPage : BottomTabbedPage
    {
        public MasterTabbedPage()
        {
            InitializeComponent();
        } 
    }
}