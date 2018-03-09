using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CprPrototype.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CprPrototype.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
        public static BaseViewModel _bvm = BaseViewModel.Instance;
		public TestPage ()
		{
            InitializeComponent();

           
            DataTemplate template = new DataTemplate(typeof(DrugCell));
            template.SetBinding(DrugCell.LabelTextProperty, "Tlabel");
            lstDrugs.ItemTemplate = template;
            lstDrugs.ItemsSource = _bvm.NotificationQueue;
		}
	}
}