using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using jTech.Wpf.Mvvm;
using Snake.Model;
using Snake.ViewModel.GameArgs;

namespace Snake.ViewModel
{
    public interface ISnakeyGameSummaryViewModel
    {
        ICommand SetGameModeCommand { get; }
        ICommand ReleaseCommand { get; }

        bool IsDisplayingSummary { get; set; }
        List<GameMode> GameModes { get; }
        GameMode SelectedGameMode { get; set; }
        SnakePlayerArgs PlayerOneGameArgs { get; set; }
        SnakePlayerArgs PlayerTwoGameArgs { get; set; }

        Task<SnakeyGameArgs> GetGameArgsAsync();
    }

    [Export(typeof(ISnakeyGameSummaryViewModel))]
    public class SnakeyGameSummaryViewModel : NotifyPropertyChangedViewModel, ISnakeyGameSummaryViewModel
    {
        private readonly object _sessionLock;

        private SemaphoreSlim _token;

        [ImportingConstructor]
        public SnakeyGameSummaryViewModel()
        {
            _sessionLock = new object();
            GameModes = ((GameMode[])Enum.GetValues(typeof(GameMode))).ToList();
            SelectedGameMode = GameModes.FirstOrDefault();
            PlayerOneGameArgs = new SnakePlayerArgs();
            PlayerTwoGameArgs = new SnakePlayerArgs();
        }

        #region RelayCommands

        private ICommand _setGameMode;
        public ICommand SetGameModeCommand { get { return _setGameMode ?? (_setGameMode = new RelayCommand<GameMode>(DoSetGameMode)); } }

        private void DoSetGameMode(GameMode gameMode)
        {
            SelectedGameMode = gameMode;
        }

        private ICommand _release;
        public ICommand ReleaseCommand { get { return _release ?? (_release = new RelayCommand(DoRelease)); } }

        private void DoRelease()
        {
            try
            {
                if (_token == null) return;
                lock (_sessionLock)
                {
                    if (_token == null) return;

                    _token.Release();   
                }
            }
            catch (Exception)
            {
                
                // TODO
            }
        }

        #endregion

        #region BoundProperties

        private bool _isDisplayingSummary;
        public bool IsDisplayingSummary
        {
            get { return _isDisplayingSummary; }
            set { OnChange(ref _isDisplayingSummary, value); }
        }

        public List<GameMode> GameModes { get; private set; }

        private GameMode _selectedGameMode;
        public GameMode SelectedGameMode
        {
            get { return _selectedGameMode; }
            set { OnChange(ref _selectedGameMode, value); }
        }

        private SnakePlayerArgs _playerOneGameArgs;
        public SnakePlayerArgs PlayerOneGameArgs
        {
            get { return _playerOneGameArgs; }
            set { OnChange(ref _playerOneGameArgs, value); }
        }

        private SnakePlayerArgs _playerTwoGameArgs;
        public SnakePlayerArgs PlayerTwoGameArgs
        {
            get { return _playerTwoGameArgs; }
            set { OnChange(ref _playerTwoGameArgs, value); }
        }

        #endregion

        public async Task<SnakeyGameArgs> GetGameArgsAsync()
        {
            if (_isDisplayingSummary == false)
            {
                lock (_sessionLock)
                {
                    if (_isDisplayingSummary == false)
                    {
                        _token = new SemaphoreSlim(0, 1);
                        IsDisplayingSummary = true;
                    }
                }
            }

            try
            {
                await _token.WaitAsync();

                return new SnakeyGameArgs(SelectedGameMode, PlayerOneGameArgs, PlayerTwoGameArgs);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
            finally
            {
                lock (_sessionLock)
                {
                    _token = null;
                    IsDisplayingSummary = false;
                }
            }
        }

    }
}
