using Proyecto.Contracts;
using Proyecto.Models;
using System.Text.Json;

namespace Proyecto.Services
{
    public class EditorService : IEditorService
    {
        private readonly IProjectService _projectService;
        private readonly ILocalStorageService _localStorage;
        private const string EditorStateKey = "editorState_";

        public EditorService(IProjectService projectService, ILocalStorageService localStorage)
        {
            _projectService = projectService;
            _localStorage = localStorage;
        }

        public async Task ApplyFiltersAsync(Guid projectId, EditorState editorState)
        {
            await SaveEditorStateAsync(projectId, editorState);

            var project = _projectService.GetProjectById(projectId);
            if (project != null)
            {
                project.Brightness = editorState.Brightness;
                project.Contrast = editorState.Contrast;
                project.Saturation = editorState.Saturation;
                project.Rotation = editorState.Rotation;
                project.FlipHorizontal = editorState.FlipHorizontal;
                project.FlipVertical = editorState.FlipVertical;

                project.StickersJson = JsonSerializer.Serialize(editorState.Stickers);
                project.TextElementsJson = JsonSerializer.Serialize(editorState.TextElements);

                project.LastModified = DateTime.Now;
                _projectService.UpdateProject(project);
            }
        }

        public async Task<Project> GetProjectForEditingAsync(Guid projectId)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project != null)
            {
                var editorState = await GetEditorStateAsync(projectId);
                if (editorState != null)
                {
                    project.Brightness = editorState.Brightness;
                    project.Contrast = editorState.Contrast;
                    project.Saturation = editorState.Saturation;
                    project.Rotation = editorState.Rotation;
                    project.FlipHorizontal = editorState.FlipHorizontal;
                    project.FlipVertical = editorState.FlipVertical;

                    if (!string.IsNullOrEmpty(project.StickersJson))
                    {
                        editorState.Stickers = JsonSerializer.Deserialize<List<StickerElement>>(project.StickersJson) ?? new List<StickerElement>();
                    }
                    if (!string.IsNullOrEmpty(project.TextElementsJson))
                    {
                        editorState.TextElements = JsonSerializer.Deserialize<List<TextElement>>(project.TextElementsJson) ?? new List<TextElement>();
                    }
                }
            }
            return project;
        }
        public async Task ResetFiltersAsync(Guid projectId)
        {
            var defaultState = new EditorState();
            await ApplyFiltersAsync(projectId, defaultState);
        }

        public async Task<string> GenerateEditedImageAsync(Project project, EditorState editorState)
        {   
            return project.ImagePath;
        }

        public async Task SaveEditorStateAsync(Guid projectId, EditorState editorState)
        {
            var key = $"{EditorStateKey}{projectId}";
            await _localStorage.SetItemAsync(key, editorState);
        }

        private async Task<EditorState?> GetEditorStateAsync(Guid projectId)
        {
            var key = $"{EditorStateKey}{projectId}";
            return await _localStorage.GetItemAsync<EditorState>(key);
        }
    }
}