using Xamarin.Forms;
using Xamarin.Essentials;

namespace music_player.Views
{
   public partial class AboutPage : ContentPage
   {
      public AboutPage()
      {
         InitializeComponent();
         VersionTracking.Track();
         VersionLabel.Text = VersionTracking.CurrentVersion;
      }
   }
}