using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable PossibleNullReferenceException

namespace StudyFiles.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _depth;

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

            if (_depth == 5)
                return;

            _depth++;

            if (_depth == 5)
            {
                vm.ShowFile();
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
    }
}
