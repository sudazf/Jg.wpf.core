# Jg.wpf.core
Jige's MVVM core for WPF

1. Way to use Jg.wpf.controls style
```xaml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/Jg.wpf.controls;Component/Themes/Generic.xaml" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```    

2. Way to get one of Jg.wpf.core service
```cs
var fileService = ServiceManager.GetService<IFileService>();
```


