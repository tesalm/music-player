using music_player.Views;
using System;
using Xamarin.Forms;

namespace music_player
{
   public partial class AppShell : Xamarin.Forms.Shell
   {
      public AppShell()
      {
         InitializeComponent();
         Routing.RegisterRoute(nameof(TracksPage), typeof(TracksPage));
         Routing.RegisterRoute(nameof(FoldersPage), typeof(FoldersPage));
         Routing.RegisterRoute(nameof(PlaylistsPage), typeof(PlaylistsPage));
         Routing.RegisterRoute(nameof(PlaylistContentPage), typeof(PlaylistContentPage));
         Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
      }

      private async void OnMenuItemClicked(object sender, EventArgs e)
      {
         Shell.Current.FlyoutIsPresented = false;
         await Shell.Current.GoToAsync(nameof(AboutPage));
      }
   }
}
