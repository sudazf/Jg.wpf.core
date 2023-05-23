using System;
using System.Collections.Generic;
using Jg.wpf.core.Extensions.Types;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class SelectAllListViewModel : ViewModelBase
    {
        public List<Food> Foods1 { get; }
        public List<Food> Foods2 { get; }

        public SelectAllListViewModel()
        {
            Foods1 = new List<Food>
            {
                new Food("苹果", "苹苹", 5.0f),
                new Food("香蕉", "蕉蕉", 3.0f),
                new Food("草莓", "莓莓", 6.0f),
                new Food("梨子", "梨梨", 7.0f),
                new Food("桃子", "桃桃", 6.0f),
                new Food("蛋糕", "蛋蛋", 10.0f),
            };

            Foods2 = new List<Food>
            {
                new Food("苹果", "苹苹", 5.0f),
                new Food("香蕉", "蕉蕉", 3.0f),
                new Food("草莓", "莓莓", 6.0f),
                new Food("梨子", "梨梨", 7.0f),
                new Food("桃子", "桃桃", 6.0f),
                new Food("蛋糕", "蛋蛋", 10.0f),
            };
        }
    }

    public class Food : ViewModelBase, ISelectable
    {
        private bool _isSelected;

        public event EventHandler OnSelectedChanged;

        public string Name { get; }
        public string Description { get; }
        public float Price { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));

                    OnSelectedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Food(string name, string description, float price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
