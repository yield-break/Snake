using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Snake.Model;

namespace Snake.Converters
{
    public class GameModeEqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var enumerator = CastToGameMode(values).GetEnumerator();

            // If there are no items in the collection, return false.
            if (enumerator.MoveNext() == false) return false;

            var firstGameMode = enumerator.Current;

            bool processing = true;
            while (processing && enumerator.MoveNext())
            {
                var currentGameMode = enumerator.Current;

                if (currentGameMode != firstGameMode)
                {
                    // Bug right out if not equal.
                    return false;
                }
            }

            // If there was only one item in the collection, or all items were equal, return true.
            return true;
        }

        private IEnumerable<GameMode> CastToGameMode(object[] values)
        {
            foreach(var value in values.Where(v => v != null))
            {
                GameMode gameMode;
                if (Enum.TryParse(value.ToString(), out gameMode)
                    && Enum.IsDefined(typeof(GameMode), gameMode))
                {
                    yield return gameMode;
                }
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
