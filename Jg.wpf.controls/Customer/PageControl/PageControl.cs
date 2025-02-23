using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types.Pages;

namespace Jg.wpf.controls.Customer.PageControl
{
    public class PageControl : Control
    {
        private ItemsControl _itemsControl;
        private Button _preButton;
        private Button _nextButton;

        public ObservableCollection<PageItem> PageItems
        {
            get => (ObservableCollection<PageItem>)GetValue(PageItemsProperty);
            set => SetValue(PageItemsProperty, value);
        }

        public static readonly DependencyProperty PageItemsProperty =
            DependencyProperty.Register("PageItems", typeof(ObservableCollection<PageItem>), typeof(PageControl),
                new PropertyMetadata(null, OnPageItemsChanged));

        private static void OnPageItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageControl pageControl)
            {
                if (e.NewValue is ObservableCollection<PageItem> items)
                {
                    //foreach (var item in items)
                    //{
                    //    item.OnPageChanged += pageControl.Item_OnPageChanged;
                    //}
                }
            }
        }

        private void Item_OnPageChanged(object sender, PageChangedEventArgs e)
        {
            CurrentPage = e.Page;
        }


        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(PageControl), new PropertyMetadata(0));


        public int RecordCount
        {
            get => (int)GetValue(RecordCountProperty);
            set => SetValue(RecordCountProperty, value);
        }

        public static readonly DependencyProperty RecordCountProperty =
            DependencyProperty.Register("RecordCount", typeof(int), typeof(PageControl), new PropertyMetadata(0, OnRecordCountChanged));

        private static void OnRecordCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageControl pageControl)
            {
                pageControl.CalcPageNumList();
            }
        }

        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageControl), new PropertyMetadata(10, OnPageSizeChanged));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageControl pageControl)
            {
                pageControl.CalcPageNumList();
            }
        }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PageControl), new PropertyMetadata(0, OnCurrentPageChanged));

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageControl pageControl)
            {
                pageControl.CalcPageNumList();
            }
        }

        public int ContinuousCount
        {
            get => (int)GetValue(ContinuousCountProperty);
            set => SetValue(ContinuousCountProperty, value);
        }

        public static readonly DependencyProperty ContinuousCountProperty =
            DependencyProperty.Register("ContinuousCount", typeof(int), typeof(PageControl), new PropertyMetadata(3, OnContinuousCountChanged));

        private static void OnContinuousCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageControl pageControl)
            {
                pageControl.CalcPageNumList();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _itemsControl = Template.FindName("PART_Items", this) as ItemsControl;
            _preButton = Template.FindName("PART_btnPrePage", this) as Button;
            _nextButton = Template.FindName("PART_tnNextPage", this) as Button;

            if (_itemsControl != null)
            {
                _itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("PageItems"){ Source = this});
            }

            if (_preButton != null)
            {
                _preButton.Click += PreButtonOnClick;

            }
            if (_nextButton != null)
            {
                _nextButton.Click += NextButtonOnClick;
            }
        }

        private void NextButtonOnClick(object sender, RoutedEventArgs e)
        {
            int nextPage = CurrentPage + 1;
            if (nextPage > PageCount) nextPage = PageCount;
            if (nextPage != CurrentPage)
            {
                CurrentPage = nextPage;
            }
        }

        private void PreButtonOnClick(object sender, RoutedEventArgs e)
        {
            int prePage = CurrentPage - 1;
            if (prePage < 1) prePage = 1;
            if (prePage != CurrentPage)
            {
                CurrentPage = prePage;
            }
        }

        public PageControl()
        {
            PageItems = new ObservableCollection<PageItem>();
        }

        private void CalcPageNumList()
        {
            PageCount = (RecordCount - 1) / PageSize + 1; //计算总页数PageCount

            var newPageItems = new ObservableCollection<PageItem>();

            //第一页
            var first = new PageItem(1, CurrentPage);
            first.OnPageChanged += Item_OnPageChanged;
            newPageItems.Add(first);

            //当前页码连续页码
            for (int i = CurrentPage - ContinuousCount; i <= CurrentPage + ContinuousCount; i++)
            {
                if (i > 0 && i < PageCount)
                {
                    var item = new PageItem(i, CurrentPage);
                    item.OnPageChanged += Item_OnPageChanged;
                    if (newPageItems.FirstOrDefault(a => a.Page == item.Page) == null)
                    {
                        newPageItems.Add(item);
                    }
                }
            }

            //最后一页
            var last = new PageItem(PageCount, CurrentPage);
            last.OnPageChanged += Item_OnPageChanged;
            if (newPageItems.FirstOrDefault(a => a.Page == last.Page) == null)
            {
                newPageItems.Add(last);
            }

            for (int i = newPageItems.Count - 1; i > 0; i--)
            {
                if (newPageItems[i].Page - newPageItems[i - 1].Page > 1)
                {
                    newPageItems.Insert(i, new PageItem(0, CurrentPage, 2));
                }
            }

            foreach (var pageItem in PageItems)
            {
                pageItem.OnPageChanged -= Item_OnPageChanged;
            }
            PageItems.Clear();
            PageItems = newPageItems;

            if (_preButton != null)
            {
                _preButton.IsEnabled = CurrentPage != 1;
            }

            if (_nextButton != null)
            {
                _nextButton.IsEnabled = CurrentPage != PageCount;
            }
        }
    }
}
