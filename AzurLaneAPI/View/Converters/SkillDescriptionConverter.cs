using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AzurLaneAPI.View.Converters
{
    class SkillDescriptionConverter : IMultiValueConverter
    {
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string description = values[1].ToString();
            ushort lv = (ushort)values[0];
            string pattern = @"\d{1,}\.?\d*.?\s\(([^\s]{3})?\d{1,}\.?\d*.?\)";
            MatchCollection matches = Regex.Matches(description, pattern);
            if (matches.Count > 0)
            {
                string subPattern = @"(\d{1,}\.?\d*)";
                double minVal, maxVal, diffVal, newVal;
                string newString = "", subString = "";
                int indexOffset = 0;
                var map = new Dictionary<string, string>();
                //matches from full string vb.: hello 12% (40%)
                foreach (Match match in matches)
                {

                    //check if no multiple parts have been replaced, see bottom
                    if (map.ContainsKey(match.Value))
                    {
                        List<KeyValuePair<string, string>> pair = map.ToList();
                        KeyValuePair<string, string> idx = pair.Find(x => x.Key == match.Value);
                        indexOffset += (match.Value.Length - idx.Value.Length);
                        continue;
                    }

                    //get the substring
                    subString = description.Substring(match.Index - indexOffset, match.Length);
                    //get the match result
                    MatchCollection subMatches = Regex.Matches(subString, subPattern);
                    //matches from substring vb.: 12% (40%) -> 12 and 40
                    int indx = 0;
                    while (subMatches[indx].Length == 0)
                    {
                        ++indx;
                    }

                    minVal = double.Parse(subString.Substring(subMatches[indx].Index, subMatches[indx].Length));

                    do
                    {
                        ++indx;
                    } while (subMatches[indx].Length == 0);
                    maxVal = double.Parse(subString.Substring(subMatches[indx].Index, subMatches[indx].Length));

                    //get difference per lv
                    diffVal = maxVal - minVal;
                    diffVal /= 9;

                    //get new value
                    newVal = minVal + diffVal * (lv - 1);
                    newVal = Math.Round(newVal, 1);
                    newString = newVal.ToString();
                    if (subString.Contains("%"))
                        newString += "%";

                    //replace, replaces everything that matches!!!
                    description = description.Replace(subString, newString);

                    //keep track of the replaced substrings
                    map.Add(subString, newString);

                    indexOffset += (subString.Length - newString.Length);
                }
            }
            return description;
        }
    }
}
