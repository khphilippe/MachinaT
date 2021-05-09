using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Machina
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ICommand AnimationClickedCommand { get; set; }

        public MainPage()
        {
            AnimationClickedCommand = new Command(() =>    
            {
                StartButtonClickedAsync();
            });
            BindingContext = this;

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void StartButtonClicked(object sender, EventArgs e)
        {
            StartButtonClickedAsync();
        }

        private async Task StartButtonClickedAsync()
        {
            // Tester si on a accès à internet
            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Erreur", "Vous devez être connecté au réseau", "OK");
                return;
            }

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Erreur", "La caméra n'est pas disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            }) ;

            if (file == null)
            {
                // Quand l'utilisateur annule la photo
                //await DisplayAlert("Erreur", "Pas de fichier", "OK");
                return;
            }


            //await DisplayAlert("Réussi", file.Path, "OK");

            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });*/


            await Navigation.PushAsync(new ScannerPage(file), false);
        }
    }
}
