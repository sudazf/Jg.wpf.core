using System.Windows.Input;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public class TextBoxSuggestionsCommands
    {
        private TextBoxSuggestionsCommands() { }

        public static readonly RoutedCommand SelectSuggestionItemCommand = new RoutedCommand();
    }
}
