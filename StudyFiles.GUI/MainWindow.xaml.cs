using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
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
            var vm = DataContext as ApplicationViewModel;

            if (_depth != 4)
            {
                vm.Models.Add(new NullDTO());
                return;
            }

            var fd = new OpenFileDialog
            {
                Title = "Select a file",

                Filter = "PDF files (*.pdf)|*.pdf|" +
                         "Microsoft Word (*.doc;*.docx)|*.doc;*.docx|" +
                         "Text files (*.txt)|*.txt" 
            };

            if (fd.ShowDialog() == true)
                vm.AddItem(fd.FileName);
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
            
        private void Table_OnItemDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Left)
                return;

            var vm = DataContext as ApplicationViewModel;

            if (_depth == 4)
            {
                vm.ShowFile();

                _depth++;
                return;
            }

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
                Table.Focus();
        }
    }
}
