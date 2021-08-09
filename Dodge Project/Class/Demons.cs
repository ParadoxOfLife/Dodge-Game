using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Dodge_Project
{
    class Demons : Character
    {
        //Class memebers
        int _LocationVertical = 100;
        int _LocationHorizontal = 75;
        string _randomDeamonNumber;
        string _src;

        public int iSSize
        {
            get { return _size; }
        }
        private string SetRandomPath()//just long way how to stick img into few constructors
        {

            string path;
            string randomString = _randomDeamonNumber = _randomDemon.ToString();
            path = $"ms-appx:///Poop Pictures/poo{randomString}.gif";
            return randomString;
        }
        public Demons(Canvas PlayArea, int index) : base(_size)//Demons Constructor
        {
            _PlayArea = PlayArea;
            _img = new Image();
            _src = SetRandomPath();
            _img.Source = new BitmapImage(new Uri(_src));
            _img.Height = _size - 10;
            _img.Width = _size - 10;
            Canvas.SetLeft(_img, _LocationVertical*(index+1));
            Canvas.SetTop(_img, _LocationHorizontal);
            PlayArea.Children.Add(_img);
        }
        public Demons(Canvas PlayArea, int top, int left, bool isAlive, int randomDeamonNumber) : base(_size)//Demons Constructor that allows to Load
                                                                                                             //Change Icons by Random
        {
            _randomDemon = randomDeamonNumber;
            _PlayArea = PlayArea;
            _img = new Image();
            string str = SetRandomPath();
            _src = $"ms-appx:///Poop Pictures/poo{str}.gif";
            _img.Source = new BitmapImage(new Uri(_src));
            _img.Height = _size-10;
            _img.Width = _size-10;
            _randomDeamonNumber = randomDeamonNumber.ToString();
            Canvas.SetLeft(_img, left);
            Canvas.SetTop(_img, top);
            PlayArea.Children.Add(_img);
        }
    }
}
