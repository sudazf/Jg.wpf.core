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
                new DemoItem("Draggable Item", "DraggableDemo", new DraggableDemoViewModel()),
                new DemoItem("Custom TextBox", "TextBoxDemo", new TextBoxDemoViewModel()),
                new DemoItem("ListView","ListViewDemo",new ListViewViewModel()),
                new DemoItem("Refresh Default","RefreshPerFrameDemo1",new RefreshPerFrameViewModel1()),
                new DemoItem("Refresh Queue","RefreshPerFrameDemo2",new RefreshPerFrameViewModel2()),
                new DemoItem("ScrollViewer Animation","ScrollViewerAnimationDemo",new ScrollViewerAnimationViewModel()),
                new DemoItem("Autocomplete","AutocompleteDemo",new AutocompleteViewModel()),
                
            };
            DemoItems = _defaultItems;

        }
    }
}
