using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Dodge_Project
{
    class Player : Character
    {
        //Class Memebers
        int _LocationVertical = 500;
        int _LocationHorizontal = 500;
        

        public Player(Canvas PlayArea) : base(_size)//Player constractor
        {
            _PlayArea = PlayArea;
            _img = new Image();
            _img.Source = new BitmapImage(new Uri("ms-appx:///Doggy Picture/dog.gif"));
            _img.Height = _size;
            _img.Width = _size;
            Canvas.SetLeft(_img, _LocationVertical);
            Canvas.SetTop(_img, _LocationHorizontal);
            PlayArea.Children.Add(_img);
        }
        public Player(Canvas PlayArea, int top, int left, bool isAlive) : base(_size)//Player Constractor that allows to Load
        {
            _PlayArea = PlayArea;
            _img = new Image();
            _img.Source = new BitmapImage(new Uri("ms-appx:///Doggy Picture/dog.gif"));
            _img.Height = _size;
            _img.Width = _size;
            Canvas.SetLeft(_img, left);
            Canvas.SetTop(_img, top);
            PlayArea.Children.Add(_img);
        }
    }
}
