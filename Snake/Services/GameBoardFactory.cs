using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.ViewModel;

namespace Snake.Services
{
    public interface IGameBoardFactory
    {
        IGameBoardViewModel<T> CreateGameBoard<T>(int rowCount, int columnCount);
    }

    [Export(typeof(IGameBoardFactory))]
    public class GameBoardFactory : IGameBoardFactory
    {
        private readonly IGameBoardService _gameBoardService;

        [ImportingConstructor]
        public GameBoardFactory(IGameBoardService gameBoardService)
        {
            _gameBoardService = gameBoardService;
        }

        public IGameBoardViewModel<T> CreateGameBoard<T>(int rowCount, int columnCount)
        {
            return new GameBoardViewModel<T>(_gameBoardService, rowCount, columnCount);
        }

    }
}
