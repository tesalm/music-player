using Xamarin.Forms;
using MediaManager.Library;

namespace music_player.Models
{
   public class Track
   {
      public string Id { get; set; }
      public string Name { get; set; }
      public string Path { get; set; }
      public string Artist { get; set; }
      public ImageSource Art { get; set; }
      public IMediaItem MediaItem { get; set; }
   }
}
