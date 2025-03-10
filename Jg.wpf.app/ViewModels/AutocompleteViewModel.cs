﻿using System.Collections.Generic;
using System.Linq;
using Jg.wpf.controls.Customer.Autocompletes;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class AutocompleteViewModel : ViewModelBase
    {
        private ITextBoxSuggestionsSource textBoxSuggestionsSource;
        private string _text;
        private string _selected;

        public ITextBoxSuggestionsSource TextBoxSuggestionsSource => textBoxSuggestionsSource;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged(()=>Text);
            }
        }

        public string Selected
        {
            get => _selected;
            set
            {
                if (value == _selected) return;
                _selected = value;
                RaisePropertyChanged(nameof(Selected));
            }
        }

        public AutocompleteViewModel()
        {
            textBoxSuggestionsSource = new OperatingSystemTextBoxSuggestionsSource();

        }
    }

    public class OperatingSystemTextBoxSuggestionsSource : TextBoxSuggestionsSource
    {
        private List<string> _items;

        public OperatingSystemTextBoxSuggestionsSource()
        {
            _items = new List<string>()
            {
                "Android Gingerbread",
                "Android Icecream Sandwich",
                "Android Jellybean",
                "Android Lollipop",
                "Android Nougat",
                "Debian",
                "Mac OSX",
                "Raspbian",
                "Ubuntu Wily Werewolf",
                "Ubuntu Xenial Xerus",
                "Ubuntu Yakkety Yak",
                "Ubuntu Zesty Zapus",
                "Windows 7",
                "Windows 8",
                "Windows 8.1",
                "Windows 10",
                "Windows Vista",
                "Windows XP"
            };
        }

        public override IEnumerable<string> Search(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;
            searchTerm = searchTerm.ToLower();

            return _items.Where(item => item.ToLower().Contains(searchTerm));
        }
    }
}
