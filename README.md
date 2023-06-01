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

2. Way to get service of Jg.wpf.core

    init service
    ```cs
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceManager.Init(Current.Dispatcher);
        }
    }
    ```
    get service, e.g.
    ```cs
    var fileService = ServiceManager.GetService<IFileService>();
    ```

3. some demos:
![j1](https://github.com/sudazf/Jg.wpf.core/assets/3366672/c5f3c15f-48ac-4ff1-9d33-736eeeb84c7d)
![j2](https://github.com/sudazf/Jg.wpf.core/assets/3366672/1858608d-c49d-4213-8709-abe6cbbcc345)
![j3](https://github.com/sudazf/Jg.wpf.core/assets/3366672/203001d7-cb3a-497a-b17e-11484247977e)
