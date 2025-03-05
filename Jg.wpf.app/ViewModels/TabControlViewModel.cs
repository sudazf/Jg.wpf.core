using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Profilers;
using Jg.wpf.core.Service;
using Jg.wpf.core.Service.ThreadService;

namespace Jg.wpf.app.ViewModels
{
    public class TabControlViewModel : ViewModelBase
    {
        private readonly TaskManager _taskManager;
        private readonly IDispatcher _dispatcher;

        private TabTestItem _selectedItem;
        public ObservableCollection<TabTestItem> TestItems { get; }

        public TabTestItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public JCommand AddCommand { get;  }

        public TabControlViewModel()
        {
            _taskManager = new TaskManager("TaskManager");

            _dispatcher = ServiceManager.GetService<IDispatcher>();

            var subItems1 = new List<SubItem>(){new SubItem("Sub1"), new SubItem("Sub2"), };
            var testItem1 = new TabTestItem("Name1", subItems1) {};

            var subItems2 = new List<SubItem>() { new SubItem("Sub3"), new SubItem("Sub4"), };
            var testItem2 = new TabTestItem("Name2", subItems2) {};

            TestItems = new ObservableCollection<TabTestItem>()
            {
                testItem1,testItem2
            };

            AddCommand = new JCommand("AddCommand", OnAdd);
        }

        private void OnAdd(object obj)
        {
            var subItems2 = new List<SubItem>() { new SubItem("Sub3"), new SubItem("Sub4"), };
            var testItem2 = new TabTestItem("Name2", subItems2) { };
            TestItems.Add(testItem2);
        }

        public void Init()
        {
            _taskManager.StartNewTaskProxy("Task", () =>
            {
                _dispatcher.Invoke(() =>
                {
                    if (SelectedItem == null)
                    {
                        return;
                    }
                    var subItems1 = new List<SubItem>() { new SubItem("Sub1") };
                    var subItems2 = new List<SubItem>() { new SubItem("Sub3"), new SubItem("Sub4"), };
                    if (SelectedItem.Name == "Name1")
                    {
                        SelectedItem.SubItems = subItems1;
                        SelectedItem.SelectedItem = subItems1[0];
                    }
                    else
                    {
                        SelectedItem.SubItems = subItems2;
                        SelectedItem.SelectedItem = subItems2.Last();
                    }
                });
            });
        }
    }

    public class TabTestItem : ViewModelBase
    {
        private SubItem _selectedItem;
        private List<SubItem> _subItems;
        public string Name { get; }

        public List<SubItem> SubItems
        {
            get => _subItems;
            set
            {
                if (Equals(value, _subItems)) return;
                _subItems = value;
                RaisePropertyChanged(nameof(SubItems));
            }
        }

        public SubItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public TabTestItem(string name, List<SubItem> subItems)
        {
            Name = name;
            SubItems = subItems;
        }
    }

    public class SubItem
    {
        public string SubName { get; }
        public SubItem(string subName)
        {
            SubName = subName;
        }
    }

}
