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
    }
}

