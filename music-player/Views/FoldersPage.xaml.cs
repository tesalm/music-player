using music_player.Models;
using music_player.ViewModels;
using music_player.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace music_player.Views
{
   public partial class FoldersPage : ContentPage
   {
      FoldersViewModel _viewModel;

      public FoldersPage()
      {
         InitializeComponent();

         BindingContext = _viewModel = new FoldersViewModel();
      }

      protected override void OnAppearing()
      {
         base.OnAppearing();
         _viewModel.OnAppearing();
      }
   }
}