using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Jg.wpf.controls.Converter;

namespace Jg.wpf.controls.Customer.AutoTrimmedText
{
    public class AutoTrimmedTextBlock : TextBlock
    {
        public AutoTrimmedTextBlock()
        {
            TextTrimming = TextTrimming.CharacterEllipsis;

            AssociateToolTip();
        }

        private void AssociateToolTip()
        {
            var toolTip = new ToolTip();
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;

            var content = new TextBlock();
            content.SetBinding(TextProperty, new Binding("Text")
            {
                Source = this
            });
            toolTip.Content = content;

            var multiBinding = new MultiBinding()
            {
                Converter = TrimmedTextBlockVisibilityConverter.Converter
            };
            multiBinding.Bindings.Add(new Binding("PlacementTarget")
            {
                RelativeSource = new RelativeSource(mode: RelativeSourceMode.Self)
            });
            toolTip.SetBinding(VisibilityProperty, multiBinding);

            ToolTip = toolTip;
        }
    }
}
