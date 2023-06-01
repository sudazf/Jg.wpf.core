using Jg.wpf.app.Models;
using System.Collections.Generic;

namespace Jg.wpf.app.ViewModels
{
    public class FastDataGridViewModel
    {
        public List<TestItem> Items { get; }
        public TestItem SelectedTestItem { get; set; }
        public FastDataGridViewModel()
        {
            Items = new List<TestItem>()
            {
                new TestItem("c1", "c2", "c3"),
                new TestItem("c4", "c5", "c6"),
                new TestItem("c7", "c8", "c9"),
            };
        }
    }
}
