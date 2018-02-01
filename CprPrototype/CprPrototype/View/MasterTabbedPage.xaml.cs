using CprPrototype.Model;
using CprPrototype.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTabbedPage : Naxam.Controls.Forms.BottomTabbedPage
    {
        public MasterTabbedPage()
        {
            InitializeComponent(); 
        }
        
    }
}