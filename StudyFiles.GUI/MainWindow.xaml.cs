using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudyFiles.DTO;

// ReSharper disable PossibleNullReferenceException

namespace StudyFiles.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _depth;
        private string _lastSearchQuery;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();
        }

        // Otherwise mouse double click is not working
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            _depth--;
        }
        private void Table_OnItemDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Left)
                return;

            var vm = DataContext as ApplicationViewModel;
            var listBox = sender as ListBox;

            if (_depth == 5 || listBox.SelectedItem is NotFoundDTO)
                return;

            _depth++;

            if (_depth == 5)
            {
                vm.ShowFile();
                return;
            }

            if (listBox.SelectedItem is SearchResultDTO)
            {
                vm.ShowFile(_lastSearchQuery);
                return;
            }

            vm.GetNextItemList();
            Back.IsEnabled = true;
        }

        private void NewItem_OnInitialized(object? sender, EventArgs e)
        {
            if(sender is TextBox textBox)
                textBox.Focus();
        }
        private void NewItem_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text == "")
                return;

            var vm = DataContext as ApplicationViewModel;
            vm.AddItem(textBox.Text);
        }
        private void NewItem_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Table.Focus();
        }

        private void SearchBox_OnMouseEnter(object sender, MouseEventArgs e)
        {
            SearchBorder.Background = Brushes.AliceBlue;
        }
        private void SearchBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SearchBox.IsFocused)
                SearchBorder.Background = Brushes.Transparent;
        }
        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if(SearchBox.Text.Length == 0)
                SearchBorder.Background = Brushes.Transparent;
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            _lastSearchQuery = SearchBox.Text;
        }
    }
}
