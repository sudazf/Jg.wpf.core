using System.Collections;
using System.Collections.Generic;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public interface IAutocompleteSource
    {
        IEnumerable Search(string searchTerm);
    }

    public interface IAutocompleteSource<T> : IAutocompleteSource
    {
        new IEnumerable<T> Search(string searchTerm);
    }

    public abstract class AutocompleteSource<T> : IAutocompleteSource<T>
    {
        public AutocompleteSource() { }

        public abstract IEnumerable<T> Search(string searchTerm);

        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
