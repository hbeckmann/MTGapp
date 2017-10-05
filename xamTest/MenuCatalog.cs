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

namespace xamTest
{
    public class MenuCatalog
    {

        static int[] page =
        {
            Resource.Layout.Main, Resource.Layout.LifeCounter
        };

        public int[] pages = page;

        public int NumPages { get { return pages.Length; } }

    }
}