using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Jg.wpf.core.Log;
using Jg.wpf.core.Service.Resource;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;

namespace Jg.wpf.core.Service.ThemeService
{
    internal class ThemeServiceImp : IThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public ThemeServiceImp()
        {
            
        }

        public void ApplyBase(bool isDark)
        {
            var theme = _paletteHelper.GetTheme();
            var baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);

            var color = isDark ? "Dark" : "Light";
            var resourceList = ResourceManager.GetValue("ThemesSetting", "ResourceList", $"Jg.wpf.controls | Themes\\Colors\\JgColor.{color}.xaml");
            var resourceArray = resourceList.Split(',');

            foreach (var resourceItem in resourceArray)
            {
                var resource = resourceItem.Split('|');
                AddResources("/{0};component/{1}", resource[0].Trim(), resource[1].Trim());
            }
        }

        public void ChangePrimaryColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }

        public void ChangeSecondaryColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(color.Lighten());
            theme.SecondaryMid = new ColorPair(color);
            theme.SecondaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }


        private void AddResources(string pathFormat, string pattern1, string pattern2)
        {
            if (string.IsNullOrEmpty(pathFormat))
            {
                return;
            }
            AddResourceDictionary(Application.Current.Resources.MergedDictionaries, string.Format(pathFormat, pattern1, pattern2));
        }

        private static void AddResourceDictionary(Collection<ResourceDictionary> mergedDictionaries, string resourcePath, UriKind uriKind = UriKind.Relative)
        {
            if (mergedDictionaries != null && !string.IsNullOrEmpty(resourcePath))
            {
                try
                {
                    ResourceDictionary resourceDictionary = new ResourceDictionary
                    {
                        Source = new Uri(resourcePath, uriKind)
                    };
                    if (resourceDictionary.MergedDictionaries.Count > 0)
                    {
                        foreach (var resourceDictionaryMergedDictionary in resourceDictionary.MergedDictionaries)
                        {
                            AddResourceDictionary(mergedDictionaries, resourceDictionaryMergedDictionary.Source.LocalPath);
                        }
                    }
         
                    mergedDictionaries.Add(resourceDictionary);
                }
                catch (IOException e)
                {
                    Logger.WriteLineInfo($"ResourceDictionary:{resourcePath} not found!, ex{e}");
                }
                catch (Exception ex)
                {
                    Logger.WriteLineError("Failed load ResourceDictionary from:{0} ,ex:{1}", resourcePath, ex);
                }
            }
        }

    }
}
