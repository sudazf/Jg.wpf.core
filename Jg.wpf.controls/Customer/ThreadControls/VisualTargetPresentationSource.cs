using System;
using System.Windows.Media;
using System.Windows;

namespace Jg.wpf.controls.Customer.ThreadControls
{
    public class VisualTargetPresentationSource : PresentationSource, IDisposable
    {
        private readonly VisualTarget _visualTarget;
        private bool _isDisposed;
        public override bool IsDisposed => _isDisposed;

        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
            AddSource();
        }

        public void Dispose()
        {
            RemoveSource();
            _isDisposed = true;
        }

        public override Visual RootVisual
        {
            get
            {
                try
                {
                    return _visualTarget.RootVisual;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                var oldRoot = _visualTarget.RootVisual;

                _visualTarget.RootVisual = value;

                RootChanged(oldRoot, value);

                if (value is UIElement rootElement)
                {
                    rootElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }
    }
}
