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

        public MultiSelectComboBoxViewModel()
        {
            TestItems = new List<MySelectableItem>()
            {
                new MySelectableItem(false, "Tom"),
                new MySelectableItem(true, "Jerry"),
            };

            Containers = new List<MyContainer>()
            {
                new MyContainer("Category1", new List<MySelectableItem>()
                {
                    new MySelectableItem(false, "Jack"),
                    new MySelectableItem(true, "Hellen"),
                }),
                new MyContainer("Category2", new List<MySelectableItem>()
                {
                    new MySelectableItem(false, "pony"),
                    new MySelectableItem(true, "jimi"),
                }),
            };
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
