using System;
using Xamarin.Forms;
using music_player.Views;
using System.Collections.ObjectModel;
using music_player.Models;
using System.Threading.Tasks;

namespace music_player.ViewModels
{
   public class MainViewModel : BaseViewModel
   {
      public ObservableCollection<TrackPlaylist> Playlists { get; }
      public Command FolderTapped { get; }
      public Command TracksTapped { get; }
      public Command<TrackPlaylist> RemovePlaylistCommand { get; }
      public Command<TrackPlaylist> PlaylistTapped { get; }
      private bool showControl = false;
      private int trackCount, folderCount = 0;

      public MainViewModel()
      {
         Title = "Music Player";
         Playlists = new ObservableCollection<TrackPlaylist>();
         FolderTapped = new Command(OnFolderTapped);
         TracksTapped = new Command(OnTracksTapped);
         PlaylistTapped = new Command<TrackPlaylist>(OnPlaylistSelected);
         RemovePlaylistCommand = new Command<TrackPlaylist>(OnRemovePlaylist);
      }

      public int FolderCount
      {
         get => folderCount;
         set => SetProperty(ref folderCount, value);
      }

      public int TrackCount
      {
         get => trackCount;
         set => SetProperty(ref trackCount, value);
      }

      public bool ShowControl
      {
         get => showControl;
         set => SetProperty(ref showControl, value);
      }

      private async void OnFolderTapped()
      {
         // This will push the FoldersPage onto the navigation stack
         await Shell.Current.GoToAsync(nameof(FoldersPage), true);
      }

      private async void OnTracksTapped()
      {
         await Shell.Current.GoToAsync(nameof(TracksPage), true);
      }

      private async void OnPlaylistSelected(TrackPlaylist playlist)
      {
         if (playlist == null) return;

         await Shell.Current.GoToAsync($"{nameof(PlaylistContentPage)}?{nameof(PlaylistContentViewModel.PlaylistId)}={playlist.Id}");
      }

      private async void OnRemovePlaylist(TrackPlaylist playlist)
      {
         bool answer = await Application.Current.MainPage.DisplayAlert(
               title: null,
               message: "Delete " + playlist.Name + "?",
               cancel: "No",
               accept: "Yes"
            );

         if (answer)
         {
            await PlaylistDataStore.DeleteItemAsync(playlist.Id);
            Playlists.Remove(playlist);
            if (Playlists.Count == 0) ListIsEmpty = true;
         }
      }

      private async Task LoadPlaylists()
      {
         try
         {
            Playlists.Clear();
            var playlists = await PlaylistDataStore.GetItemsAsync();
            foreach (TrackPlaylist list in playlists)
            {
               Playlists.Add(list);
            }
            if (Playlists.Count > 0) ListIsEmpty = false;
            else ListIsEmpty = true;
         }
         catch (Exception)
         {
         }
      }

      public async void OnAppearing()
      {
         ShowControl = ShowPlayerControl;
         try {
            await LoadPlaylists();
            if (TrackCount == 0)
            {
               var tracks = await TrackDataStore.GetItemsAsync();
               var folders = await FolderDataStore.GetItemsAsync();
               TrackCount = tracks.Count;
               FolderCount = folders.Count;
            }
         }
         catch (Exception)
         {
         }
      }
   }
}