using System.Windows;
using System.Windows.Controls;
using StudyFiles.DTO;
// ReSharper disable PossibleNullReferenceException

namespace StudyFiles.GUI
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var ui = container as FrameworkElement;

            return item switch
            {
                NullDTO _ => (ui.FindResource("NullObject") as DataTemplate),
                FileDTO _ => (ui.FindResource("File") as DataTemplate),
                FileViewDTO _ => (ui.FindResource("FileView") as DataTemplate),
                _ => (ui.FindResource("RealItem") as DataTemplate)
            };
        }
    }
}
