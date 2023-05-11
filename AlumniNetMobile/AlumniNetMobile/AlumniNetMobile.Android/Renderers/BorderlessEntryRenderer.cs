using Android.Content;
using Android.Graphics.Drawables;
using TSEMobileApp.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
public class BorderlessEntryRenderer : EntryRenderer
{
    public BorderlessEntryRenderer(Context context) : base(context)
    {

    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);
        if (e.OldElement == null)
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetCornerRadius(60f);
            gradientDrawable.SetStroke(5, Android.Graphics.Color.Black);
            gradientDrawable.SetStroke(5, Android.Graphics.Color.Transparent);
            Control.SetBackground(gradientDrawable);

            Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);

            //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
            //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);

        }
    }
}