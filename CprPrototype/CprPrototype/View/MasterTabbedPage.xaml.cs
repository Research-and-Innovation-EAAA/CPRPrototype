using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTabbedPage : TabbedPage
    {
        public MasterTabbedPage()
        {
            InitializeComponent();
        }
    }
}