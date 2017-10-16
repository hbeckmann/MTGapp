using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System.Threading;
using System;
using System.Diagnostics;
using Android.Content;

namespace xamTest
{
    [Activity(Label = "LifeCounter")]
    public class LifeCounterFragment : Android.Support.V4.App.Fragment
    {

        Chronometer gameTimer;
        ISharedPreferences sharedPref;
        ISharedPreferencesEditor editor;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.LifeCounter, container, false);
            return view;

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            Button decreaseButton = View.FindViewById<Button>(Resource.Id.topDecreaseButton);
            Button increaseButton = View.FindViewById<Button>(Resource.Id.topIncreaseButton);
            Button resetTimerButton = View.FindViewById<Button>(Resource.Id.refreshTimerButton);
            TextView topLifeCounter = View.FindViewById<TextView>(Resource.Id.topLifeCounter);

            Context context = this.Activity;
            sharedPref = context.GetSharedPreferences(
                GetString(Resource.String.preference_file), FileCreationMode.Private
                );
            editor = sharedPref.Edit();

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

        public override void OnStart()
        {
            base.OnStart();

            gameTimer = View.FindViewById<Chronometer>(Resource.Id.gameTimer);
            gameTimer.Base = SystemClock.ElapsedRealtime();
            gameTimer.Start();

        }


        public override void OnPause()
        {
            base.OnPause();
            gameTimer.Stop();
            editor.PutLong("pausedTime", SystemClock.ElapsedRealtime() - gameTimer.Base);
            editor.Commit();
            Console.WriteLine("Pausing. Attemption to log " + (SystemClock.ElapsedRealtime() - gameTimer.Base) + " time to the preferences");
        }

        public override void OnResume()
        {
            base.OnResume();
            long pausedGameTime = sharedPref.GetLong("pausedTime", 0);
            Console.WriteLine("Unpausing with pref of " + pausedGameTime);
            gameTimer.Base = SystemClock.ElapsedRealtime() - pausedGameTime;
            gameTimer.Start();
        }


    }



}

