using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.IO;
using Android.Media;
using Android.Provider;
using MediaManager.Library;

[assembly: Dependency(typeof(music_player.Droid.ExternalStorage))]
namespace music_player.Droid
{
   public class ExternalStorage : IExternalStorage
   {
      private bool ContainsAudio(string dir)
      {
         string[] file = Directory.GetFiles(dir);
         foreach (string f in file)
         {
            if (f.EndsWith("mp3")) return true;
         }
         return false;
      }

      private IList<string> SearchAudioDirectorys(string root)
      {
         IList<string> audiodirs = new List<string>();
         IList<string> dirs = Directory.GetDirectories(root, "*", SearchOption.AllDirectories).ToList();
         dirs.Add(root);
         foreach (string path in dirs)
         {
            if (path.Contains(Android.OS.Environment.DirectoryRingtones) ||
               path.Contains(Android.OS.Environment.DirectoryNotifications)) continue;
            if (Directory.Exists(path) && ContainsAudio(path))
            {
               audiodirs.Add(path);
            }
         }
         return audiodirs;
      }

      public ImageSource GetAlbumArt(string path)
      {
         MediaMetadataRetriever mmr = new MediaMetadataRetriever();
         mmr.SetDataSource(path);
         byte[] art = mmr.GetEmbeddedPicture();
         if (art == null) return "record.png";
         ImageSource img = ImageSource.FromStream(() => new MemoryStream(art));
         return img;
      }

      private Android.Graphics.Bitmap GetBitmap(byte[] byte_arr)
      {
         if (byte_arr == null) return null;
         Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(byte_arr, 0, byte_arr.Length);
         return bitmap;
      }

      public IMediaItem ExtractAudioMetaData(string path)
      {
         MediaMetadataRetriever mmr = new MediaMetadataRetriever();
         mmr.SetDataSource(path);
         string artist = mmr.ExtractMetadata(MetadataKey.Artist);
         string album = mmr.ExtractMetadata(MetadataKey.Album);
         if (String.IsNullOrEmpty(artist)) artist = "unknown";
         if (String.IsNullOrEmpty(album)) album = "unknown";
         byte[] art = mmr.GetEmbeddedPicture();
         var artBitmap = GetBitmap(art);
         string fileName = Path.GetFileNameWithoutExtension(path);

         ImageSource img = null;
         if (art == null) img = "record.png";
         else img = ImageSource.FromStream(() => new MemoryStream(art));

         IMediaItem audioMeta = new MediaItem()
         {
            IsMetadataExtracted = true,
            Artist = artist,
            Album = album,
            FileName = fileName,
            MediaUri = path,
            DisplayImage = artBitmap,
            Image = img
         };

         return audioMeta;
      }

      [Obsolete]
      public IList<string> GetAudioFiles()
      {
         IList<string> files = new List<string>();
         foreach (string dir in GetAudioDirectorys())
         {
            List<string> enumfiles = Directory.EnumerateFiles(dir).ToList<string>();
            enumfiles.ForEach(f => { if (f.EndsWith("mp3")) files.Add(f); });
         }
         return files;
      }

      [Obsolete]
      public IList<string> GetAudioDirectorys()
      {
         try {
            List<string> temp = new List<string>();
            IList<string> dirList = new List<string>();

            if (Build.VERSION.SdkInt <= BuildVersionCodes.Q)
            {
               string path = Android.OS.Environment.GetExternalStoragePublicDirectory(
                  Android.OS.Environment.DirectoryMusic).AbsolutePath;
               temp.Add(path);
               temp.AddRange(Directory.GetDirectories(path, "*", SearchOption.AllDirectories));
            }
            else
            {
               string path = Path.Combine(
                  Android.OS.Environment.StorageDirectory.AbsolutePath,
                  "emulated", "0",
                  Android.OS.Environment.DirectoryMusic);
               temp.Add(path);
               temp.AddRange(Directory.GetDirectories(path, "*", SearchOption.AllDirectories));
            }
            temp.ForEach(dir => {
               if (Directory.Exists(dir) && ContainsAudio(dir))
               {
                  dirList.Add(dir);
               }
            });
            return dirList;
         }
         catch (Exception) {
            throw;
         }
      }
   }
}