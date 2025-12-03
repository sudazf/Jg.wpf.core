using System;
using System.Collections.Generic;
using Jg.wpf.core.Extensions.Types;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class MultiSelectComboBoxViewModel : ViewModelBase
    {
        private MyContainer _selectedContainer;

        public List<MySelectableItem> TestItems { get; }
        public List<MyContainer> Containers { get; }
        public MyContainer SelectContainer
        {
            get => _selectedContainer;
            set
            {
                _selectedContainer = value;
                RaisePropertyChanged(() => SelectContainer);
            }
        }

        public bool SelectedAll { get; set; }

        public MultiSelectComboBoxViewModel()
        {
            TestItems = new List<MySelectableItem>()
            {
                new MySelectableItem(false, "Tom"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
                new MySelectableItem(false, "Jerry"),
            };

            Containers = new List<MyContainer>()
            {
                new MyContainer("Category1", new List<MySelectableItem>()
                {
                    new MySelectableItem(true, "Jack"),
                    new MySelectableItem(true, "Hellen"),
                    new MySelectableItem(false, "sunny"),
                }),
                new MyContainer("Category2", new List<MySelectableItem>()
                {
                    new MySelectableItem(true, "pony"),
                    new MySelectableItem(false, "jimi"),
                    new MySelectableItem(false, "lisa"),
                }),
            };

            _selectedContainer = Containers[0];
        }
    }

    public class MySelectableItem : ISelectable
    {
        public event EventHandler OnSelectedChanged;

        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public MySelectableItem(bool isSelected, string name)
        {
            IsSelected = isSelected;
            Name = name;
        }
    }

    public class MyContainer
    {
        public string Category { get; }
        public List<MySelectableItem> Items { get; }

        public MyContainer(string category, List<MySelectableItem> items)
        {
            Category = category;
            Items = items;
        }
    }
}
