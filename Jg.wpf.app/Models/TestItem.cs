namespace Jg.wpf.app.Models
{
    public class TestItem
    {
        public string Column1 { get; }
        public string Column2 { get; }
        public string Column3 { get; }

        public TestItem(string c1, string c2, string c3)
        {
            Column1 = c1;
            Column2 = c2;
            Column3 = c3;
        }
    }
}
