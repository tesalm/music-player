using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;

namespace music_player.Droid
{
   [Activity(Label = "Music Player", Icon = "@mipmap/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
   public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
   {
      protected async override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         Window.SetStatusBarColor(Android.Graphics.Color.Black);
         Xamarin.Essentials.Platform.Init(this, savedInstanceState);
         global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
         await TryToGetPermissions();
         base.SetTheme(Resource.Style.MainTheme);
         LoadApplication(new App());
      }
      public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
      {
         Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

         base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      }

      #region RuntimePermissions
      async Task TryToGetPermissions()
      {
         if ((int)Build.VERSION.SdkInt >= 23)
         {
            await GetPermissionsAsync();
            return;
         }
      }
      async Task GetPermissionsAsync()
      {
         var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
         if (status != PermissionStatus.Granted)
         {
            await Permissions.RequestAsync<Permissions.StorageRead>();
         }
      }
      #endregion
   }
}