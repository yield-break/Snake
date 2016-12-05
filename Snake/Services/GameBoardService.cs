using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.ViewModel;
using Snake.Model;

namespace Snake.Services
{
    public interface IGameBoardService
    {
        IEnumerable<IEnumerable<IGameBoardCellViewModel<T>>> GenerateCells<T>(int rowCount, int columnCount);
        IEnumerable<IEnumerable<IGameBoardCellViewModel<T>>> GenerateCells<T>(int rowCount, int columnCount, Func<GameBoardCoordinate, T> getCellContent);
    }

    [Export(typeof(IGameBoardService))]
    public class GameBoardService : IGameBoardService
    {
        public IEnumerable<IEnumerable<IGameBoardCellViewModel<T>>> GenerateCells<T>(int rowCount, int columnCount)
        {
            return GenerateCells(rowCount, columnCount, c => default(T));
        }

        public IEnumerable<IEnumerable<IGameBoardCellViewModel<T>>> GenerateCells<T>(int rowCount, int columnCount, Func<GameBoardCoordinate, T> getCellContent)
        {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                yield return GenerateRow<T>(rowIndex, columnCount, getCellContent);
            }
        }

        private IEnumerable<IGameBoardCellViewModel<T>> GenerateRow<T>(int rowIndex, int columnCount, Func<GameBoardCoordinate, T> getCellContent)
        {
            for (int colIndex = 0; colIndex < columnCount; colIndex++)
            {
                var coordinate = new GameBoardCoordinate(colIndex, rowIndex);
                T content = getCellContent(coordinate);

                yield return new GameBoardCellViewModel<T>(coordinate, content);
            }
        }

    }
}
