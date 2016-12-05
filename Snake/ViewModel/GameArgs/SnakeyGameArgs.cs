using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Model;

namespace Snake.ViewModel.GameArgs
{
    public class SnakeyGameArgs
    {
        public SnakeyGameArgs(GameMode gameMode, SnakePlayerArgs playerOneArgs, SnakePlayerArgs playerTwoArgs)
        {
            GameMode = gameMode;
            PlayerOneArgs = playerOneArgs;
            PlayerTwoArgs = playerTwoArgs;
        }

        public GameMode GameMode { get; set; }
        public SnakePlayerArgs PlayerOneArgs { get; set; }
        public SnakePlayerArgs PlayerTwoArgs { get; set; }

    }
}
