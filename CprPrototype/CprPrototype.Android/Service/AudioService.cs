using System;
using CprPrototype.Service;
using Xamarin.Forms;
using CprPrototype.Droid.Service;
using Android.Media;

[assembly: Dependency(typeof(AudioService))]
namespace CprPrototype.Droid.Service
{
    public class AudioService : IAudio
    {
        public AudioService() { }

        private MediaPlayer _mediaPlayer;

        public bool PlayMp3File(int i)
        {
            if (i == 1)
            {
                _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.beep1);
                _mediaPlayer.Start();
            }
            else
            {
                _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.beep2);
                _mediaPlayer.Start();
            }

            return true;
        }
    }
}