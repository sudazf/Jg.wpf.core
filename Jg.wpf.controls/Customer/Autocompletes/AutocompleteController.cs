using System;
using System.Collections;
using System.Threading.Tasks;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public class AutocompleteController
    {
        private readonly object _lockObject = new object();


        private const int SearchDelay = 300;

        private IAutocompleteSource _autocompleteSource;

        private string _lastId;

        public event AutocompleteItemsChangedEventHandler AutocompleteItemsChanged;

        private string LastId
        {
            get
            {
                lock (_lockObject)
                {
                    return _lastId;
                }
            }

            set
            {
                lock (_lockObject)
                {
                    _lastId = value;
                }
            }
        }

        public IAutocompleteSource AutocompleteSource
        {
            get
            {
                lock (_lockObject)
                {
                    return _autocompleteSource;
                }
            }

            set
            {
                lock (_lockObject)
                {
                    _autocompleteSource = value;
                }
            }
        }

        public AutocompleteController()
        {
            _autocompleteSource = null;

            _lastId = null;
        }

        public void Search(string searchTerm)
        {
            Task.Run(async () =>
            {
                string id = Guid.NewGuid().ToString();

                IAutocompleteSource autocompleteSource = null;

                lock (_lockObject)
                {
                    autocompleteSource = _autocompleteSource;

                    // no source, no search
                    if (autocompleteSource == null)
                    {
                        return;
                    }

                    LastId = id;
                }

                await Task.Delay(SearchDelay).ConfigureAwait(false);

                // search only if there was no other request during the delay
                if (DoSearchWithId(id))
                {
                    IEnumerable items = autocompleteSource.Search(searchTerm);

                    // a final check if this result will not be replaced by another active search
                    if (DoSearchWithId(id))
                    {
                        AutocompleteItemsChanged?.Invoke(this, new AutocompleteItemsChangedEventArgs(items));
                    }
                }
            });
        }

        private bool DoSearchWithId(string id)
        {
            lock (_lockObject)
            {
                return LastId == null || LastId == id;
            }
        }
    }

    public class AutocompleteItemsChangedEventArgs : EventArgs
    {
        public IEnumerable Items { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AutocompleteItemsChangedEventArgs" />.
        /// </summary>
        /// <param name="items">The result of the autocomplete</param>
        public AutocompleteItemsChangedEventArgs(IEnumerable items)
        {
            Items = items;
        }
    }

    public delegate void AutocompleteItemsChangedEventHandler(object sender, AutocompleteItemsChangedEventArgs args);
}
