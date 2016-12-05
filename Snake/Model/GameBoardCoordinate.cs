using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Model
{
    public struct GameBoardCoordinate
    {
        private readonly int _x;
        private readonly int _y;

        public GameBoardCoordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public override bool Equals(Object obj)
        {
            if (obj == null) return false;
            if ((obj is GameBoardCoordinate) == false) return false;

            var other = (GameBoardCoordinate) obj;

            return Equals(other);
        }

        public bool Equals(GameBoardCoordinate other)
        {
            if (other == null) return false;

            return (X == other.X) && (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static bool operator ==(GameBoardCoordinate first, GameBoardCoordinate second)
        {
            if (System.Object.ReferenceEquals(first, second)) return true;
            if (first == null || second == null) return false;

            return first.Equals(second);
        }

        public static bool operator !=(GameBoardCoordinate first, GameBoardCoordinate second)
        {
            return !(first == second);
        }

    }
}
