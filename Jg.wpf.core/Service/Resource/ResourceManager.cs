using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jg.wpf.core.Service.Resource
{
    public static class ResourceManager
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _sections = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, Dictionary<string, string>> Sections => _sections;

        public static void Initialize(string path)
        {
            _sections.Clear();
            if (Directory.Exists(path))
            {
                LoadDirectory(path);
            }
        }
        public static string GetValue(string section, string key, string defaultValue)
        {
            var result = defaultValue;
            if (_sections.ContainsKey(section))
            {
                var keyValue = _sections[section];
                if (keyValue.ContainsKey(key))
                {
                    result = keyValue[key];
                }
            }
            return result;
        }

        public static T GetValue<T>(string section, string key, T defaultValue)
        {
            if (_sections.ContainsKey(section))
            {
                var keyValue = _sections[section];
                if (keyValue.ContainsKey(key))
                {
                    Type t = typeof(T);
                    var value = keyValue[key];
                    if (t == typeof(string))
                    {
                        return (T)((object)value);
                    }

                    if (t == typeof(bool) && (value == "0" || value == "1"))
                    {
                        return (T)((object)(value != "0"));
                    }

                    try
                    {
                        if (t.IsEnum)
                        {
                            return (T)Enum.Parse(t, value, true);
                        }

                        return (T)Convert.ChangeType(value, t);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }


        public static Dictionary<string, string> GetSectionValues(string section)
        {
            if (_sections.ContainsKey(section))
            {
                return _sections[section];
            }
            return null;
        }

        public static string GetSection(string key)
        {
            return _sections.FirstOrDefault(section => section.Value.ContainsKey(key)).Key;
        }


        private static void LoadDirectory(string filePath)
        {
            DirectoryInfo folder = new DirectoryInfo(filePath);
            foreach (var file in folder.GetFiles())
            {
                if (file.Extension == ".ini")
                {
                    LoadFile(file.FullName);
                }
            }

            foreach (var subFolder in folder.GetDirectories())
            {
                LoadDirectory(subFolder.FullName);
            }
        }
        private static void LoadFile(string filePath)
        {
            try
            {
                string line = "";
                string section = "";
                Dictionary<string, string> subItems = new Dictionary<string, string>();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var lineData = line.Trim();
                        if (lineData.StartsWith("["))
                        {
                            if (section != "")
                            {
                                _sections[section] = subItems;
                                section = "";
                                subItems = new Dictionary<string, string>();
                            }
                            section = lineData.Trim('[', ']');
                        }
                        else if (lineData.StartsWith("#") || lineData == "")
                        {
                            continue;
                        }
                        else if (lineData != "")
                        {
                            var array = lineData.Split('=');
                            if (array.Length == 2)
                            {
                                subItems[array[0].Trim()] = array[1].Trim();
                            }
                            else if (array.Length == 1)
                            {
                                var lastItem = subItems.Last();
                                subItems[lastItem.Key] = string.Join("", lastItem.Value.Trim(), array[0].Trim());
                            }
                        }
                    }

                    if (section != "" && !_sections.ContainsKey(section))
                    {
                        _sections[section] = subItems;
                        section = "";
                        subItems = new Dictionary<string, string>();
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
