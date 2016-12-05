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
using Snake.Services;
using Snake.ViewModel.Cell;
using Snake.ViewModel.GameArgs;

namespace Snake.ViewModel
{
    public interface ISnakeMasterViewModel
    {
        ICommand ChangePlayerOneDirectionOfTravelCommand { get; }
        ICommand ChangePlayerTwoDirectionOfTravelCommand { get; }
        
        IGameBoardViewModel<SnakeCellViewModel> GameBoard { get; set; }
        bool IsGameRunning { get; set; }

        void Initialise();
        void Reset();
        Task GoSnakeAsync(SnakeyGameArgs gameArgs);
    }
    
    [Export(typeof(ISnakeMasterViewModel))]
    public class SnakeMasterViewModel : NotifyPropertyChangedViewModel, ISnakeMasterViewModel
    {
        private readonly IGameBoardFactory _gameBoardFactory;
        private readonly GameBoardCoordinate _initialCoordinate;
        private readonly Random _random;

        private CancellationTokenSource _snakeGameTokenSource;
        private ISnakeyViewModel _playerOneSnakey;
        private ISnakeyViewModel _playerTwoSnakey;

        [ImportingConstructor]
        public SnakeMasterViewModel
        (
            IGameBoardFactory gameBoardFactory
        )
        {
            _gameBoardFactory = gameBoardFactory;
            _initialCoordinate = new GameBoardCoordinate(1, 11);
            _random = new Random();
        }

        public void Initialise()
        {
            GameBoard = _gameBoardFactory.CreateGameBoard<SnakeCellViewModel>(21, 21);

            Reset();
        }

        #region Commands

        private ICommand _changePlayerOneDirectionOfTravel;
        public ICommand ChangePlayerOneDirectionOfTravelCommand { get { return _changePlayerOneDirectionOfTravel ?? (_changePlayerOneDirectionOfTravel = new RelayCommand<DirectionOfTravel>(DoChangePlayerOneDirectionOfTravel)); } }

        private void DoChangePlayerOneDirectionOfTravel(DirectionOfTravel directionOfTravel)
        {
            try
            {
                _playerOneSnakey.ChangeDirectionOfTravel(directionOfTravel);
            }
            catch (Exception)
            {
                // TODO
            } 
        }

        private ICommand _changePlayerTwoDirectionOfTravel;
        public ICommand ChangePlayerTwoDirectionOfTravelCommand { get { return _changePlayerTwoDirectionOfTravel ?? (_changePlayerTwoDirectionOfTravel = new RelayCommand<DirectionOfTravel>(DoChangePlayerTwoDirectionOfTravel)); } }

