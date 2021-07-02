using music_player.ViewModels;
using Xamarin.Forms;
using MediaManager;
using MediaManager.Player;

namespace music_player.Views
{
   [ContentProperty("Child")]
   public partial class PlayerControlBar : ContentView
   {
      PlayerControlModel _viewModel;

      public PlayerControlBar()
      {
         InitializeComponent();

         BindingContext = _viewModel = new PlayerControlModel();
         CrossMediaManager.Current.StateChanged += MediaPlayerStateChanged;
         CrossMediaManager.Current.MediaItemChanged += MediaPlayerTrackChanged;
      }

      private void MediaPlayerStateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
      {
         if (e.State == MediaPlayerState.Buffering) BufferTimeout();
         if (e.State == MediaPlayerState.Playing)
         {
            _viewModel.Control_PlayPauseImage = "pause.png";
            _viewModel.Control_IsPlaying = true;
         }
         else if (e.State == MediaPlayerState.Paused || e.State == MediaPlayerState.Stopped)
         {
            _viewModel.Control_PlayPauseImage = "play.png";
            _viewModel.Control_IsPlaying = false;
         }
      }

      private async void BufferTimeout()
      {
         await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(2));
         if (CrossMediaManager.Current.State == MediaPlayerState.Buffering)
            _ = await CrossMediaManager.Current.PlayNext();
      }

      private void MediaPlayerTrackChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
      {
         _viewModel.Control_TrackName = CrossMediaManager.Current.Queue.Current.FileName;
      }
   }
}
