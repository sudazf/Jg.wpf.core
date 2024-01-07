using System.Collections.Generic;
using Jg.wpf.app.Models;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class LocalizationViewModel : ViewModelBase
    {
        public List<LocalizationDemoItem> DemoSource { get; }

        public LocalizationViewModel()
        {
            DemoSource = new List<LocalizationDemoItem>();
            DemoSource.Add(new LocalizationDemoItem("type1"));
            DemoSource.Add(new LocalizationDemoItem("type2"));
            DemoSource.Add(new LocalizationDemoItem("type3"));
        }
    }
}
