using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Model;

namespace Snake.ViewModel
{
    public interface ISnakeyViewModel
    {
        TimeSpan Interval { get; }
        SnakeyToken SnakeyToken { get; }

        void ChangeDirectionOfTravel(DirectionOfTravel directionOfTravel);
        DirectionOfTravel GetDirectionOfTravel();
        IEnumerable<GameBoardCoordinate> SnakeBody();
        GameBoardCoordinate AddNewSnakeSegment(DirectionOfTravel directionOftravel);
        GameBoardCoordinate RemoveSnakeSegment();
        void SpeedUpSnakey();
    }

    public class SnakeyViewModel : ISnakeyViewModel
    {
        private readonly SnakeyToken _snakeyToken;
        private readonly Queue<GameBoardCoordinate> _snakeBody;
        private readonly bool _doesSnakeySpeedUp;
        private readonly TimeSpan _initialInterval;

        private TimeSpan _interval;
        private DirectionOfTravel _directionOfTravel;
        private DirectionOfTravel? _newDirectionOfTravel;

        public SnakeyViewModel
        (
            SnakeyToken snakeyToken,
            TimeSpan interval,
            bool doesSnakeySpeedUp,
            DirectionOfTravel initialDirectionOfTravel,
            GameBoardCoordinate initial
        )
        {
            _snakeyToken = snakeyToken;
            _doesSnakeySpeedUp = doesSnakeySpeedUp;
            _interval = _initialInterval = interval;
            _directionOfTravel = initialDirectionOfTravel;

            _snakeBody = new Queue<GameBoardCoordinate>();
            _snakeBody.Enqueue(initial);

        }

        public TimeSpan Interval { get { return _interval; } }

        public SnakeyToken SnakeyToken { get { return _snakeyToken; } }

        public void ChangeDirectionOfTravel(DirectionOfTravel directionOfTravel)
        {
            if (_directionOfTravel == directionOfTravel || _directionOfTravel == GetOpposite(directionOfTravel)) return;

            _newDirectionOfTravel = directionOfTravel;
        }

        public DirectionOfTravel GetDirectionOfTravel()
        {
            _directionOfTravel = _newDirectionOfTravel ?? _directionOfTravel;
            _newDirectionOfTravel = null;

            return _directionOfTravel;
        }

        public IEnumerable<GameBoardCoordinate> SnakeBody()
        {
            foreach (var bodySegment in _snakeBody)
            {
                yield return bodySegment;
            }
        }

        public GameBoardCoordinate AddNewSnakeSegment(DirectionOfTravel directionOfTravel)
        {
            var newSnakeSegment = CalculateNewSnakeSegment(directionOfTravel);
            _snakeBody.Enqueue(newSnakeSegment);

            return newSnakeSegment;
        }

        public GameBoardCoordinate RemoveSnakeSegment()
        {
            return _snakeBody.Dequeue();
        }

        public void SpeedUpSnakey()
        {
            if (_doesSnakeySpeedUp == false) return;

            int snakeSize = _snakeBody.Count;

            // Only speed up every other time.
            if (snakeSize % 2 == 0) return;

            const int snakeSizeForMaxSpeedUp = 50;
            if (snakeSize >= snakeSizeForMaxSpeedUp) return;

            var speedUpFactor = Decimal.Divide(snakeSize, snakeSizeForMaxSpeedUp);

            var initialSpeed = _initialInterval.Ticks;
            var maxSpeed = Decimal.Divide(initialSpeed, 3);
            var newSpeed = initialSpeed - ((initialSpeed - maxSpeed) * speedUpFactor);

            _interval = TimeSpan.FromTicks((long) newSpeed);
        }

        private DirectionOfTravel GetOpposite(DirectionOfTravel directionOfTravel)
        {
            switch (directionOfTravel)
            {
                case DirectionOfTravel.Up:
                    return DirectionOfTravel.Down;
                case DirectionOfTravel.Right:
                    return DirectionOfTravel.Left;
                case DirectionOfTravel.Down:
                    return DirectionOfTravel.Up;
                case DirectionOfTravel.Left:
                    return DirectionOfTravel.Right;
                default:
                    throw new ArgumentOutOfRangeException("directionOfTravel");
            }
        }

        private GameBoardCoordinate CalculateNewSnakeSegment(DirectionOfTravel directionOfTravel)
        {
            var snakeHead = _snakeBody.Last();

            switch (directionOfTravel)
            {
                case DirectionOfTravel.Up:

                    return new GameBoardCoordinate(snakeHead.X, snakeHead.Y - 1);

                    break;
                case DirectionOfTravel.Right:

                    return new GameBoardCoordinate(snakeHead.X + 1, snakeHead.Y);

                    break;
                case DirectionOfTravel.Down:

                    return new GameBoardCoordinate(snakeHead.X, snakeHead.Y + 1);

                    break;
                case DirectionOfTravel.Left:

                    return new GameBoardCoordinate(snakeHead.X - 1, snakeHead.Y);

                    break;
                default:
                    throw new ArgumentOutOfRangeException("directionOfTravel");
            }
        }

    }
}
