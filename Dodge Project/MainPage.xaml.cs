using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dodge_Project
{
    public sealed partial class MainPage : Page
    {
        //Memebers
        Game _game;
        bool isPaused;

        public MainPage()
        {
            isPaused = false;
            this.InitializeComponent();
            _game = new Game(PlayArea, MyDialogWin, MyDialogLose);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown; 
        }//Main Page that Use Windows keys and setup Game
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
                switch (args.VirtualKey)//Sense Key press and moves player , Disabled when game is paused
                {
                    case VirtualKey.Up:
                        if (isPaused == false)
                        _game.MovePlayerUp();
                        break;
                    case VirtualKey.Down:
                        if (isPaused == false)
                        _game.MovePlayerDown();
                        break;
                    case VirtualKey.Left:
                        if (isPaused == false)
                        _game.MovePlayerLeft();
                        break;
                    case VirtualKey.Right:
                        if (isPaused == false)
                        _game.MovePlayerRight();
                        break;
                }
        }
        private void Page_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void AppBarButtonNewGame_Click(object sender, RoutedEventArgs e)//Start new game Resets Puase icon & Status to Flase
        {
            _game.NewGame();
            isPaused = false;
            AppBarButtonPauseGame.Icon = new SymbolIcon(Symbol.Pause);
        }

        private void AppBarButtonPauseGame_Click(object sender, RoutedEventArgs e)//Puase Event that change icon of Puase int two states
        {                                                                         //Pause and Resumes by one click
            if (!isPaused)
            {
                AppBarButtonPauseGame.Icon = new SymbolIcon(Symbol.Play);
                _game.GamePause();
            }
            else
            {
                AppBarButtonPauseGame.Icon = new SymbolIcon(Symbol.Pause);
               _game.GameResume();
            }
            isPaused = !isPaused;
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)//Loads Game From Array and noticed and Prevents bug by Reseting Puase Event
        {                                                            //To Event Before the Load itself :)
            _game.Load();

            if (isPaused)
                _game.GamePause();
            else
                _game.GameResume();
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)//Simple Save Method that Pass Char info into Arry
        {
            _game.Save();
        }

        private async void ShowIntro()
        {
            await MyDialogWelcome.ShowAsync();
        }//intro for Page load

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShowIntro();
            await _game.PlayIntro();
        }//Plays Sound and Pop up at Start

    }
}
