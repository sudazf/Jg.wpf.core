using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// DisplayChartDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DisplayChartDemo : UserControl
    {
        private readonly ObservableDataSource<Point> _dataSource = new ObservableDataSource<Point>();
        private readonly PerformanceCounter _cpuPerformance = new PerformanceCounter();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private int i = 0;

        public DisplayChartDemo()
        {
            InitializeComponent();
        }

        private void AnimatedPlot(object sender, EventArgs e)
        {
            _cpuPerformance.CategoryName = "Processor";
            _cpuPerformance.CounterName = "% Processor Time";
            _cpuPerformance.InstanceName = "_Total";

            double x = i;
            double y = _cpuPerformance.NextValue();

            Point point = new Point(x, y);
            _dataSource.AppendAsync(base.Dispatcher, point);

            cpuUsageText.Text = $"{y:0}%";
            i++;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            plotter.AddLineGraph(_dataSource, Colors.Green, 2, "Percentage");
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += AnimatedPlot;
            _timer.IsEnabled = true;
            plotter.Viewport.FitToView();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.IsEnabled = false;
        }
    }
}
