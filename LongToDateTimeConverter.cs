using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp
{
    // Cette classe implémente l'interface IValueConverter pour convertir des long en DateTime.
    public class LongToDateTimeConverter : IValueConverter
    {
        // Date de départ Unix : 1er janvier 1970 à minuit (heure UTC).
        DateTime _time = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Convertit un long en DateTime.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convertit la valeur en long.
            long dateTime = (long)value;

            // Ajoute le nombre de secondes représenté par la valeur à la date de départ Unix.
            DateTime result = _time.AddSeconds(dateTime);

            // Retourne la date et l'heure au format texte, suivies de l'heure UTC.
            return $"{result.ToString()} UTC";
        }

        // Convertit un DateTime en long (non utilisé dans cette application).
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lève une exception car la conversion en arrière n'est pas prise en charge.
            throw new NotImplementedException();
        }
    }
}
