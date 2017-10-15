using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System.Threading;
using System;
using System.Diagnostics;

namespace xamTest
{
    [Activity(Label = "LifeCounter")]
    public class LifeCounterFragment : Android.Support.V4.App.Fragment
    {

        Chronometer gameTimer;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.LifeCounter, container, false);
            return view;

        }

        public override void OnStart()
        {
            base.OnStart();
            Button decreaseButton = View.FindViewById<Button>(Resource.Id.topDecreaseButton);
            Button increaseButton = View.FindViewById<Button>(Resource.Id.topIncreaseButton);
            Button resetTimerButton = View.FindViewById<Button>(Resource.Id.refreshTimerButton);
            TextView topLifeCounter = View.FindViewById<TextView>(Resource.Id.topLifeCounter);
            gameTimer = View.FindViewById<Chronometer>(Resource.Id.gameTimer);
            gameTimer.Base = SystemClock.ElapsedRealtime();
            gameTimer.Start();

            decreaseButton.Click += delegate
            {
                if (int.Parse(topLifeCounter.Text) > 0)
                {
                    topLifeCounter.Text = (int.Parse(topLifeCounter.Text) - 1).ToString();
                }
            };

            increaseButton.Click += delegate
            {
                topLifeCounter.Text = (int.Parse(topLifeCounter.Text) + 1).ToString();
            };

            resetTimerButton.Click += delegate
            {
                gameTimer.Stop();
                gameTimer.Base = SystemClock.ElapsedRealtime();
                gameTimer.Start();
            };

        }

        //public override void OnPause()
        //{
        //    base.OnPause();
        //    gameTimer.Stop();
        //}

        //public override void OnResume()
        //{
        //    base.OnResume();
        //    gameTimer.Start();
        //}


    }



}

