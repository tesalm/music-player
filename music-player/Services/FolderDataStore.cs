using music_player.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace music_player.Services
{
   public class FolderDataStore : IDataStore<Folder>
   {
      private readonly List<Folder> folders;

      public FolderDataStore()
      {
         List<string> dirs = (List<string>)DependencyService.Get<IExternalStorage>().GetAudioDirectorys();
         folders = new List<Folder>();

         foreach (string path in dirs)
         {
            folders.Add(new Folder
            {
               Id = Guid.NewGuid().ToString(),
               Name = Path.GetFileName(path),
               Path = path,
               FileCount = Directory.GetFiles(path).Length.ToString()
            });
         }
      }

      public async Task<bool> AddItemAsync(Folder dir)
      {
         folders.Add(dir);

         return await Task.FromResult(true);
      }

      public async Task<bool> UpdateItemAsync(Folder dir)
      {
         var oldItem = folders.Where((Folder arg) => arg.Id == dir.Id).FirstOrDefault();
         folders.Remove(oldItem);
         folders.Add(dir);

         return await Task.FromResult(true);
      }

      public async Task<bool> DeleteItemAsync(string id)
      {
         var oldItem = folders.Where((Folder arg) => arg.Id == id).FirstOrDefault();
         folders.Remove(oldItem);

         return await Task.FromResult(true);
      }

      public async Task<Folder> GetItemAsync(string id)
      {
         return await Task.FromResult(folders.FirstOrDefault(s => s.Id == id));
      }

      public async Task<IList<Folder>> GetItemsAsync(bool forceRefresh = false)
      {
         return await Task.FromResult(folders);
      }
   }
}