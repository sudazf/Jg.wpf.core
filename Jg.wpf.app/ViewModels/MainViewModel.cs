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
                    RaisePropertyChanged(()=> DemoItems);
                }
            }
        }

        public MainViewModel()
        {
            _defaultItems = new List<DemoItem>()
            {
                new DemoItem("Draggable ItemsControl", "DraggableDemo", new DraggableDemoViewModel()),

            };
            DemoItems = _defaultItems;

        }
    }
}
