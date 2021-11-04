using System;
using System.Collections.Generic;
using System.Linq;
using Jg.wpf.app.Models;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _searchKeyword;
        private readonly List<DemoItem> _defaultItems;
        private DemoItem _selectedDemo;

        public event EventHandler<DemoItem> OnSelectDemo;

        public List<DemoItem> DemoItems { get; private set; }


        public DemoItem SelectedDemo
        {
            get => _selectedDemo;
            set
            {
                _selectedDemo = value;
                RaisePropertyChanged(()=> SelectedDemo);

                OnSelectDemo?.Invoke(this, _selectedDemo);
            }
        }

        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                if (!string.IsNullOrEmpty(_searchKeyword))
                {
                    DemoItems = _defaultItems.Where(i => i.Name.ToLower().Contains(_searchKeyword.ToLower())).ToList();
                }
                else
                {
                    DemoItems = _defaultItems;
                }
                RaisePropertyChanged(() => DemoItems);
            }
        }

        public MainViewModel()
        {
            _defaultItems = new List<DemoItem>()
            {
                new DemoItem("Navigation", "NavigationRailDemo", null),
                new DemoItem("Draggable", "DraggableDemo", new DraggableDemoViewModel()),
                new DemoItem("DisplayChart", "DisplayChartDemo", null),
                new DemoItem("CustomTextBox", "TextBoxDemo", new TextBoxDemoViewModel()),

            };
            DemoItems = _defaultItems;

        }
    }
}
