using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using music_player.ViewModels;

namespace music_player.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class PlaylistContentPage : ContentPage
   {
      public PlaylistContentPage()
      {
         InitializeComponent();
         BindingContext = new PlaylistContentViewModel();
      }
   }
}