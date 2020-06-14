using System.Windows;
using System.Windows.Controls;
using StudyFiles.DTO;

// ReSharper disable PossibleNullReferenceException

namespace StudyFiles.GUI.Controls
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var ui = container as FrameworkElement;

            var template = item switch
            {
                NullDTO _ => "NullObject",
                FileDTO _ => "File",
                FileViewDTO _ => "FileView",
                _ => "RealItem"
            };

            return ui.FindResource(template) as DataTemplate;
        }
    }
}
