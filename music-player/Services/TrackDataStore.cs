using music_player.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MediaManager.Library;

namespace music_player.Services
{
   public class TrackDataStore : IDataStore<Track>
   {
      private readonly IList<Track> tracks;

      public TrackDataStore()
      {
         IExternalStorage intrf = DependencyService.Get<IExternalStorage>();
         IList<string> audiofiles = intrf.GetAudioFiles();
         tracks = new List<Track>();

         foreach (string path in audiofiles)
         {
            IMediaItem item = intrf.ExtractAudioMetaData(path);
            tracks.Add(new Track
            {
               Id = Guid.NewGuid().ToString(),
               Name = item.FileName,
               Path = path,
               Artist = item.Artist,
               Art = (ImageSource)item.Image,
               MediaItem = item
            });
         }
      }

      public async Task<Track> GetItemAsync(string id)
      {
         return await Task.FromResult(tracks.FirstOrDefault(s => s.Id == id));
      }

      public async Task<IList<Track>> GetItemsAsync(bool forceRefresh = false)
      {
         return await Task.FromResult(tracks);
      }

      public async Task<bool> AddItemAsync(Track dir)
      {
         tracks.Add(dir);

         return await Task.FromResult(true);
      }

      public async Task<bool> UpdateItemAsync(Track dir)
      {
         var oldItem = tracks.Where((Track arg) => arg.Id == dir.Id).FirstOrDefault();
         tracks.Remove(oldItem);
         tracks.Add(dir);

         return await Task.FromResult(true);
      }

      public async Task<bool> DeleteItemAsync(string id)
      {
         var oldItem = tracks.Where((Track arg) => arg.Id == id).FirstOrDefault();
         tracks.Remove(oldItem);

         return await Task.FromResult(true);
      }
   }
}