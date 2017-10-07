using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

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

    }
}

