using Android.App;
using Android.Widget;
using Android.OS;
using xamTest;

[Activity(Label = "LifeCounter")]
public class LifeCounter : Activity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.LifeCounter);


    }
}
