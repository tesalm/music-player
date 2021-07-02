using music_player.Services;
using Xamarin.Forms;
using MediaManager;

namespace music_player
{
   public partial class App : Application
   {
      public App()
      {
         InitializeComponent();

         CrossMediaManager.Current.Init();

         DependencyService.Register<FolderDataStore>();
         DependencyService.Register<TrackDataStore>();
         DependencyService.Register<PlaylistDataStore>();
         MainPage = new AppShell();
      }

      protected override void OnStart()
      {
      }

      protected override void OnSleep()
      {
      }

      protected override void OnResume()
      {
      }
   }
}
