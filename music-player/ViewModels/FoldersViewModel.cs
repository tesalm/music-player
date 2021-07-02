using music_player.Models;
using music_player.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace music_player.ViewModels
{
   public class FoldersViewModel : BaseViewModel
   {
      public IList<Folder> Folders { get; }
      public Command<Folder> FolderTapped { get; }

      public FoldersViewModel()
      {
         Title = "Folders";
         Folders = new List<Folder>();
         FolderTapped = new Command<Folder>(OnFolderSelected);
      }

      private async Task LoadFolders()
      {
         try
         {
            Folders.Clear();
            var folders = await FolderDataStore.GetItemsAsync();
            foreach (Folder folder in folders)
            {
               Folders.Add(folder);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex);
         }
      }

      public async void OnAppearing()
      {
         if (Folders.Count == 0)
            await LoadFolders();
      }

      private async void OnFolderSelected(Folder dir)
      {
         if (dir == null) return;

         await Shell.Current.GoToAsync($"{nameof(TracksPage)}?{nameof(TracksViewModel.FolderId)}={dir.Id}");
      }
   }
}