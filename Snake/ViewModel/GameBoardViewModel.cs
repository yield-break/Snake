using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jTech.Wpf.Mvvm;
using Snake.Services;
using Snake.Model;

namespace Snake.ViewModel
{
    public interface IGameBoardViewModel<T>
    {
        IGameBoardCellViewModel<T> this[GameBoardCoordinate index] { get; }
        ICollection<ICollection<IGameBoardCellViewModel<T>>> RowsAndColumns { get; set; }
    
        IGameBoardViewModel<T> ResetBoard();
        IGameBoardViewModel<T> ResetBoard(Func<GameBoardCoordinate, T> getContent);
        IEnumerable<IGameBoardCellViewModel<T>> Cells();
    }

    public class GameBoardViewModel<T> : NotifyPropertyChangedViewModel, IGameBoardViewModel<T>
    {
        private readonly IGameBoardService _gameBoardService;
        private readonly int _rowCount;
        private readonly int _columnCount;

        private Dictionary<GameBoardCoordinate, IGameBoardCellViewModel<T>> _gameBoard;

        public GameBoardViewModel
        (
            IGameBoardService gameBoardService,
            int rowCount,
            int columnCount
        )
        {
            _gameBoardService = gameBoardService;
            _rowCount = rowCount;
            _columnCount = columnCount;
        }

        public IGameBoardCellViewModel<T> this[GameBoardCoordinate index]
        {
            get
            {
                IGameBoardCellViewModel<T> cell;
                if (_gameBoard.TryGetValue(index, out cell))
                {
                    return cell;
                }
                else
                {
                    throw new OutOfGameBoardException(index);
                }
            }
        }

        private ICollection<ICollection<IGameBoardCellViewModel<T>>> _rowsAndColumns;
        public ICollection<ICollection<IGameBoardCellViewModel<T>>> RowsAndColumns
        {
            get { return _rowsAndColumns; }
            set { OnChange(ref _rowsAndColumns, value); }
        }

        public IGameBoardViewModel<T> ResetBoard()
        {
            return ResetBoard(c => default(T));
        }

        public IGameBoardViewModel<T> ResetBoard(Func<GameBoardCoordinate, T> getContent)
        {
            try
            {
                var rows = new List<ICollection<IGameBoardCellViewModel<T>>>(_rowCount);

                var gameBoard = new Dictionary<GameBoardCoordinate, IGameBoardCellViewModel<T>>();

                foreach (var row in _gameBoardService.GenerateCells(_rowCount, _columnCount, getContent))
                {
                    var column = new List<IGameBoardCellViewModel<T>>(_columnCount);

                    foreach (var cell in row)
	                {
                        var coordinate = cell.Coordinate;
                        
                        gameBoard[coordinate] = cell;
                        column.Add(cell);
	                }

                    rows.Add(column);
                }

                _gameBoard = gameBoard;
                RowsAndColumns = rows;
            }
            catch (Exception e)
            {
                // TODO:
            }

            return this;
        }

        public IEnumerable<IGameBoardCellViewModel<T>> Cells()
        {
            foreach (var cell in _gameBoard.Values)
            {
                yield return cell;
            }
        }

    }
}
