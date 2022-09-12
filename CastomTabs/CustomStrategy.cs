using Android.Graphics.Drawables;
using AndroidX.AppCompat.Content.Res;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using AndroidX.Core.Graphics.Drawable;
using Google.Android.Material.Badge;
using Google.Android.Material.Tabs;
using static Google.Android.Material.Tabs.TabLayoutMediator;

namespace MyFinToControl
{
    public partial class MainActivity
    {

        public class CustomStrategy : Java.Lang.Object, ITabConfigurationStrategy
        {
            public void OnConfigureTab(TabLayout.Tab p0, int p1)
            {
                
               //// Drawable mIcon = ContextCompat.GetDrawable(p0.View.Context, MainActivity.tabIcons[p1]);
               // Drawable mIcon = AppCompatResources.GetDrawable(p0.View.Context, Resource.Mipmap.ic_cash50);
               // mIcon = DrawableCompat.Wrap(mIcon);
              
               // DrawableCompat.SetTint(mIcon, Android.Graphics.Color.Red);




                //Drawable mIcon = ContextCompat.GetDrawable(p0.View.Context, MainActivity.tabIcons[p1]);
                //mIcon.SetColorFilter(Android.Graphics.Color.Red, Android.Graphics.PorterDuff.Mode.Multiply);


                //p0.SetIcon(mIcon);


                //p0.SetText(MainActivity.fragmentTitles[p1]);
                p0.SetIcon(MainActivity.tabIcons[p1]);
                //var layoutParams = p0.View.LayoutParameters;
                //p0.SetTabLabelVisibility(TabLayout.TabLabelVisibilityLabeled);
                //layoutParams.Width = LinearLayoutCompat.LayoutParams.WrapContent;
                //p0.View.LayoutParameters = layoutParams;
                p0.SetText(p0.Text);
                p0.SetTabLabelVisibility(TabLayout.TabLabelVisibilityUnlabeled);

                p0.View.Visibility = Android.Views.ViewStates.Gone;
               
            }
           
        }

    }

}

