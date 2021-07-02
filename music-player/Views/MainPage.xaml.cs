using Xamarin.Forms;
using music_player.ViewModels;

namespace music_player.Views
{
   public partial class MainPage : ContentPage
   {
      MainViewModel _viewModel;
      public MainPage()
      {
         InitializeComponent();
         BindingContext = _viewModel = new MainViewModel();
      }

      protected override void OnAppearing()
      {
         base.OnAppearing();
         _viewModel.OnAppearing();
      }
   }
}