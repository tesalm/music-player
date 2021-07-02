using music_player.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using music_player.Views;
using System.Collections.Generic;
using System.Linq;

namespace music_player.ViewModels
{
   [QueryProperty(nameof(FolderId), nameof(FolderId))]
   public class TracksViewModel : BaseViewModel
   {
      public Command ShuffleTapped { get; }
      public Command SearchTapped { get; }
      public Command<Track> TrackTapped { get; }
      public Command<Track> AddToPlaylistCommand { get; }
      public bool PlayerPrepared { get; set; } = false;
      public string FolderId { get; set; }
      private IList<Track> DataHolder { get; set; } = new List<Track>();
      private IList<Track> tracks = new List<Track>();
      private bool showSearch = false;
      private string queryText = string.Empty;

      public TracksViewModel()
      {
         Title = "Tracks";
         TrackTapped = new Command<Track>(OnTrackSelected);
         ShuffleTapped = new Command(OnShuffleTapped);
         SearchTapped = new Command(OnSearchIconTapped);
         AddToPlaylistCommand = new Command<Track>(OnAddToPlaylist);
      }

      public IList<Track> Tracks
      {
         get => tracks;
         set => SetProperty(ref tracks, value);
      }

      public bool ShowSearchBar
      {
         get => showSearch;
         set => SetProperty(ref showSearch, value);
      }

      private async Task LoadTracks()
      {
         try
         {
            var tracks = await TrackDataStore.GetItemsAsync();
            if (FolderId == null) Tracks = tracks;
            else
            {
               Folder folder = await FolderDataStore.GetItemAsync(FolderId);
               Title = folder.Name;
               foreach (Track track in tracks)
               {
                  if (track.Path.Contains(folder.Path))
                     Tracks.Add(track);
               }
            }
            DataHolder = Tracks;
         }
         catch (Exception)
         {
         }
      }

      public async void OnAppearing()
      {
         if (Tracks.Count == 0)
         {
            await LoadTracks();
         }
      }

      private void OnSearchIconTapped()
      {
         if (ShowSearchBar)
         {
            ShowSearchBar = false;
            QueryText = string.Empty;
         }
         else ShowSearchBar = true;
      }

      public string QueryText
      { 
         get => queryText;
         set
         {
            SetProperty(ref queryText, value);
            if (DataHolder.Count > 0) SearchFilter(value);
         }
      }

      private void SearchFilter(string query)
      {
         if (query.Length > 1)
         {
            IList<Track> filtered = new List<Track>();
            filtered = DataHolder.Where(track => (
               track.Name.ToLower().Contains(query.ToLower()) ||
               track.Artist.ToLower().Contains(query.ToLower())))
               .ToList();
            if (filtered.Count > 0)
            {
               Tracks = filtered;
               PlayerPrepared = false;
            }
         }
         else if (query.Length == 0)
         {
            Tracks = DataHolder;
            PlayerPrepared = false;
         }
      }

      private void OnShuffleTapped()
      {
         if (Tracks.Count == 0) return;
         ShuffleTracks(Tracks);
         PlayerPrepared = ShowPlayerControl = true;
      }

      private async void OnAddToPlaylist(Track track)
      {
         await Shell.Current.GoToAsync($"{nameof(PlaylistsPage)}?{nameof(PlaylistsViewModel.TrackId)}={track.Id}");
      }

      private void OnTrackSelected(Track track)
      {
         if (track == null) return;
         if (!PlayerPrepared)
            PopulateQueue(track.Path, Tracks);
         else
            PlayTrack(track);

         PlayerPrepared = ShowPlayerControl = true;
      }
   }
}