        private void DoChangePlayerTwoDirectionOfTravel(DirectionOfTravel directionOfTravel)
        {
            try
            {
                ISnakeyViewModel snakey = _playerTwoSnakey ?? _playerOneSnakey;

                snakey.ChangeDirectionOfTravel(directionOfTravel);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        #endregion

        #region BoundProperties

        private IGameBoardViewModel<SnakeCellViewModel> _gameBoard;
        public IGameBoardViewModel<SnakeCellViewModel> GameBoard
        {
            get { return _gameBoard; }
            set { OnChange(ref _gameBoard, value); }
        }

        private bool _isGameRunning;
        public bool IsGameRunning
        {
            get { return _isGameRunning; }
            set { OnChange(ref _isGameRunning, value); }
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set { OnChange(ref _score, value); }
        }

        private int _highScore;
        public int HighScore
        {
            get { return _highScore; }
            set { OnChange(ref _highScore, value); }
        }

        #endregion

        public void Reset()
        {
            _playerOneSnakey = null;
            _playerTwoSnakey = null;
            Score = 0;
            GameBoard.ResetBoard(GenerateCell);
        }

        public async Task GoSnakeAsync(SnakeyGameArgs gameArgs)
        {
            try
            {
                _snakeGameTokenSource = new CancellationTokenSource();

                var initialTimeSpan = TimeSpan.FromSeconds(0.2);
                
                // Create snake(s).
                if (gameArgs.GameMode == GameMode.OnePlayer)
                {
                    _playerOneSnakey = new SnakeyViewModel(
                        SnakeyToken.SnakeyOne,
                        initialTimeSpan,
                        gameArgs.PlayerOneArgs.DoesSnakeSpeedUp,
                        DirectionOfTravel.Right,
                        new GameBoardCoordinate(2, 11));
                }
                else if (gameArgs.GameMode == GameMode.TwoPlayer)
                {
                    _playerOneSnakey = new SnakeyViewModel(
                        SnakeyToken.SnakeyOne,
                        initialTimeSpan,
                        gameArgs.PlayerOneArgs.DoesSnakeSpeedUp,
                        DirectionOfTravel.Up,
                        new GameBoardCoordinate(19, 18));

                    _playerTwoSnakey = new SnakeyViewModel(
                        SnakeyToken.SnakeyTwo,
                        initialTimeSpan,
                        gameArgs.PlayerTwoArgs.DoesSnakeSpeedUp,
                        DirectionOfTravel.Down,
                        new GameBoardCoordinate(1, 2));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("GameMode", "GameMode must be OnePlayer or TwoPlayer");
                }

                // Initialise player one.
                foreach (var snakeBody in _playerOneSnakey.SnakeBody())
                {
                    var gameCell = GameBoard[snakeBody];
                    gameCell.Content.SnakeyToken = _playerOneSnakey.SnakeyToken;
                }

                // Initialise player two.
                if (_playerTwoSnakey != null)
                {
                    foreach (var snakeBody in _playerTwoSnakey.SnakeBody())
                    {
                        var gameCell = GameBoard[snakeBody];
                        gameCell.Content.SnakeyToken = _playerTwoSnakey.SnakeyToken;
                    }
                }

                GenerateNewTreat();

                // Give the player(s) a chance to get ready.
                await Task.Delay(TimeSpan.FromSeconds(1.5));

                // Snake on!
                await Task.WhenAll(
                    DriveSnakeyAsync(_playerOneSnakey, _snakeGameTokenSource),
                    Task.Run(async () => 
                    {
                        if (gameArgs.GameMode != GameMode.TwoPlayer)
                        {
                            return;
                        }

                        await DriveSnakeyAsync(_playerTwoSnakey, _snakeGameTokenSource);
                    }));
            }
            catch (Exception)
            {
                // TODO:
            }
        }

        private async Task DriveSnakeyAsync(ISnakeyViewModel snakey, CancellationTokenSource snakeGameTokenSource)
        {
            while (snakeGameTokenSource.Token.IsCancellationRequested == false)
            {
                MoveSnake(snakey);

                await Task.Delay(snakey.Interval);
            }
        }

        private void MoveSnake(ISnakeyViewModel snakey)
        {
            try
            {
                var directionOfTravel = snakey.GetDirectionOfTravel();

                var newSnakeHead = snakey.AddNewSnakeSegment(directionOfTravel);

                var gameCell = GameBoard[newSnakeHead];

                if (gameCell.Content.HasSnakey)
                {
                    GameOver();
                }
                else
                {
                    var treat = gameCell.Content as SnakeTreatCellViewModel;
                    if (treat != null)
                    {
                        gameCell.Content = new SnakeCellViewModel();

                        // Yummy!
                        snakey.SpeedUpSnakey();

                        Score++;

                        GenerateNewTreat();

                        // No need for me to remove a segment!
                    }
                    else
                    {
                        var oldSnakeTail = snakey.RemoveSnakeSegment();

                        var tailGameCell = _gameBoard[oldSnakeTail];
                        tailGameCell.Content.SnakeyToken = null;
                    }

                    // Add Snakey to cell
                    gameCell.Content.SnakeyToken = snakey.SnakeyToken;
                }
            }
            catch (OutOfGameBoardException outOfGameBoardException)
            {
                GameOver();
            }
            catch (Exception)
            {
                // TODO:
            }
        }

        private SnakeCellViewModel GenerateCell(GameBoardCoordinate coordinate)
        {
            return new SnakeCellViewModel();
        }

        private void GenerateNewTreat()
        {
            var freeCells = _gameBoard.Cells()
                                      .Where(c => c.Content.HasSnakey == false)
                                      .ToList();
            var count = freeCells.Count;

            var cell = freeCells[_random.Next(count)];

            var treats = (SnakeTreatCellViewModel.TreatType[])Enum.GetValues(typeof(SnakeTreatCellViewModel.TreatType));
            count = treats.Count();

            var treat = treats[_random.Next(count)];

            cell.Content = new SnakeTreatCellViewModel(treat);
        }

        private void GameOver()
        {
            _snakeGameTokenSource.Cancel();

            if (HighScore < Score)
            {
                HighScore = Score;
            }
        }

    }

}
