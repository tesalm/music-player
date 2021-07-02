using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using music_player.ViewModels;

namespace music_player.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class PlaylistsPage : ContentPage
   {
      PlaylistsViewModel _viewModel;

      public PlaylistsPage()
      {
         InitializeComponent();
         BindingContext = _viewModel = new PlaylistsViewModel();
      }

      protected override void OnAppearing()
      {
         base.OnAppearing();
         _viewModel.OnAppearing();
      }
   }
}