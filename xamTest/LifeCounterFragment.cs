using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System.Threading;
using System;

namespace xamTest
{
    [Activity(Label = "LifeCounter")]
    public class LifeCounterFragment : Android.Support.V4.App.Fragment
    {
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
            TextView topLifeCounter = View.FindViewById<TextView>(Resource.Id.topLifeCounter);
            TextView gameTimer = View.FindViewById<TextView>(Resource.Id.gameTimer);
            StartTimer(1000, gameTimer);

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

        }

        public void StartTimer(int time, TextView timerGUI)
        {
            TimeInfo timeInfo = new TimeInfo(timerGUI);
            Timer t = new Timer(new TimerCallback(TimerTick), timeInfo, time, time);
        }

        public void TimerTick (object state)
        {
            TimeInfo t = (TimeInfo)state;
            Activity.RunOnUiThread(() => t.timerGUI.Text = (DateTime.Now.Second - t.startTime.Second).ToString());
            Console.WriteLine("TICK!");
           
        }

    }


    public class TimeInfo 
    {
        public TextView timerGUI;

        public TimeInfo(TextView timerGUI)
        {
            this.timerGUI = timerGUI;
        }

        public DateTime startTime = DateTime.Now;
    }


}

