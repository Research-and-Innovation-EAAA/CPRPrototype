using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CprPrototype.ViewModel;
using CprPrototype.View;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
        public static BaseViewModel _bvm = BaseViewModel.Instance;
        ExDrugsModel vm;
		public TestPage ()
		{
            InitializeComponent();
             vm = new ExDrugsModel();
            lstDrugs.ItemsSource = vm.Exdrugs;
		}

        void Button_Clicked(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            var pt = temp.Parent;

            var label = pt.FindByName<Label>("lblname");


            //Todo
            // switch case on drug labels:
            switch (label.Text)
            {
                case "Adrenalin": // add on history
                    break;
            }


            //var answer = await  DisplayAlert("Test formål","" +label.Text,"Yes","NO");
        }
    }
}