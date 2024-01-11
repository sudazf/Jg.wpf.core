using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Xml;

namespace Jg.wpf.controls.Customer.ThreadControls
{
    [ContentProperty("Child")]
    public class ThreadControl : FrameworkElement
    {
        private readonly AutoResetEvent _resentEvent;
        private HostVisual _hostVisual;
        private Func<object, FrameworkElement> _createContentFromStyle;
        private UIElement _child;

        public event EventHandler OnContentLoadCompleted;

        public FrameworkElement UiContent;
        public Dispatcher ThreadDispatcher;

        public Style ThreadSeparatedStyle
        {
            get => (Style)GetValue(ThreadSeparatedStyleProperty);
            set => SetValue(ThreadSeparatedStyleProperty, value);
        }

        public static readonly DependencyProperty ThreadSeparatedStyleProperty = DependencyProperty.Register(
            "ThreadSeparatedStyle",
            typeof(Style),
            typeof(ThreadControl),
            new FrameworkPropertyMetadata(null, OnThreadSeparatedStyleChanged));

        private static void OnThreadSeparatedStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ThreadControl)d;

            if (e.NewValue is Style style)
            {
                var templateDict = new Dictionary<DependencyProperty, string>();
                var invokingType = style.TargetType;
                var setters = style.Setters.ToArray();

                foreach (var setterBase in setters)
                {
                    var setter = (Setter)setterBase;

                    if (setter.Value is FrameworkTemplate oldTemp && !templateDict.ContainsKey(setter.Property))
                    {
                        var templateString = XamlWriter.Save(oldTemp);
                        templateDict.Add(setter.Property, templateString);
                    }
                }

                control._createContentFromStyle = (dataContext) =>
                {
                    var newStyle = new Style
                    {
                        TargetType = invokingType
                    };
                    foreach (var setterBase in setters)
                    {
                        var setter = (Setter)setterBase;

                        if (templateDict.TryGetValue(setter.Property, out var templateString))
                        {
                            var reader = new StringReader(templateString);
                            var xmlReader = XmlReader.Create(reader);
                            var template = XamlReader.Load(xmlReader); //can not use binding.
                            setter = new Setter(setter.Property, template);
                        }

                        newStyle.Setters.Add(setter);
                    }

                    var content = (FrameworkElement)Activator.CreateInstance(newStyle.TargetType);
                    content.Style = newStyle;
                    content.DataContext = dataContext;
                    content.Width = control.ActualWidth;
                    content.Height = control.ActualHeight;
                    return content;
                };
            }
            else
            {
                control._createContentFromStyle = null;
            }
        }

        public UIElement Child
        {
            get => _child;
            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }

                _child = value;

                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        public ThreadControl()
        {
            _resentEvent = new AutoResetEvent(false);
            Loaded += OnControlLoaded;
            Unloaded += OnControlUnLoaded;
        }


        public void Reload()
        {
            DestroyThreadSeparatedElement();
            CreateThreadSeparatedElement(DataContext);
        }

        private void OnControlUnLoaded(object sender, RoutedEventArgs e)
        {
            DestroyThreadSeparatedElement();
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            CreateThreadSeparatedElement(DataContext);
        }

        public static T Clone<T>(T source)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }

        protected virtual FrameworkElement CreateUiContent(object dataContext)
        {
            return _createContentFromStyle?.Invoke(dataContext);
        }

        protected virtual void CreateThreadSeparatedElement(object dataContext)
        {
            _hostVisual = new HostVisual();
            AddLogicalChild(_hostVisual);
            AddVisualChild(_hostVisual);

            var thread = new Thread(CreateContentOnSeparateThread)
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(dataContext);

            _resentEvent.WaitOne();

            InvalidateMeasure();
        }

        protected virtual void DestroyThreadSeparatedElement()
        {
            if (ThreadDispatcher != null)
            {
                ThreadDispatcher.InvokeShutdown();

                RemoveLogicalChild(_hostVisual);
                RemoveVisualChild(_hostVisual);

                _hostVisual = null;
                UiContent = null;
                ThreadDispatcher = null;
            }
        }

        private void CreateContentOnSeparateThread(object dataContext)
        {
            if (_hostVisual != null)
            {
                var visualTarget = new VisualTargetPresentationSource(_hostVisual);

                UiContent = CreateUiContent(dataContext);

                if (UiContent == null)
                {
                    throw new InvalidOperationException("Created UI Content cannot return null. Either override 'CreateUiContent()' or assign a style to 'ThreadSeparatedStyle'");
                }

                ThreadDispatcher = UiContent.Dispatcher;

                _resentEvent.Set();
                visualTarget.RootVisual = UiContent;

                //workaround
                UiContent.Visibility = Visibility.Collapsed;

                OnContentLoadCompleted?.Invoke(this, EventArgs.Empty);

                Dispatcher.Run();

                visualTarget.Dispose();
            }
        }


        protected override int VisualChildrenCount =>
            Child != null && _hostVisual != null ? 2
            : Child != null || _hostVisual != null ? 1
            : 0;

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (Child != null)
                {
                    yield return Child;
                }

                if (_hostVisual != null)
                {
                    yield return _hostVisual;
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (Child != null)
            {
                switch (index)
                {
                    case 0:
                        return Child;

                    case 1:
                        return _hostVisual;
                }
            }
            else if (index == 0)
            {
                return Child != null ? (Visual)Child : _hostVisual;
            }

            throw new IndexOutOfRangeException("index");
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var childSize = new Size();
            var uiSize = new Size();

            if (Child != null)
            {
                Child.Measure(constraint);

                var element = Child as FrameworkElement;
                childSize.Width = element?.ActualWidth ?? Child.DesiredSize.Width;
                childSize.Height = element?.ActualHeight ?? Child.DesiredSize.Height;
            }

            if (UiContent != null)
            {
                UiContent.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => UiContent.Measure(constraint)));
                uiSize.Width = UiContent.ActualWidth;
                uiSize.Height = UiContent.ActualHeight;
            }

            var size = new Size(Math.Max(childSize.Width, uiSize.Width), Math.Max(childSize.Height, uiSize.Height)); ;
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Child?.Arrange(new Rect(finalSize));

            UiContent?.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => UiContent.Arrange(new Rect(finalSize))));

            return finalSize;
        }

    }
}
