using System.Collections.Generic;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class ScrollViewerAnimationViewModel : ViewModelBase
    {
        public IList<string> Source { get; }
        public ScrollViewerAnimationViewModel()
        {
            Source = new List<string>()
            {
                "AAAAAAAA",
                "BBBBBBBB",
                "CCCCCCCC",
                "DDDDDDDD",
                "EEEEEEEE",
                "FFFFFFFF",
                "GGGGGGGG",
                "HHHHHHHH",
                "IIIIIIII",
                "JJJJJJJJ",
                "KKKKKKKK",
                "LLLLLLLL",
                "MMMMMMMM",
            };
        }
    }
}
