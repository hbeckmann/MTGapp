using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace xamTest
{
    [Activity(Label = "MenuActivity", MainLauncher = true)]
    public class MenuActivity : FragmentActivity
    { 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.menuViewPager);
            MenuCatalog menuCatalog = new MenuCatalog();
            viewPager.Adapter = new MenuPagerAdapter(SupportFragmentManager);
        }
    }
}