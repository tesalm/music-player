using music_player.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace music_player.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class TracksPage : ContentPage
   {
      TracksViewModel _viewModel;

      public TracksPage()
      {
         InitializeComponent();

         BindingContext = _viewModel = new TracksViewModel();
      }

      protected override void OnAppearing()
      {
         base.OnAppearing();
         _viewModel.OnAppearing();
      }

      public void Search_Clicked(object sender, System.EventArgs args)
      {
         search.IsVisible = !search.IsVisible;
         listView.VerticalOptions = LayoutOptions.Start;
         listView.VerticalOptions = LayoutOptions.FillAndExpand;

         if (search.IsVisible)
         {
            _ = search.Focus();
         }
      }
   }
}