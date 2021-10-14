using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Jg.wpf.core.Service.Resource;

namespace Jg.wpf.core.Utility
{
    public static class TranslateHelper
    {
        private static readonly Dictionary<string, string> TranslationDictionary = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> DefaultNlS;
        private static readonly Dictionary<string, string> LanguageToCulture = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> CultureToLanguage = new Dictionary<string, string>();

        public static string CurrentLanguage { get; private set; }

        static TranslateHelper()
        {
            DefaultNlS = new Dictionary<string, string>
            {
                {"Chinese", "zh-CN"},
                {"English", "en-US"},
                {"German", "de-DE"},
                {"Greek", "el-GR"},
                {"Malay", "ms-MY"},
                {"Portuguese", "pt-PT"},
                {"Romanian", "ro-RO"},
                {"Spanish", "es-ES"},
                {"Swedish", "sv-SE"},
                {"Norwegian", "nb-NO"},
                {"Danish", "da-DK"},
                {"Finnish", "fi-FI"},
                {"French", "fr-FR"},
                {"Polish", "pl-PL"},
                {"Russian", "ru-RU"},
                {"Uighur", "ug-CN"},
            };
        }

        public static void Initialize(string languagesFolder)
        {
            if (!Directory.Exists(languagesFolder))
            {
                return;
            }
            //initial nlsMapping
            Dictionary<string, string> nlsMapping = new Dictionary<string, string>(DefaultNlS);

            foreach (KeyValuePair<string, string> keyValuePair in nlsMapping)
            {
                LanguageToCulture[keyValuePair.Key.ToLower()] = keyValuePair.Value;
                CultureToLanguage[keyValuePair.Value.ToLower()] = keyValuePair.Key;
            }

            if (string.IsNullOrEmpty(CurrentLanguage))
            {
                var languageName = ResourceManager.GetValue("LanguageSetting", "Language", "English");
                string localeFileFullName = Path.Combine(languagesFolder, $@"{languageName}.xml");
                if (!File.Exists(localeFileFullName))
                {
                    languageName = "English";
                    localeFileFullName = Path.Combine(languagesFolder, $@"{languageName}.xml");
                }
                CurrentLanguage = languageName;

                string defaultFileFullName = string.Empty;
                if (CurrentLanguage != "English")
                {
                    defaultFileFullName = Path.Combine(languagesFolder, @"English.xml");
                }
                LoadTranslateFile(TranslationDictionary, defaultFileFullName, localeFileFullName);
            }
        }

        public static void LoadTranslateFile(Dictionary<string, string> translation, params string[] files)
        {
            if (files != null && files.Length > 0)
            {
                foreach (var fileName in files)
                {
                    //Load current language and overwrite the language dictionary.
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            var document = new XmlDocument();
                            document.Load(fileName);
                            if (document.DocumentElement != null)
                            {
                                XmlNodeList nodes = document.DocumentElement.SelectNodes("./LocaleItem");
                                if (nodes != null)
                                {
                                    foreach (XmlNode node in nodes)
                                    {
                                        if (node.Attributes != null)
                                        {
                                            string key = node.Attributes["key"].Value;
                                            string value = node.InnerText;
                                            if (!string.IsNullOrEmpty(value) && value.Contains("\\r\\n"))
                                            {
                                                value = value.Replace("\\r\\n", Environment.NewLine);
                                            }
                                            translation[key] = value;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
        }

        public static string Translate(string key, bool ignoreWhiteSpace = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            if (!TranslationDictionary.TryGetValue(key, out var temp))
            {
                temp = string.Empty;
            }

            if (string.IsNullOrEmpty(temp) && ignoreWhiteSpace)
            {
                temp = key;
            }
            return temp;
        }
    }
}
