using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jTech.Wpf.Mvvm;
using Snake.Model;

namespace Snake.ViewModel
{
    public interface IGameBoardCellViewModel<T>
    {
        GameBoardCoordinate Coordinate { get; }
        T Content { get; set; }
    }

    public class GameBoardCellViewModel<T> : NotifyPropertyChangedViewModel, IGameBoardCellViewModel<T>
    {
        private readonly GameBoardCoordinate _gameBoardCoordinate;

        public GameBoardCellViewModel(GameBoardCoordinate gameBoardCoordinate) : this(gameBoardCoordinate, default(T)) {  }

        public GameBoardCellViewModel
        (
            GameBoardCoordinate gameBoardCoordinate,
            T content
        )
        {
            _gameBoardCoordinate = gameBoardCoordinate;
            Content = content;
        }

        public GameBoardCoordinate Coordinate { get { return _gameBoardCoordinate; } }

        private T _content;
        public T Content
        {
            get { return _content; }
            set { OnChange(ref _content, value); }
        }

    }
}
