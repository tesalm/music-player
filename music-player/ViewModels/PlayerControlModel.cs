using Xamarin.Forms;
using MediaManager;
using MediaManager.Player;

namespace music_player.ViewModels
{
   public class PlayerControlModel : BaseViewModel
   {
      public Command PlayPauseCommand { get; }
      public Command PlayNextCommand { get; }

      private bool isPlaying;
      private string playPauseImage;
      private string trackName;

      public PlayerControlModel()
      {
         MediaPlayerState state = CrossMediaManager.Current.State;
         if (state == MediaPlayerState.Playing)
         {
            Control_PlayPauseImage = "pause.png";
            Control_IsPlaying = true;
            Control_TrackName = CrossMediaManager.Current.Queue.Current.FileName;
         }
         else if (state == MediaPlayerState.Paused)
         {
            Control_PlayPauseImage = "play.png";
            Control_IsPlaying = false;
            Control_TrackName = CrossMediaManager.Current.Queue.Current.FileName;
         }
         PlayPauseCommand = new Command(PlayPause);
         PlayNextCommand = new Command(PlayNext);
      }

      public bool Control_IsPlaying
      {
         get => isPlaying;
         set => SetProperty(ref isPlaying, value);
      }

      public string Control_PlayPauseImage
      {
         get => playPauseImage;
         set => SetProperty(ref playPauseImage, value);
      }

      public string Control_TrackName
      {
         get => trackName;
         set => SetProperty(ref trackName, value);
      }
   }
}