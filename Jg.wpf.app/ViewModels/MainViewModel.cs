using System;
using System.Collections.Generic;
using System.Linq;
using Jg.wpf.app.Controls;
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
                if (value != null)
                {
                    _selectedDemo = value;
                    RaisePropertyChanged(() => SelectedDemo);
                    OnSelectDemo?.Invoke(this, _selectedDemo);
                }
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
                new DemoItem("1. Navigation", "NavigationRailDemo", null),
                new DemoItem("2. TextBox", "TextBoxDemo", new TextBoxDemoViewModel()),
                new DemoItem("3. Autocomplete","AutocompleteDemo",new AutocompleteViewModel()),
                new DemoItem("4. ListBox with Select All","SelectAllListDemo", new SelectAllListViewModel()),
                new DemoItem("5. TabControl Animation","TabControlDemo", null),
                new DemoItem("6. Dragging animation", "DraggableDemo", new DraggableDemoViewModel()),
                new DemoItem("7. ScrollViewer Animation","ScrollViewerAnimationDemo",new ScrollViewerAnimationViewModel()),
                new DemoItem("8. Performance","RefreshPerFrameDemo1",new RefreshPerFrameViewModel1()),
                new DemoItem("9. Task Scheduler","TaskSchedulerDemo",new TaskSchedulerViewModel()),
                new DemoItem("10. File/Folder Dialog","FileFolderDemo",new FileFolderViewModel()),
                new DemoItem("11. Fast DataGrid","FastDataGridDemo", new FastDataGridViewModel()),
                
            };
            DemoItems = _defaultItems;

        }
    }
}
