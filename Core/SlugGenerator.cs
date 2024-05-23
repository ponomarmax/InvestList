using Slugify;

namespace Core
{
    public static class SlugGenerator
    {
        private static readonly SlugHelper _slugGenerator;
        
        private static Dictionary<string,string> mapReplacement = new()
        {
            // Uppercase Cyrillic letters to lowercase Latin letters
            { "А", "a" }, { "Б", "b" }, { "В", "v" }, { "Г", "g" }, { "Д", "d" },
            { "Е", "e" }, { "Ё", "e" }, { "Ж", "zh" }, { "З", "z" }, { "И", "i" },
            { "Й", "y" }, { "К", "k" }, { "Л", "l" }, { "М", "m" }, { "Н", "n" },
            { "О", "o" }, { "П", "p" }, { "Р", "r" }, { "С", "s" }, { "Т", "t" },
            { "У", "u" }, { "Ф", "f" }, { "Х", "kh" }, { "Ц", "ts" }, { "Ч", "ch" },
            { "Ш", "sh" }, { "Щ", "shch" }, { "Ъ", "" }, { "Ы", "y" }, { "Ь", "" },
            { "Э", "e" }, { "Ю", "yu" }, { "Я", "ya" },

            // Lowercase Cyrillic letters to lowercase Latin letters
            { "а", "a" }, { "б", "b" }, { "в", "v" }, { "г", "g" }, { "д", "d" },
            { "е", "e" }, { "ё", "e" }, { "ж", "zh" }, { "з", "z" }, { "и", "i" },
            { "й", "y" }, { "к", "k" }, { "л", "l" }, { "м", "m" }, { "н", "n" },
            { "о", "o" }, { "п", "p" }, { "р", "r" }, { "с", "s" }, { "т", "t" },
            { "у", "u" }, { "ф", "f" }, { "х", "kh" }, { "ц", "ts" }, { "ч", "ch" },
            { "ш", "sh" }, { "щ", "shch" }, { "ъ", "" }, { "ы", "y" }, { "ь", "" },
            { "э", "e" }, { "ю", "yu" }, { "я", "ya" }, {" ", "-"}
        };

        static SlugGenerator()
        {
            var config = new SlugHelperConfiguration();
            config.StringReplacements = mapReplacement;
            _slugGenerator = new SlugHelper(config);
        }

        public static string Get(string title) => _slugGenerator.GenerateSlug(title);
    }
}