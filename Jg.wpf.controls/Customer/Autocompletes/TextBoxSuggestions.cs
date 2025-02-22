using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    [ContentProperty(nameof(TextBox))]
    public class TextBoxSuggestions : ControlWithAutocompletePopup
    {
        private ItemsControl m_suggestionItemsControl;
        private AutocompleteController m_autocompleteController;

        private static readonly string SuggestionItemsControlName = "suggestionItemsControl";
        private static readonly string SuggestionItemsPopupName = "suggestionItemsPopup";


        public static readonly DependencyProperty KeepFocusOnSelectionProperty = DependencyProperty.Register(
            nameof(KeepFocusOnSelection), typeof(bool), typeof(TextBoxSuggestions), new PropertyMetadata(false));


        public bool KeepFocusOnSelection
        {
            get => (bool)GetValue(KeepFocusOnSelectionProperty);

            set => SetValue(KeepFocusOnSelectionProperty, value);
        }


        public static readonly DependencyProperty TextBoxProperty = DependencyProperty.Register(
            nameof(TextBox), typeof(TextBox), typeof(TextBoxSuggestions), new PropertyMetadata(null, TextBoxChangedHandler));

        public TextBox TextBox
        {
            get => (TextBox)GetValue(TextBoxProperty);

            set => SetValue(TextBoxProperty, value);
        }

        public static readonly DependencyProperty TextBoxSuggestionsSourceProperty = DependencyProperty.Register(
            nameof(TextBoxSuggestionsSource), typeof(ITextBoxSuggestionsSource), typeof(TextBoxSuggestions), new PropertyMetadata(null, TextBoxSuggestionsSourceChangedHandler));

        public ITextBoxSuggestionsSource TextBoxSuggestionsSource
        {
            get => (ITextBoxSuggestionsSource)GetValue(TextBoxSuggestionsSourceProperty);

            set => SetValue(TextBoxSuggestionsSourceProperty, value);
        }


        public string SelectedItem
        {
            get => (string)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(string), typeof(TextBoxSuggestions), new PropertyMetadata(""));


        static TextBoxSuggestions()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxSuggestions), new FrameworkPropertyMetadata(typeof(TextBoxSuggestions)));
        }

        public TextBoxSuggestions()
            : base()
        {
            m_suggestionItemsControl = null;

            m_autocompleteController = new AutocompleteController() { AutocompleteSource = TextBoxSuggestionsSource };

            CommandBindings.Add(new CommandBinding(TextBoxSuggestionsCommands.SelectSuggestionItemCommand, SelectSuggestionItemCommandHandler));

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = Template.FindName(SuggestionItemsPopupName, this) as AutocompletePopup;

            m_suggestionItemsControl = Template.FindName(SuggestionItemsControlName, this) as ItemsControl;
        }

        protected override void LoadedHandler(object sender, RoutedEventArgs args)
        {
            base.LoadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged += AutocompleteItemsChangedHandler;
            }

            if (TextBox != null)
            {
                // first remove the event handler to prevent multiple registrations
                TextBox.TextChanged -= TextBoxTextChangedHandler;
                TextBox.KeyUp -= TextBoxKeyUpHandler;

                // and then set the event handler
                TextBox.TextChanged += TextBoxTextChangedHandler;
                TextBox.KeyUp += TextBoxKeyUpHandler;
            }
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            base.UnloadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged -= AutocompleteItemsChangedHandler;
            }

            if (TextBox != null)
            {
                TextBox.TextChanged -= TextBoxTextChangedHandler;
                TextBox.KeyUp -= TextBoxKeyUpHandler;
            }
        }

        private void SelectSuggestionItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (TextBox != null)
            {
                TextBox.Text = args.Parameter as string ?? string.Empty;

                SetValue(SelectedItemProperty, TextBox.Text);

                if (KeepFocusOnSelection)
                {
                    Keyboard.Focus(TextBox);
                    TextBox.CaretIndex = TextBox.Text.Length;
                }
                else
                {
                    Keyboard.Focus(null);
                }
            }
        }

        private static void TextBoxChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as TextBoxSuggestions)?.TextBoxChangedHandler(args.OldValue as TextBox, args.NewValue as TextBox);
        }

        private void TextBoxChangedHandler(TextBox oldTextBox, TextBox newTextBox)
        {
            if (oldTextBox != null)
            {
                oldTextBox.TextChanged -= TextBoxTextChangedHandler;
                newTextBox.KeyUp -= TextBoxKeyUpHandler;
            }

            if (newTextBox != null)
            {
                newTextBox.TextChanged += TextBoxTextChangedHandler;
                newTextBox.KeyUp += TextBoxKeyUpHandler;
            }
        }

        private void TextBoxTextChangedHandler(object sender, TextChangedEventArgs args)
        {
            if (sender == TextBox && IsEnabled && IsLoaded && TextBox.IsLoaded && TextBox.IsFocused)
            {
                m_autocompleteController?.Search(TextBox.Text);
            }
        }

        private void TextBoxKeyUpHandler(object sender, KeyEventArgs args)
        {
            if (sender == TextBox && args.Key == Key.Down)
            {
                m_suggestionItemsControl.Focus();
            }
        }

        private static void TextBoxSuggestionsSourceChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as TextBoxSuggestions)?.TextBoxSuggestionsSourceChangedHandler(args.NewValue as ITextBoxSuggestionsSource);
        }

        private void TextBoxSuggestionsSourceChangedHandler(ITextBoxSuggestionsSource textBoxSuggestionsSource)
        {
            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteSource = textBoxSuggestionsSource;
            }
        }

        private void AutocompleteItemsChangedHandler(object sender, AutocompleteItemsChangedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                SetSuggestionItems(args.Items);
            });
        }

        private void SetSuggestionItems(IEnumerable suggestionItems)
        {
            if (m_suggestionItemsControl != null)
            {
                if (suggestionItems != null)
                {
                    m_suggestionItemsControl.ItemsSource = suggestionItems;
                }
                else
                {
                    m_suggestionItemsControl.ItemsSource = null;
                }
            }
        }
    }
}
