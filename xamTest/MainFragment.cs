using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace xamTest
{
    [Activity(Label = "xamTest")]
    public class MainFragment : Android.Support.V4.App.Fragment 
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.Main, container, false);
            
            return view;

        }

        public override void OnStart()
        {
            base.OnStart();
            ViewPager viewPager = this.Activity.FindViewById<ViewPager>(Resource.Id.menuViewPager);
            Button button = View.FindViewById<Button>(Resource.Id.lifeCounterMenuButton);
            button.Click += delegate
            {
                viewPager.SetCurrentItem(1, true);
            };
        }


    }
}

