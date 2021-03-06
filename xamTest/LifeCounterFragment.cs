﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System.Threading;
using System;
using System.Diagnostics;
using Android.Content;
using SQLite;
using System.Threading.Tasks;

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
            Button topDecreaseButton = View.FindViewById<Button>(Resource.Id.topDecreaseButton);
            Button topIncreaseButton = View.FindViewById<Button>(Resource.Id.topIncreaseButton);
            Button bottomDecreaseButton = View.FindViewById<Button>(Resource.Id.bottomDecreaseButton);
            Button bottomIncreaseButton = View.FindViewById<Button>(Resource.Id.bottomIncreaseButton);
            Button resetTimerButton = View.FindViewById<Button>(Resource.Id.refreshTimerButton);
            ImageButton saveGameButton = View.FindViewById<ImageButton>(Resource.Id.saveGameButton);
            TextView topLifeCounter = View.FindViewById<TextView>(Resource.Id.topLifeCounter);
            TextView bottomLifeCounter = View.FindViewById<TextView>(Resource.Id.bottomLifeCounter);

            Context context = this.Activity;
            sharedPref = context.GetSharedPreferences(
                GetString(Resource.String.preference_file), FileCreationMode.Private
                );
            editor = sharedPref.Edit();

            topDecreaseButton.Click += (senderAlert, args) => { DecreaseLifeTotal(topLifeCounter); };
            topIncreaseButton.Click += (senderAlert, args) => { IncreaseLifeTotal(topLifeCounter); };
            bottomDecreaseButton.Click += (senderAlert, args) => { DecreaseLifeTotal(bottomLifeCounter); };
            bottomIncreaseButton.Click += (senderAlert, args) => { IncreaseLifeTotal(bottomLifeCounter); };
            saveGameButton.Click += 
                (senderAlert, args) => { gameDialog(this.Context, "Save game results?", "Save", SaveGameStats, "No Thanks", null  ); };

            resetTimerButton.Click += delegate
            {
                gameTimer.Stop();
                gameTimer.Base = SystemClock.ElapsedRealtime();
                gameTimer.Start();
            };

        }

        //Logic that executes when the fragment is visible
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
                        string question = "Do you want to resume your last game?";
                        string positivePrompt = "Resume";
                        string negativePrompt = "No thanks";
                        gameDialog(this.Context, question, positivePrompt, ResumeTime, negativePrompt, RestartTime);
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

        public void gameDialog(Context context, string questionPrompt, string positivePrompt, AlertFunction positiveFunc, string negativePrompt, AlertFunction negativeFunc )
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle(questionPrompt);
            builder.SetPositiveButton(positivePrompt, (senderAlert, args) => { positiveFunc?.Invoke(); });
            builder.SetNegativeButton(negativePrompt, (senderAlert, args) => { negativeFunc?.Invoke(); });
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
            
        public void IncreaseLifeTotal(TextView counter)
        {
            counter.Text = (int.Parse(counter.Text) + 1).ToString();
        }

        public void DecreaseLifeTotal(TextView counter)
        {
            if (int.Parse(counter.Text) > 0)
            {
                counter.Text = (int.Parse(counter.Text) - 1).ToString();
            }
        }

        public void SaveGameStats()
        {
            Game gameStats = new Game
            {
                Date = "10/29/2017",
                Result = "Win"
            };
            GameDatabase.InsertGameData(gameStats);
            //GameDatabase.NumberOfGames().ContinueWith(t => { Console.WriteLine("PLEEEEEEEEEEEEEEEASE LORD " + t.Result.ToString()); });
            Console.WriteLine("AYYYYYYYYYYYYYYYYYYYYYYYYYYYYY  " + GameDatabase.NumberOfGames());
            

        }


    }

    public class GameDatabase
    {

        static SQLite.SQLiteConnection database;

        public static SQLiteConnection Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLite.SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),"MTGSQL.db3"));
                    database.CreateTable<Game>();
                }

                return database;
            }
        }

        public static void InsertGameData(Game game)
        {
            Database.Insert(game);
        }

        public static string NumberOfGames()
        {
            //var count = Database.ExecuteScalarAsync<int>("SELECT Count(*) FROM Game");
            var db = Database.Table<Game>();
            //Console.WriteLine("LUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUL");
            //return count;
            return Database.Get<Game>(1).Result;

        }

        public GameDatabase()
        {

        }
    }

    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Date { get; set; }

        public string Result { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: ID={0}, Date={1}, Result={2}]", ID, Date, Result);
        }

    }


}

