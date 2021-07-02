using System.Collections.Generic;

namespace music_player.Models
{
   public class TrackPlaylist
   {
      public string Id { get; set; }
      public string Name { get; set; }
      public int TrackCount { get; set; }
      public IList<string> Tracks { get; set; }
   }
}
