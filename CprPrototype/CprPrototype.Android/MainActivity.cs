using Android.App;
using Android.Content.PM;
using Android.OS;
using Naxam.Controls.Platform.Droid;
using Android.Graphics;

namespace CprPrototype.Droid
{
    [Activity(Label = "CprPrototype", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SetupBottomTab();

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            AdvancedTimer.Forms.Plugin.Droid.AdvancedTimerImplementation.Init();
            LoadApplication(new App());
        }
        private void SetupBottomTab()
        {
            var statelist = new Android.Content.Res.ColorStateList(
                new int[][]
                {
                    new int[] {/*Android.Resource.Attribute.StateChecked}*/},
                    new int[] {Android.Resource.Attribute.StateEnabled},
                }, new int[] { Color.White, Color.White });

            // Bottom navigation setup
            BottomTabbedRenderer.BackgroundColor = Color.ParseColor("#00447B");
            BottomTabbedRenderer.FontSize = 15;
            BottomTabbedRenderer.ItemTextColor = statelist;
            BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(8);
            BottomTabbedRenderer.BottomBarHeight = 35;
            BottomTabbedRenderer.ItemSpacing = 10;
        }
    }
}

