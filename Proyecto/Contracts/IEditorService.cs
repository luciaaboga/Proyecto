using Proyecto.Models;

namespace Proyecto.Contracts
{
    public interface IEditorService
    {
        Task<Project> GetProjectForEditingAsync(Guid projectId);
        Task ApplyFiltersAsync(Guid projectId, EditorState editorState);
        Task ResetFiltersAsync(Guid projectId);
        Task<string> GenerateEditedImageAsync(Project project, EditorState editorState);
        Task SaveEditorStateAsync(Guid projectId, EditorState editorState);
    }
}