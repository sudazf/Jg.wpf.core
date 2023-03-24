using System.Collections;
using System.Collections.Generic;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public interface ITextBoxSuggestionsSource : IAutocompleteSource<string>
    {
    }

    public abstract class TextBoxSuggestionsSource : ITextBoxSuggestionsSource
    {
        public TextBoxSuggestionsSource() { }

        public abstract IEnumerable<string> Search(string searchTerm);

        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
