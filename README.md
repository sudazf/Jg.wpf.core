# Jg.wpf.core
Jige's MVVM core for WPF

1. To use Jg.wpf.controls style
    ```xaml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Jg.wpf.controls;Component/Themes/Generic.xaml" />

            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```    

2. To get service of Jg.wpf.core

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

3. Some demos:
   1) Navigator
    ![j1](https://github.com/sudazf/Jg.wpf.core/assets/3366672/c5f3c15f-48ac-4ff1-9d33-736eeeb84c7d)

   2) TextBox
    ![j2](https://github.com/sudazf/Jg.wpf.core/assets/3366672/1858608d-c49d-4213-8709-abe6cbbcc345)

   3) Animation TabControl
    ![j3](https://github.com/sudazf/Jg.wpf.core/assets/3366672/203001d7-cb3a-497a-b17e-11484247977e)
    
   4) Task Scheduler  
    ![j4](https://github.com/sudazf/Jg.wpf.core/assets/3366672/5d4ca92b-51ec-46af-83ec-bdf8a98cd138)




