namespace Jg.wpf.core.Extensions.Types.Animations
{
    public enum DragOrientation
    {
        Horizontal,
        Vertical,
        All
    }

    public interface IRoutedEventArgs
    {
        object Source { get; }

        object OriginalSource { get; }

        object GetSourceDataContext();

        object GetOriginalSourceDataContext();
    }

    public interface IItemDroppedEventArgs : IRoutedEventArgs
    {
        /// <summary>
        /// Index of dragged item
        /// </summary>
        int PreviousIndex { get; }

        /// <summary>
        /// new index when dragging item dropped
        /// </summary>
        int CurrentIndex { get; }

        object DataContext { get; set; }
    }
}
