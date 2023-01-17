﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// RefreshPerFrameDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RefreshPerFrameDemo : UserControl
    {
        public RefreshPerFrameDemo()
        {
            InitializeComponent();

            CompositionTargetEx.Rendering += OnRendering;
        }

        private void OnRendering(object sender, RenderingEventArgs e)
        {
            if (DataContext is RefreshPerFrameViewModel vm)
            {
                vm.Refresh();
            }
        }
    }

    public static class CompositionTargetEx
    {
        private static TimeSpan _last = TimeSpan.Zero;
        private static event EventHandler<RenderingEventArgs> _frameUpdating;
        public static event EventHandler<RenderingEventArgs> Rendering
        {
            add
            {
                if (_frameUpdating == null)
                {
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }

                _frameUpdating += value;
            }
            remove
            {
                _frameUpdating -= value;
                if (_frameUpdating == null)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            }
        }

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs args = (RenderingEventArgs)e;
            if (args.RenderingTime - _last < TimeSpan.FromMilliseconds(30))
            {
                return;
            }

            _last = args.RenderingTime;
            _frameUpdating?.Invoke(sender, args);
        }
    }
}
