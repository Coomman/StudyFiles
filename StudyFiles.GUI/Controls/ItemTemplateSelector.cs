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
                NotFoundDTO _ => "NoMatches",
                SearchResultDTO _ => "File",
                NullDTO _ => "NullObject",
                FileDTO _ => "File",
                FileViewDTO _ => "FileView",
                _ => "Folder"
            };

            return ui.FindResource(template) as DataTemplate;
        }
    }
}
