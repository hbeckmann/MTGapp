﻿using Android.App;
using Android.Widget;
using Android.OS;

namespace xamTest
{
    [Activity(Label = "xamTest")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.lifeCounterMenuButton);
            button.Click += delegate
            {
                StartActivity(typeof(LifeCounter));
            };



        }
    }
}

