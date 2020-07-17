using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace StudyFiles.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new ApplicationViewModel();
            vm.OnSearchStateExit += OnSearchStateExit;

            DataContext = vm;
        }

        private void OnSearchStateExit()
        {
            SearchBox.Clear();
            SearchBox.Focus();
            Table.Focus();
        }

        private void NewItem_OnInitialized(object? sender, EventArgs e)
        {
            if(sender is TextBox textBox)
                textBox.Focus();
        }
        private void NewItem_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;

            if (textBox.Text.Length == 0)
                return;

            var vm = (ApplicationViewModel) DataContext;
            vm.AddFolder(textBox.Text);
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
            if (!SearchBox.IsFocused && SearchBox.Text.Length == 0)
                SearchBorder.Background = Brushes.Transparent;
        }
        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if(SearchBox.Text.Length == 0)
                SearchBorder.Background = Brushes.Transparent;
        }
    }
}
