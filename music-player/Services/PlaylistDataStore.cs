using music_player.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace music_player.Services
{
   public class PlaylistDataStore : IDataStore<TrackPlaylist>
   {
      private readonly IList<TrackPlaylist> playlists;
      private readonly string backingFile;

      public PlaylistDataStore()
      {
         playlists = new List<TrackPlaylist>();
         backingFile = Path.Combine(FileSystem.AppDataDirectory, "playlists.txt");
         //string pl = Preferences.Get("playlists", "[]");

         if (backingFile != null && File.Exists(backingFile))
         {
            string fileContent = File.ReadAllText(backingFile);
            if (!string.IsNullOrEmpty(fileContent))
            {
               JArray jarray = JArray.Parse(fileContent);
               foreach (JToken obj in jarray)
               {
                  List<string> tracks = obj["Tracks"].ToObject<List<string>>();

                  var temp = new List<string>(tracks);
                  temp.ForEach(path => {
                     if (!File.Exists(path)) {
                        tracks.Remove(path);
                     }
                  });

                  playlists.Add(new TrackPlaylist
                  {
                     Id = obj["Id"].ToString(),
                     Name = obj["Name"].ToString(),
                     TrackCount = tracks.Count,
                     Tracks = tracks
                  });
               }
            }
         }
      }

      public async Task<TrackPlaylist> GetItemAsync(string id)
      {
         return await Task.FromResult(playlists.FirstOrDefault(s => s.Id == id));
      }

      public async Task<IList<TrackPlaylist>> GetItemsAsync(bool forceRefresh = false)
      {
         return await Task.FromResult(playlists);
      }

      public async Task<bool> AddItemAsync(TrackPlaylist plist)
      {
         playlists.Add(plist);

         return await Task.FromResult(true);
      }

      public async Task<bool> UpdateItemAsync(TrackPlaylist plist)
      {
         var oldItem = playlists.Where((TrackPlaylist arg) => arg.Id == plist.Id).FirstOrDefault();
         int indx = playlists.IndexOf(oldItem);
         if (indx != -1) playlists[indx] = plist;
         //Preferences.Set("playlists", ToJsonArray());
         File.WriteAllText(backingFile, ToJsonArray());

         return await Task.FromResult(true);
      }

      public async Task<bool> DeleteItemAsync(string id)
      {
         var oldItem = playlists.Where((TrackPlaylist arg) => arg.Id == id).FirstOrDefault();
         playlists.Remove(oldItem);
         //Preferences.Set("playlists", ToJsonArray());
         File.WriteAllText(backingFile, ToJsonArray());

         return await Task.FromResult(true);
      }

      private string ToJson(TrackPlaylist list)
      {
         StringWriter sw = new StringWriter();
         JsonTextWriter writer = new JsonTextWriter(sw);

         // {
         writer.WriteStartObject();

         // "Id" : "od43eflk-fi87y89f7"
         writer.WritePropertyName("Id");
         writer.WriteValue(list.Id);

         writer.WritePropertyName("Name");
         writer.WriteValue(list.Name);

         writer.WritePropertyName("Tracks");
         writer.WriteStartArray();
         foreach (string track in list.Tracks)
         {
            writer.WriteValue(track);
         }
         writer.WriteEndArray();

         // }
         writer.WriteEndObject();

         return sw.ToString();
      }

      private string ToJsonArray()
      {
         string jsonStr = "[";
         foreach (TrackPlaylist list in playlists)
         {
            jsonStr += ToJson(list) + ",";
         }
         jsonStr += "]";

         return jsonStr;
      }
   }
}