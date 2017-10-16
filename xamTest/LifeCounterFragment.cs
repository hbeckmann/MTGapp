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
        Boolean resumeGame, appRestarted;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.LifeCounter, container, false);
            appRestarted = true;
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

        public override bool UserVisibleHint
        {
            get => base.UserVisibleHint;
            set {
                base.UserVisibleHint = value;
                if (value) {
                    if(resumeGame != true && !appRestarted)
                    {
                        gameTimer.Start();
                        Console.WriteLine("Starting Clock from on visibuleHint");
                    }

                    if (appRestarted)
                    {
                        ResumeGameDialog(this.Context, ResumeTime, RestartTime);
                    }
                }
                appRestarted = false;

            }
        }

        public override void OnStart()
        {
            base.OnStart();
            gameTimer = View.FindViewById<Chronometer>(Resource.Id.gameTimer);
            gameTimer.Base = SystemClock.ElapsedRealtime();

            Console.WriteLine("LifeCounter fragment has hit OnStart");

        }


        public override void OnPause()
        {
            base.OnPause();
            gameTimer.Stop();
            editor.PutLong("pausedTime", SystemClock.ElapsedRealtime() - gameTimer.Base);
            editor.Commit();
            resumeGame = true;
            appRestarted = false;
            //Console.WriteLine("Pausing. Attemption to log " + (SystemClock.ElapsedRealtime() - gameTimer.Base) + " time to the preferences");
        }

        public override void OnResume()
        {
            base.OnResume();
            if (resumeGame == true)
            {
                long pausedGameTime = sharedPref.GetLong("pausedTime", 0);
                //Console.WriteLine("Unpausing with pref of " + pausedGameTime);
                gameTimer.Base = SystemClock.ElapsedRealtime() - pausedGameTime;
                gameTimer.Start();
            } else if (UserVisibleHint && !appRestarted)
            {
                gameTimer.Base = SystemClock.ElapsedRealtime();
                Console.WriteLine("Starting Clock from on Resume");
                gameTimer.Start();
            }

            Console.WriteLine("LifeCounter fragment has hit OnResume");
        }

        public void ResumeGameDialog(Context context, AlertFunction positiveFunc, AlertFunction negativeFunc )
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Do you want to resume your last game?");
            builder.SetPositiveButton("Resume", (senderAlert, args) => { positiveFunc(); });
            builder.SetNegativeButton("No thanks", (senderAlert, args) => { negativeFunc(); });
            Dialog dialog = builder.Create();
            dialog.Show();
        }

        public delegate void AlertFunction();

        public void ResumeTime()
        {
            resumeGame = true;
            long pausedGameTime = sharedPref.GetLong("pausedTime", 0);
            gameTimer.Base = SystemClock.ElapsedRealtime() - pausedGameTime;
            gameTimer.Start();
            Console.WriteLine("Starting from ResumeTime dialog func");
        }

        public void RestartTime()
        {
            resumeGame = false;
            gameTimer.Base = SystemClock.ElapsedRealtime();
            gameTimer.Start();
            Console.WriteLine("Starting from restart time");
        }




    }



}

