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
            BarBackgroundColor = Color.FromHex("#004578");

            NavigationPage.SetHasNavigationBar(this, false);
        } 
    }
}