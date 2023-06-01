using System;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    [Serializable]
    public class EditCellObject : ICellValue
    {
        public EditCellType EditCellType { get; }
        public bool IsReadonly { get; }
        public string Display { get; set; }
        public object Value { get; set; }

        public EditCellObject(EditCellType type, object value, string display = "", bool isReadonly = false)
        {
            EditCellType = type;
            Value = value;
            Display = display;
            IsReadonly = isReadonly;
        }

        public EditCellObject Copy()
        {
            return new EditCellObject(EditCellType, Value, Display, IsReadonly);
        }
    }
}
