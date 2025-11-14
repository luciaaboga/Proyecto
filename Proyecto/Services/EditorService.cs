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
                project.Perspective = editorState.Perspective;
                project.PerspectiveVertical = editorState.PerspectiveVertical;

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                // Guardar stickers
                project.StickersJson = JsonSerializer.Serialize(editorState.Stickers, serializerOptions);

                // Guardar textos
                project.TextElementsJson = JsonSerializer.Serialize(editorState.TextElements, serializerOptions);

                project.LastModified = DateTime.Now;

                // Actualizar proyecto en la base de datos
                _projectService.UpdateProject(project);
            }
        }

        public async Task<Project> GetProjectForEditingAsync(Guid projectId)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project != null)
            {
                var editorState = await GetEditorStateAsync(projectId);

                if (editorState == null)
                {
                    editorState = new EditorState();
                }

                // Sincronizar con los datos del proyecto
                editorState.Brightness = project.Brightness;
                editorState.Contrast = project.Contrast;
                editorState.Saturation = project.Saturation;
                editorState.Rotation = project.Rotation;
                editorState.FlipHorizontal = project.FlipHorizontal;
                editorState.FlipVertical = project.FlipVertical;
                editorState.Perspective = project.Perspective;
                editorState.PerspectiveVertical = project.PerspectiveVertical;

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Cargar stickers desde el proyecto
                if (!string.IsNullOrEmpty(project.StickersJson))
                {
                    try
                    {
                        editorState.Stickers = JsonSerializer.Deserialize<List<StickerElement>>(
                            project.StickersJson, serializerOptions) ?? new List<StickerElement>();
                    }
                    catch (JsonException)
                    {
                        editorState.Stickers = new List<StickerElement>();
                    }
                }

                // Cargar textos desde el proyecto
                if (!string.IsNullOrEmpty(project.TextElementsJson))
                {
                    try
                    {
                        editorState.TextElements = JsonSerializer.Deserialize<List<TextElement>>(
                            project.TextElementsJson, serializerOptions) ?? new List<TextElement>();
                    }
                    catch (JsonException)
                    {
                        editorState.TextElements = new List<TextElement>();
                    }
                }
                
                await SaveEditorStateAsync(projectId, editorState);
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