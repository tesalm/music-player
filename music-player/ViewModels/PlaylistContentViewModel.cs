using music_player.Models;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace music_player.ViewModels
{
   [QueryProperty(nameof(PlaylistId), nameof(PlaylistId))]
   public class PlaylistContentViewModel : TracksViewModel
   {
      private string playlistId;
      private TrackPlaylist SelectedPlaylist { get; set; }
      public Command<Track> RemoveTrackCommand { get; }

      public PlaylistContentViewModel()
      {
         Tracks = new ObservableCollection<Track>();
         RemoveTrackCommand = new Command<Track>(OnRemoveTrack);
      }

      public string PlaylistId
      {
         get => playlistId;
         set { playlistId = value; LoadPlaylist(value); }
      }

      private async void LoadPlaylist(string playlistId)
      {
         try
         {
            Tracks.Clear();
            TrackPlaylist playlist = await PlaylistDataStore.GetItemAsync(playlistId);
            SelectedPlaylist = playlist;
            Title = playlist.Name;
            var tracks = await TrackDataStore.GetItemsAsync();

            foreach (string path in playlist.Tracks)
            {
               foreach (Track track in tracks)
               {
                  if (path.Contains(track.Name))
                  {
                     Tracks.Add(track);
                     break;
                  }
               }
            }
            if (Tracks.Count == 0) ListIsEmpty = true;
         }
         catch (Exception)
         {
            Debug.WriteLine("Failed to Load Playlist");
         }
      }

      private async void OnRemoveTrack(Track track)
      {
         if (track == null) return;

         bool answer = await Application.Current.MainPage.DisplayAlert(
               title: null,
               message: "Remove " + track.Name + " from " + SelectedPlaylist.Name + "?",
               cancel: "No",
               accept: "Yes"
            );

         if (answer)
         {
            SelectedPlaylist.Tracks.Remove(track.Path);
            SelectedPlaylist.TrackCount = SelectedPlaylist.Tracks.Count;
            await PlaylistDataStore.UpdateItemAsync(SelectedPlaylist);
            Tracks.Remove(track);
            PlayerPrepared = false;
            if (Tracks.Count == 0) ListIsEmpty = true;
         }
      }
   }
}
