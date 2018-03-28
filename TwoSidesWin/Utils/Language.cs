using System.Collections.Generic;
using System.IO;

namespace TwoSides.Utils
{
    public class Localisation
    {
        public enum Language
        {
            EN,
            RU
        }

        static Language _currentLanguage = 0;
        static readonly Dictionary<Language, Dictionary<string, string>> LangItem = new Dictionary<Language, Dictionary<string, string>>();
        public static void LoadName(string fileName)
        {
            using ( StreamReader reader = new StreamReader(fileName) )
            {
                Dictionary<string , string> currentLang = new Dictionary<string , string>();
                while ( !reader.EndOfStream )
                {
                    string[] lang = reader.ReadLine()?.Split(';');
                    if ( lang?.Length >= 2 )
                        currentLang.Add(lang[0] , lang[1]);
                }

                LangItem.Add(_currentLanguage , currentLang);
            }
        }
        public static void LoadLocalisation(string[] langName)
        {
            LangItem.Clear();
            _currentLanguage = Language.EN;
            LoadName(langName[0]);
            _currentLanguage = Language.RU;
            LoadName(langName[1]);
        }
        public static void SetLanguage(Language lang)
        {
            _currentLanguage = lang;
        }
        public static string GetName(string a)
        {
            var programName = a;
            if (!LangItem[_currentLanguage].TryGetValue(programName, out a)) a = programName;
            return a;
        }

    }
}
