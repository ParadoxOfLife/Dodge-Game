using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Dodge_Project
{
    class Game
    {
        //Some Members that used in this Class
        Player _player;
        private MediaElement _bckSound = new MediaElement();
        Canvas _PlayArea;
        Demons[] _Demons;
        Random rnd = new Random();
        DispatcherTimer Timer;
        int _NumberOfDemons = 10;

        //make it acceseable to Demon Class so can use random on Pictures Of poo

           
        //Memebers for Message Pop ups At XAML
        ContentDialog _MyDialogLose = new ContentDialog();
        ContentDialog _MyDialogWin = new ContentDialog();

        //Demons Death Gif Animation Memebers
        private double _DemonsXpop;
        private double _DemonsYpop;
        DispatcherTimer TimerPop;
        private UIElement _PopImage;
        List<Image> _PopImageArr = new List<Image>();


        public Game(Canvas PlayArea, ContentDialog MyDialogWin, ContentDialog MyDialogLose)//Creates Canvas with timer so that timer will be present always
        {                           //can see a bug in future with timer but in our case its only 100ms not relevant :)
            _PlayArea = PlayArea;
            _Demons = new Demons[_NumberOfDemons];
            //Game Timer
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            //Pop Animation Clean Timer
            TimerPop = new DispatcherTimer();
            TimerPop.Interval = new TimeSpan(0, 0, 0, 1, 500);
            TimerPop.Tick += PopTimer;
            //Defining Dialog Pop Ups
            _MyDialogWin = MyDialogWin;
            _MyDialogLose = MyDialogLose;

        }
        public async void NewGame()//Used in MainPage.cs Assign to Button
        {
            StopBgMusic();//Stops Music from Previus Game
            await PlayBackgroundMusicSound();//Play Music
            Wipe(); //Removes Everything from Canvas
            CreateGame();//Create Player + Demons
        }
        private void Wipe()//Deletes All alive Charecters on Canvas
        {
            if (_player != null)
                _player.Destroy();
            for (int i = 0; i < _NumberOfDemons; i++)
            {
                if (_Demons[i] != null)
                    _Demons[i].Destroy();
            }
        }
        private void CreateGame()//Create Player + Demons , Starts Timers and Stop,Play Music
        {
            StopIntro();
            TimerPop.Start();
            Timer.Stop();
            CreatePlayer();
            CreateDemons();
            Timer.Start();
        }
        private void CreatePlayer()//Create Player At "Random" sort of Position
        {
            int _LocationVerticalrnd = 0;
            int _LocationHorizontalrnd = 0;
            _LocationVerticalrnd = rnd.Next(300, 600);
            _LocationHorizontalrnd = rnd.Next(1, 1000);
            _player = new Player(_PlayArea, _LocationVerticalrnd, _LocationHorizontalrnd, true);
        }
        private void CreateDemons()//Create Demons At "Random" sort of Position 
        {
            int _randomDemon;
            int _LocationVerticalrnd = 0;
            int _LocationHorizontalrnd = 0;
            for (int i = 0; i < _NumberOfDemons; i++)//Generates random Location and icon for each Demon :)
            {
                
                int randomDeamonNumber = rnd.Next(1, 5);
                _randomDemon = randomDeamonNumber;     
                                                        
                _LocationVerticalrnd = rnd.Next(0, 2001);
                _LocationHorizontalrnd = rnd.Next(0, 2001);
                _Demons[i] = new Demons(_PlayArea, _LocationVerticalrnd, _LocationHorizontalrnd, true, _randomDemon);
            }
        }
        public void GamePause()//this Pause Timer used by Event in MainPage.cs
        {
            TimerPop.Stop();
            Timer.Stop();
            PauseBgMusic();
        }
        public void GameResume()//this Start Timer used by Event in MainPage.cs
        {
            TimerPop.Start();
            Timer.Start();
            ResumePlayingBGMusic();
        }
        private void Timer_Tick(object sender, object e)//Group of Events that driven by timer
        {
            GameWonByPlayer();
            CollisionWithPlayer();//Collision betwen Player And Demon
            GameLost();
            CollissionWithDemon();//Collision Between Demon's
            Chase();
        }
        private void GameLost()//Show Msg When Player Lose game.
        {
            if (!_player.IsAlive)
            {
                MyDialogLose();
            }
        }
        private void Chase()//Code for Demons to Follow Player very basic :)
        {
            for (int i = 0; i < _NumberOfDemons; i++)
            {
                int randDemon = rnd.Next(15); //Freezes Demons from chasing
                if (_player.GetTop() < _Demons[i].GetTop())        //Check Location of Player and moves Demon Up
                {
                    if (randDemon != 0)
                        _Demons[i].MoveUp();
                }
                else if (_player.GetTop() > _Demons[i].GetTop())   //Check Location of Player and moves Demon Down
                {
                    if (randDemon != 0)
                        _Demons[i].MoveDown();
                }
                if (_player.GetLeft() < _Demons[i].GetLeft())      //Check Location of Player and moves Demon Left
                {
                    if (randDemon != 0)
                        _Demons[i].MoveLeft();
                }
                else if (_player.GetLeft() > _Demons[i].GetLeft()) //Check Location of Player and moves Demon Right
                {
                    if (randDemon != 0)
                        _Demons[i].MoveRight();
                }
            }
        }
        private async void CollisionWithPlayer()//Removes Player From Canvas
        {
            for (int i = 0; i < _NumberOfDemons; i++)//runs in loop on every Demon to check Collison with player
            {
                if (!_Demons[i].IsAlive) continue;
                if (Math.Abs(_Demons[i].GetTop() - _player.GetTop()) < _player.GetHeight() &&
                   Math.Abs(_Demons[i].GetLeft() - _player.GetLeft()) < _player.GetWidth())
                {
                    Timer.Stop();
                    await PlayPlayerDeathSound();
                    _player.Destroy();
                    _bckSound.Stop();
                    MyDialogLose();
                    //Few sounds Added here
                }
            }
            
        }
        
        private async void CollissionWithDemon()//Removes Demon From Canvas when Demon&Demon Collides with each other only one of them
        {
            for (int i = 0; i < _NumberOfDemons; i++)
            {
                if (!_Demons[i].IsAlive) continue;
                for (int j = 0; j < _NumberOfDemons; j++) 
                {
                    if (!_Demons[j].IsAlive) continue;
                    if (i != j && _Demons[i].GetTop() > _Demons[j].GetTop() - _Demons[i].iSSize
                               && _Demons[i].GetTop() < _Demons[j].GetTop() + _Demons[i].iSSize
                               && _Demons[i].GetLeft() > _Demons[j].GetLeft() - _Demons[j].iSSize
                               && _Demons[i].GetLeft() < _Demons[j].GetLeft() + _Demons[j].iSSize)
                    {
                        _DemonsXpop = (int)_Demons[i].GetLeft();
                        _DemonsYpop = (int)_Demons[i].GetTop();
                        await PlayDemonDeathSound();
                        _Demons[i].Destroy();
                        PopImage();
                        //Sound Added also sends X,Y to Death Animation Constractor
                    }
                }
            }
        }
        private void PopImage()
        {
            Image PopImage = new Image();
            Uri uri = new Uri("ms-appx:///Assets/Death.gif");
            PopImage.Source = new BitmapImage(uri);
            //Apply the size to our new Goodie
            PopImage.Height = 70;
            PopImage.Width = 70;

            //Set the x,y position of the Explosion
            Canvas.SetLeft(PopImage, _DemonsXpop);
            Canvas.SetTop(PopImage, _DemonsYpop);

            //Adding Explosion to The Canvas
            _PopImage = PopImage;
            _PlayArea.Children.Add(PopImage);
            _PopImageArr.Add(PopImage);
        }//Creating Death Animation On Demon and placing on Canvas
        private void PopTimer(object sender, object e)
        {
            foreach (var item in _PopImageArr)
            {
                Canvas.SetLeft(item, _DemonsXpop);
                Canvas.SetTop(item, _DemonsYpop);
                _PlayArea.Children.Remove(item);
            }
        }//Allows to Clear Death Animation After Some Period

        private void GameWonByPlayer()//Checks How many Demons are alive in a loop,When only 1 demon left Shows Game won msg
        {
            int DemonNumbers = 0;
            for (int i = 0; i < _NumberOfDemons; i++)
            {
                if (_Demons[i].IsAlive)
                {
                    DemonNumbers++;
                }
            }
            if (DemonNumbers ==1)
            {
                MyDialogWin();
            }
            
        }


        public void MovePlayerUp()//Moves Player Up
        {
            _player.MoveUp();
            
        }
        public void MovePlayerDown()//Moves Player Down
        {
            _player.MoveDown();
        }
        public void MovePlayerLeft()//Moves Player Left
        {
            _player.MoveLeft();
        }
        public void MovePlayerRight()//Moves Player Right
        {
            _player.MoveRight();
        }

        public async void Save()//Creates Array and puts x,y,Health status into each 
        {
            string fileName = "SaveGame.txt";
            string[] stringOutput = new string[(_NumberOfDemons+1)];
            stringOutput[_NumberOfDemons] = _player.ToString();//did put Player at end of array since easier to maintain if you change quantity of Demons
            for (int i = 0; i < stringOutput.Length-1; i++)
            {
                stringOutput[i] = _Demons[i].ToString();
            }
            

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile manifestFile = await storageFolder.GetFileAsync(fileName);
                await manifestFile.DeleteAsync();
            }
            catch (Exception e)
            {

            }
            var YourFileObject = await storageFolder.CreateFileAsync(fileName);
            await FileIO.WriteLinesAsync(YourFileObject, stringOutput);
        }
        public async void Load()//Clears PlayArea after that reads arry and calls methods that creates 
        {                       //player and demons by their vaule in array
            Wipe();
            string fileName = "SaveGame.txt";
            string stringOutput;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile manifestFile = await storageFolder.GetFileAsync(fileName);
                stringOutput = await FileIO.ReadTextAsync(manifestFile);
                string[] mystringOutput = stringOutput.Split('\n');
                LoadGame(mystringOutput);
            }
            catch (Exception e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
        }
        public void LoadGame(string[] mystringOutput)        //here had to adjust construct to take 3,4 variables it has some bug
        {                                                    // i have no idea how to fix it so kinda hacked it
            string[] Temp;                                   //somehow my array turns into [12] slots when it only should be [11]
                                                             
            for (int i = 0; i < mystringOutput.Length-2; i++)//here lowered array length by 2 in account of Player+that bugged array[12]
            {                                                //if you have any idea what could be wrong please help <3
                Temp = mystringOutput[i].Split(',');
                _Demons[i] = new Demons(_PlayArea, int.Parse(Temp[0]), int.Parse(Temp[1]), bool.Parse(Temp[2]), int.Parse(Temp[3]));
            }
            Temp = mystringOutput[_NumberOfDemons].Split(',');
            _player = new Player(_PlayArea, int.Parse(Temp[0]), int.Parse(Temp[1]), bool.Parse(Temp[2]));
        }

        //Huge Problem on Load in Array Somehow it Replace Health Status and Overrites Icon Number on Demons : <
        
        private async void MyDialogLose()//Stops Timer and Display Msg
        {
            Timer.Stop();
            TimerPop.Stop();
            await _MyDialogLose.ShowAsync();
        }

        private async void MyDialogWin()
        {
            Timer.Stop();
            TimerPop.Stop();
            await _MyDialogWin.ShowAsync();
        }//Stops Timer and Display Msg

        //From Here There are Sound Controls That Play,Pause,Resume

        public async Task<MediaElement> PlayBackgroundMusicSound()
        {
            var BgMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Spooky Scary Skeledogs.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BgMusicElement.SetSource(stream, "");
            BgMusicElement.Play();
            BgMusicElement.Volume = 0.5;
            _bckSound = BgMusicElement;
            return BgMusicElement;
        }
        
        public async Task<MediaElement> PlayDemonDeathSound()
        {
            var PlayDemonDeathSound = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Pop.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            PlayDemonDeathSound.SetSource(stream, "");
            PlayDemonDeathSound.Play();
            PlayDemonDeathSound.Volume = 0.5;
            return PlayDemonDeathSound;
        }
        public async Task<MediaElement> PlayPlayerDeathSound()
        {
            var PlayPlayerDeathSound = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Dog Howling.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            PlayPlayerDeathSound.SetSource(stream, "");
            PlayPlayerDeathSound.Play();
            PlayPlayerDeathSound.Volume = 0.5;
            return PlayPlayerDeathSound;
        }
        public async Task<MediaElement> PlayIntro()
        {
            var PlayIntro = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Intro.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            PlayIntro.SetSource(stream, "");
            PlayIntro.Play();
            PlayIntro.Volume = 0.6;
            return PlayIntro;
        }
        public void StopIntro()
        {
            _bckSound.Stop();
        }
        public void StopBgMusic()
        {
            _bckSound.Stop();
        }     

        public void PauseBgMusic()
        {
            _bckSound.Pause();
        }    
        public MediaElement ResumePlayingBGMusic()  //Unpause the game. The music will start from the paused state :-))
        {
            var BgMusicElement = new MediaElement();
            BgMusicElement = _bckSound;
            BgMusicElement.Play();
            return BgMusicElement;
        }
    }
}
