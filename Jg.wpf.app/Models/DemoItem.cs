namespace Jg.wpf.app.Models
{
    public class DemoItem
    {
        public string Name { get; }
        public string ContentType { get; }
        public object DataContext { get; }

        public DemoItem(string name, string contentType, object dataContext = null)
        {
            Name = name;
            ContentType = contentType;
            DataContext = dataContext;
        }
      
    }

}
