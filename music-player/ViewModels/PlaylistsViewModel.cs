using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using music_player.Models;

namespace music_player.ViewModels
{
   [QueryProperty(nameof(TrackId), nameof(TrackId))]
   public class PlaylistsViewModel : BaseViewModel
   {
      private string trackId;
      public ObservableCollection<TrackPlaylist> Playlists { get; }
      public Command AddNewPlaylistCommand { get; }
      public Command<TrackPlaylist> PlaylistTapped { get; }

      public PlaylistsViewModel()
      {
         Playlists = new ObservableCollection<TrackPlaylist>();
         PlaylistTapped = new Command<TrackPlaylist>(OnPlaylistSelected);
         AddNewPlaylistCommand = new Command(OnAddNewPlaylist);
      }

      public string TrackId
      {
         get => trackId;
         set { trackId = value; LoadTrackId(value); }
      }

      private Track SelectedTrack { get; set; }

      private async void LoadTrackId(string trackId)
      {
         try
         {
            SelectedTrack = await TrackDataStore.GetItemAsync(trackId);
         }
         catch (Exception)
         {
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
            if (Playlists.Count == 0) ListIsEmpty = true;
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex);
         }
      }

      public async void OnAppearing()
      {
         await LoadPlaylists();
      }

      private async void OnAddNewPlaylist()
      {
         string listName = await Application.Current.MainPage.DisplayPromptAsync("Create a new playlist", null, "OK", "CANCEL", maxLength: 20);
         if (!String.IsNullOrWhiteSpace(listName))
         {
            TrackPlaylist newPlaylist = new TrackPlaylist()
            {
               Id = Guid.NewGuid().ToString(),
               Name = listName,
               TrackCount = 0,
               Tracks = new List<string>()
            };
            _ = await PlaylistDataStore.AddItemAsync(newPlaylist);
            Playlists.Add(newPlaylist);
            ListIsEmpty = false;
         }
      }

      private async void OnPlaylistSelected(TrackPlaylist playlist)
      {
         if (playlist == null) return;

         if (playlist.Tracks.Contains(SelectedTrack.Path))
            await Application.Current.MainPage.DisplayAlert(title: null, message: "This track already exists in selected playlist", cancel: "OK");
         else
         {
            playlist.Tracks.Add(SelectedTrack.Path);
            playlist.TrackCount = playlist.Tracks.Count;
            _ = await PlaylistDataStore.UpdateItemAsync(playlist);
         }
         // This will pop the current page off the navigation stack
         await Shell.Current.GoToAsync("..");
      }
   }
}