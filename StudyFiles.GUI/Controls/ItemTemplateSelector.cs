using System.Windows;
using System.Windows.Controls;
using StudyFiles.DTO.Files;
using StudyFiles.DTO.Service;

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
                SearchResultDTO _ => "SearchResult",
                FileDTO _ => "File",
                NullDTO _ => "NullObject",
                FileViewDTO _ => "FileView",
                _ => "Folder"
            };

            return ui.FindResource(template) as DataTemplate;
        }
    }
}
