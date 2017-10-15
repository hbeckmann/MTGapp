using Android.App;
using Android.Widget;
using Android.OS;
using xamTest;
using Android.Views;

[Activity(Label = "LifeCounter")]
public class LifeCounter : Activity, GestureDetector.IOnGestureListener
{
    private GestureDetector _gestureDetector;
    public TextView _textview;

    public bool OnDown(MotionEvent e)
    {
        return false;
    }
    public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
    {
        if (velocityX > 5000)
        {
            Finish();
            return true;
        }
        else
        {
            return false;
        }


    }
    public void OnLongPress(MotionEvent e) { }
    public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
    {
        return false;
    }
    public void OnShowPress(MotionEvent e) { }
    public bool OnSingleTapUp(MotionEvent e)
    {
        return false;
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        _gestureDetector.OnTouchEvent(e);
        return false;
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.LifeCounter);


        _gestureDetector = new GestureDetector(this);
        _textview = FindViewById<TextView>(Resource.Id.textView1);


    }

    public void updateText(string newText)
    {
        _textview.Text = newText;
    }
}
