using System;

namespace Jg.wpf.core.Extensions.Types
{
    public interface ISelectable
    {
        event EventHandler OnSelectedChanged;
        bool IsSelected { get; set; }
    }
}
