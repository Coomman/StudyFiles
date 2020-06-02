using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var nullObject = new NullDTO();

            var vm = DataContext as ApplicationViewModel;
            vm.Models.Add(nullObject);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (_depth == 1)
                Back.IsEnabled = false;

            var vm = DataContext as ApplicationViewModel;
            vm.GetPrevItemList();

            _depth--;
        }
            
        private void Items_OnItemDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Left)
                return;

            var vm = DataContext as ApplicationViewModel;
            vm.GetNextItemList();

            _depth++;
            Back.IsEnabled = true;
        }

        private void NewItem_OnInitialized(object? sender, EventArgs e)
        {
            var textBox = sender as TextBox;

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
                ListBox.Focus();
        }
    }
}
