using music_player.Models;
using music_player.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using MediaManager;
using MediaManager.Library;

namespace music_player.ViewModels
{
   public class BaseViewModel : INotifyPropertyChanged
   {
      public IDataStore<Folder> FolderDataStore => DependencyService.Get<IDataStore<Folder>>();
      public IDataStore<Track> TrackDataStore => DependencyService.Get<IDataStore<Track>>();
      public IDataStore<TrackPlaylist> PlaylistDataStore => DependencyService.Get<IDataStore<TrackPlaylist>>();

      private static bool showPlayerControl = false;
      public bool ShowPlayerControl
      {
         get => showPlayerControl;
         set => SetProperty(ref showPlayerControl, value);
      }

      private bool isEmpty = false;
      public bool ListIsEmpty
      {
         get => isEmpty;
         set => SetProperty(ref isEmpty, value);
      }

      private string title = string.Empty;
      public string Title
      {
         get { return title; }
         set { SetProperty(ref title, value); }
      }

      public async void PlayPause()
      {
         await CrossMediaManager.Current.PlayPause();
      }

      public async void ShuffleTracks(IList<Track> tracks)
      {
         await CrossMediaManager.Current.Stop();
         CrossMediaManager.Current.Queue.Clear();
         IList<IMediaItem> playlist = new List<IMediaItem>();

         foreach (Track track in tracks)
         {
            playlist.Add(track.MediaItem);
         }
         playlist.Shuffle(new Random().Next(1, 100));

         _ = await CrossMediaManager.Current.Play(playlist);
         CrossMediaManager.Current.RepeatMode = MediaManager.Playback.RepeatMode.All;
      }

      public async void PopulateQueue(string path, IList<Track> tracks)
      {
         await CrossMediaManager.Current.Stop();
         CrossMediaManager.Current.Queue.Clear();
         IList<IMediaItem> playlist = new List<IMediaItem>();
         IMediaItem selectedTrack = null;

         foreach (Track track in tracks)
         {
            if (track.MediaItem.MediaUri == path)
            {
               selectedTrack = track.MediaItem;
            }
            else playlist.Add(track.MediaItem);
         }

         if (playlist.Count > 1)
            playlist.Shuffle(new Random().Next(1, 100));
         playlist.Insert(0, selectedTrack);

         _ = await CrossMediaManager.Current.Play(playlist);
         CrossMediaManager.Current.RepeatMode = MediaManager.Playback.RepeatMode.All;
      }

      public async void PlayTrack(Track track)
      {
         int trackIndex = CrossMediaManager.Current.Queue.IndexOf(track.MediaItem);
         _ = await CrossMediaManager.Current.PlayQueueItem(trackIndex);

         if (!CrossMediaManager.Current.IsPlaying())
            await CrossMediaManager.Current.PlayPause();
      }

      public async void PlayNext()
      {
         _ = await CrossMediaManager.Current.PlayNext();
         if (!CrossMediaManager.Current.IsPlaying())
            await CrossMediaManager.Current.PlayPause();
      }

      protected bool SetProperty<T>(ref T backingStore, T value,
          [CallerMemberName] string propertyName = "",
          Action onChanged = null)
      {
         if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

         backingStore = value;
         onChanged?.Invoke();
         OnPropertyChanged(propertyName);
         return true;
      }

      #region INotifyPropertyChanged
      public event PropertyChangedEventHandler PropertyChanged;
      protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
      {
         var changed = PropertyChanged;
         if (changed == null) return;

         changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
      #endregion
   }
}
