using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab4_5.Controls
{
    public partial class CustomSearchBar : UserControl
    {
        public CustomSearchBar()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SearchBoxProperty =
            DependencyProperty.Register("SearchBox", typeof(string), typeof(CustomSearchBar),
                new PropertyMetadata(string.Empty));

        public string SearchBox
        {
            get => (string)GetValue(SearchBoxProperty);
            set => SetValue(SearchBoxProperty, value);
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(CustomSearchBar),
                new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        public static readonly DependencyProperty MaxSearchLengthProperty =
            DependencyProperty.Register("MaxSearchLength", typeof(int), typeof(CustomSearchBar),
                new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.None,
                    null, CoerceMaxSearchLength),
                ValidateMaxSearchLength);

        public int MaxSearchLength
        {
            get => (int)GetValue(MaxSearchLengthProperty);
            set => SetValue(MaxSearchLengthProperty, value);
        }

        private static bool ValidateMaxSearchLength(object value)
        {
            int val = (int)value;
            return val >= 1 && val <= 1000;
        }

        private static object CoerceMaxSearchLength(DependencyObject d, object baseValue)
        {
            int val = (int)baseValue;
            if (val < 1) return 1;
            if (val > 1000) return 1000;
            return val;
        }

        public static readonly DependencyProperty SearchPlaceholderProperty =
            DependencyProperty.Register("SearchPlaceholder", typeof(string), typeof(CustomSearchBar),
                new FrameworkPropertyMetadata("Поиск...", FrameworkPropertyMetadataOptions.None,
                    null, CoerceSearchPlaceholder),
                ValidateSearchPlaceholder);

        public string SearchPlaceholder
        {
            get => (string)GetValue(SearchPlaceholderProperty);
            set => SetValue(SearchPlaceholderProperty, value);
        }

        private static bool ValidateSearchPlaceholder(object value)
        {
            return value is string;
        }

        private static object CoerceSearchPlaceholder(DependencyObject d, object baseValue)
        {
            return baseValue ?? string.Empty;
        }


        public static readonly RoutedEvent SearchInitiatedEvent =
            EventManager.RegisterRoutedEvent("SearchInitiated", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CustomSearchBar));

        public event RoutedEventHandler SearchInitiated
        {
            add { AddHandler(SearchInitiatedEvent, value); }
            remove { RemoveHandler(SearchInitiatedEvent, value); }
        }

        private void RaiseSearchInitiated() =>
            RaiseEvent(new RoutedEventArgs(SearchInitiatedEvent));

        public static readonly RoutedEvent SearchPreviewKeyDownEvent =
            EventManager.RegisterRoutedEvent("SearchPreviewKeyDown", RoutingStrategy.Tunnel, typeof(KeyEventHandler), typeof(CustomSearchBar));

        public event KeyEventHandler SearchPreviewKeyDown
        {
            add { AddHandler(SearchPreviewKeyDownEvent, value); }
            remove { RemoveHandler(SearchPreviewKeyDownEvent, value); }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            RaiseEvent(new KeyEventArgs(e.KeyboardDevice, e.InputSource, e.Timestamp, e.Key)
            {
                RoutedEvent = SearchPreviewKeyDownEvent,
                Source = this
            });
        }

        public static readonly RoutedEvent SearchTextChangedEvent =
            EventManager.RegisterRoutedEvent("SearchTextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomSearchBar));

        public event RoutedEventHandler SearchTextChanged
        {
            add { AddHandler(SearchTextChangedEvent, value); }
            remove { RemoveHandler(SearchTextChangedEvent, value); }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SearchTextChangedEvent));
        }

    }
}
