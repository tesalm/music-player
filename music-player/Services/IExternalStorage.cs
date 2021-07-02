using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MediaManager.Library;

public interface IExternalStorage
{
   IList<string> GetAudioFiles();
   IList<string> GetAudioDirectorys();
   ImageSource GetAlbumArt(string path);
   IMediaItem ExtractAudioMetaData(string path);
}
