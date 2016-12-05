using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Model
{
    public class OutOfGameBoardException : Exception
    {
        public OutOfGameBoardException(GameBoardCoordinate invalidCoordinate)
        {
            InvalidCoordinate = invalidCoordinate;
        }

        public GameBoardCoordinate InvalidCoordinate { get; private set; }

    }
}
