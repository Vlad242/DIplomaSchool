using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DiplomaSchool.Translator
{
    class Translator
    {
        public static IEnumerable<string> Languages
        {
            get
            {
                Translator.EnsureInitialized();
                return Translator._languageModeMap.Keys.OrderBy(p => p);
            }
        }

        public TimeSpan TranslationTime
        {
            get;
            private set;
        }

        public string TranslationSpeechUrl
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }

        public string Translate
            (string sourceText,
             string sourceLanguage,
             string targetLanguage)
        {
            // Initialize
            this.Error = null;
            this.TranslationSpeechUrl = null;
            this.TranslationTime = TimeSpan.Zero;
            DateTime tmStart = DateTime.Now;
            string translation = string.Empty;

            try
            {
                // Download translation
                string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                            Translator.LanguageEnumToIdentifier(sourceLanguage),
                                            Translator.LanguageEnumToIdentifier(targetLanguage),
                                            HttpUtility.UrlEncode(sourceText));
                string outputFile = Path.GetTempFileName();
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                    wc.DownloadFile(url, outputFile);
                }

                // Get translated text
                if (File.Exists(outputFile))
                {

                    // Get phrase collection
                    string text = File.ReadAllText(outputFile);
                    int index = text.IndexOf(string.Format(",,\"{0}\"", Translator.LanguageEnumToIdentifier(sourceLanguage)));
                    if (index == -1)
                    {
                        // Translation of single word
                        int startQuote = text.IndexOf('\"');
                        if (startQuote != -1)
                        {
                            int endQuote = text.IndexOf('\"', startQuote + 1);
                            if (endQuote != -1)
                            {
                                translation = text.Substring(startQuote + 1, endQuote - startQuote - 1);
                            }
                        }
                    }
                    else
                    {
                        // Translation of phrase
                        text = text.Substring(0, index);
                        text = text.Replace("],[", ",");
                        text = text.Replace("]", string.Empty);
                        text = text.Replace("[", string.Empty);
                        text = text.Replace("\",\"", "\"");

                        // Get translated phrases
                        string[] phrases = text.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; (i < phrases.Count()); i += 2)
                        {
                            string translatedPhrase = phrases[i];
                            if (translatedPhrase.StartsWith(",,"))
                            {
                                i--;
                                continue;
                            }
                            translation += translatedPhrase + "  ";
                        }
                    }

                    // Fix up translation
                    translation = translation.Trim();
                    translation = translation.Replace(" ?", "?");
                    translation = translation.Replace(" !", "!");
                    translation = translation.Replace(" ,", ",");
                    translation = translation.Replace(" .", ".");
                    translation = translation.Replace(" ;", ";");

                    // And translation speech URL
                    this.TranslationSpeechUrl = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx",
                                                               HttpUtility.UrlEncode(translation), Translator.LanguageEnumToIdentifier(targetLanguage), translation.Length);
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }

            // Return result
            this.TranslationTime = DateTime.Now - tmStart;
            return translation;
        }

       private static string LanguageEnumToIdentifier
            (string language)
        {
            string mode = string.Empty;
            Translator.EnsureInitialized();
            Translator._languageModeMap.TryGetValue(language, out mode);
            return mode;
        }

        private static void EnsureInitialized()
        {
            if (Translator._languageModeMap == null)
            {
                Translator._languageModeMap = new Dictionary<string, string>
                {
                    { "Afrikaans", "af" },
                    { "Albanian", "sq" },
                    { "Arabic", "ar" },
                    { "Armenian", "hy" },
                    { "Azerbaijani", "az" },
                    { "Basque", "eu" },
                    { "Belarusian", "be" },
                    { "Bengali", "bn" },
                    { "Bulgarian", "bg" },
                    { "Catalan", "ca" },
                    { "Chinese", "zh-CN" },
                    { "Croatian", "hr" },
                    { "Czech", "cs" },
                    { "Danish", "da" },
                    { "Dutch", "nl" },
                    { "English", "en" },
                    { "Esperanto", "eo" },
                    { "Estonian", "et" },
                    { "Filipino", "tl" },
                    { "Finnish", "fi" },
                    { "French", "fr" },
                    { "Galician", "gl" },
                    { "German", "de" },
                    { "Georgian", "ka" },
                    { "Greek", "el" },
                    { "Haitian Creole", "ht" },
                    { "Hebrew", "iw" },
                    { "Hindi", "hi" },
                    { "Hungarian", "hu" },
                    { "Icelandic", "is" },
                    { "Indonesian", "id" },
                    { "Irish", "ga" },
                    { "Italian", "it" },
                    { "Japanese", "ja" },
                    { "Korean", "ko" },
                    { "Lao", "lo" },
                    { "Latin", "la" },
                    { "Latvian", "lv" },
                    { "Lithuanian", "lt" },
                    { "Macedonian", "mk" },
                    { "Malay", "ms" },
                    { "Maltese", "mt" },
                    { "Norwegian", "no" },
                    { "Persian", "fa" },
                    { "Polish", "pl" },
                    { "Portuguese", "pt" },
                    { "Romanian", "ro" },
                    { "Russian", "ru" },
                    { "Serbian", "sr" },
                    { "Slovak", "sk" },
                    { "Slovenian", "sl" },
                    { "Spanish", "es" },
                    { "Swahili", "sw" },
                    { "Swedish", "sv" },
                    { "Tamil", "ta" },
                    { "Telugu", "te" },
                    { "Thai", "th" },
                    { "Turkish", "tr" },
                    { "Ukrainian", "uk" },
                    { "Urdu", "ur" },
                    { "Vietnamese", "vi" },
                    { "Welsh", "cy" },
                    { "Yiddish", "yi" }
                };
            }
        }

        private static Dictionary<string, string> _languageModeMap;
    }
}
