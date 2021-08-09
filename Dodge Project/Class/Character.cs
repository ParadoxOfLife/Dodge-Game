using System;
using Windows.UI.Xaml.Controls;

namespace Dodge_Project
{
    class Character 
    {
        //Regular members
        protected Canvas _PlayArea;
        protected Image _img;
        static protected int _size = 60;
        private int _step = 15;
        private bool _isAlive; //helps to disable demons that have died
        public int _randomDemon;

        public bool IsAlive//allows for use for all Sub Class's
        {
            get { return _isAlive; }
        }

        public Character(int IsSize)                 //here i tried to change Demons Player Picture so it will be random from 1 to 10
        {                                           //but it went only as far as 2 pngs out of 9 could you please share a secret how to do it
            _size = IsSize;
            _isAlive = true;
        }
        public void Destroy()//Removes img From Canvas
        {
            _PlayArea.Children.Remove(_img);
            _isAlive = false;
        }
        public void PictureChange()
        {
            _PlayArea.Children.Add(_img);
        }//Wanted To Change Dog Oritation With Path but :p its for later
        public void MoveUp()//moves Charecter Up
        {
            if (Canvas.GetTop(_img) <= _step)
                return;
            Canvas.SetTop(_img, Canvas.GetTop(_img) - _step);
        }
        public void MoveDown()//moves Charecter Down
        {
            if (Canvas.GetTop(_img) + _step + _img.Width > _PlayArea.ActualHeight)
                return;
            Canvas.SetTop(_img, Canvas.GetTop(_img) + _step);
        }
        public void MoveLeft()//moves Charecter Left
        {
            if (Canvas.GetLeft(_img) <= _step)
                return;
            Canvas.SetLeft(_img, Canvas.GetLeft(_img) - _step);
        }
        public void MoveRight()//moves Charecter Right
        {
            if (Canvas.GetLeft(_img) + _step + _img.Width > _PlayArea.ActualWidth)
                return;
            Canvas.SetLeft(_img, Canvas.GetLeft(_img) + _step);
        }

        public double GetTop()//Sends Y Position on Canvas
        {
            return Canvas.GetTop(_img);
        }
        public double GetLeft()//sends X Position on Canvas
        {
            return Canvas.GetLeft(_img);
        }
        public double GetHeight()//Sends Img Height
        {
            return _img.Height;
        }
        public double GetWidth()//sends img Width
        {
            return _img.Width;
        }
        public double DemonIcon()//this is how we save icon number into array
        {
            return this._randomDemon;
        }//Allows To Get Demon Random Icon Into Load 
        public override string ToString()//sends X,Y,Health Status into string so can be writen into Save
        {
            return $"{GetTop()},{GetLeft()},{_isAlive},{DemonIcon()},";
        }
    }
}
