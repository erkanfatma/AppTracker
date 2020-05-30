
using AU.AppTracker.Mobile.CustomControls;
using AU.AppTracker.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace AU.AppTracker.Mobile.Droid.CustomRenderers {
      [System.Obsolete]
      public class CustomSearchBarRenderer : SearchBarRenderer {
            protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e) {
                  base.OnElementChanged(e);

                  if(Control != null) {
                        var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                        var plate = Control.FindViewById(plateId);
                        plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
                  }
            }

      }
}
