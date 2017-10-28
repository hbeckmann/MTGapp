using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Java.Lang;

namespace xamTest
{

    public class MenuPagerAdapter : FragmentPagerAdapter
    {
        Android.Support.V4.App.Fragment[] page =
            {
            new MainFragment(),
            new LifeCounterFragment()
            };
        int pageCount;

        public MenuPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {

            pageCount = page.Count();

        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return page[position];
        }


        public override int Count
        {
            get { return pageCount ; }
        }

    }
}