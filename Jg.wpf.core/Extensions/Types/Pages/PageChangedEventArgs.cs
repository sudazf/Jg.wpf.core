namespace Jg.wpf.core.Extensions.Types.Pages
{
    public class PageChangedEventArgs
    {
        public int Page { get; }

        public PageChangedEventArgs(int page)
        {
            Page = page;
        }
    }
}
