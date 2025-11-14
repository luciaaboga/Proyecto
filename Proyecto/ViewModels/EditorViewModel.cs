using Proyecto.Contracts;
using Proyecto.Models;
using System.Text.Json;

namespace Proyecto.ViewModels
{
    public class EditorViewModel
    {
        private readonly IEditorService _editorService;
        private readonly IProjectService _projectService;

        public Project? CurrentProject { get; private set; }
        public EditorState EditorState { get; private set; } = new EditorState();
        public string ActiveTab { get; set; } = "filters";

        public EditorViewModel(IEditorService editorService, IProjectService projectService)
        {
            _editorService = editorService;
            _projectService = projectService;
        }

        public async Task LoadProjectAsync(Guid projectId)
        {
            CurrentProject = await _editorService.GetProjectForEditingAsync(projectId);
            if (CurrentProject != null)
            {
                await LoadEditorStateFromProject();
            }
        }

        private async Task LoadEditorStateFromProject()
        {
            if (CurrentProject == null) return;

            EditorState.Brightness = CurrentProject.Brightness;
            EditorState.Contrast = CurrentProject.Contrast;
            EditorState.Saturation = CurrentProject.Saturation;
            EditorState.Rotation = CurrentProject.Rotation;
            EditorState.FlipHorizontal = CurrentProject.FlipHorizontal;
            EditorState.FlipVertical = CurrentProject.FlipVertical;
            EditorState.Perspective = CurrentProject.Perspective;
            EditorState.PerspectiveVertical = CurrentProject.PerspectiveVertical;

            // Cargar stickers desde JSON
            if (!string.IsNullOrEmpty(CurrentProject.StickersJson))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var stickers = JsonSerializer.Deserialize<List<StickerElement>>(CurrentProject.StickersJson, options);
                    if (stickers != null)
                    {
                        EditorState.Stickers = stickers;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializando stickers: {ex.Message}");
                    EditorState.Stickers = new List<StickerElement>();
                }
            }

            // Cargar textos desde JSON
            if (!string.IsNullOrEmpty(CurrentProject.TextElementsJson))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var textElements = JsonSerializer.Deserialize<List<TextElement>>(CurrentProject.TextElementsJson, options);
                    if (textElements != null)
                    {
                        EditorState.TextElements = textElements;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializando textos: {ex.Message}");
                    EditorState.TextElements = new List<TextElement>();
                }
            }

            await SaveEditorState();
        }

        public async Task ApplyFiltersAsync()
        {
            if (CurrentProject != null)
            {
                await _editorService.ApplyFiltersAsync(CurrentProject.Id, EditorState);
                await LoadProjectAsync(CurrentProject.Id);
            }
        }

        public async Task ResetFiltersAsync()
        {
            EditorState = new EditorState();
            if (CurrentProject != null)
            {
                await _editorService.ResetFiltersAsync(CurrentProject.Id);
                await LoadProjectAsync(CurrentProject.Id);
            }
        }

        public async Task<string> GenerateDownloadUrlAsync()
        {
            if (CurrentProject != null)
            {
                return await _editorService.GenerateEditedImageAsync(CurrentProject, EditorState);
            }
            return string.Empty;
        }

        public void SetActiveTab(string tab)
        {
            ActiveTab = tab;
        }

        public void SetBrightness(int value)
        {
            EditorState.Brightness = value;
            _ = SaveEditorState(); 
        }

        public void SetContrast(int value)
        {
            EditorState.Contrast = value;
            _ = SaveEditorState();
        }

        public void SetSaturation(int value)
        {
            EditorState.Saturation = value;
            _ = SaveEditorState();
        }

        public void SetRotation(int value)
        {
            EditorState.Rotation = value;
            _ = SaveEditorState();
        }

        public void ToggleFlipHorizontal()
        {
            EditorState.FlipHorizontal = !EditorState.FlipHorizontal;
            _ = SaveEditorState();
        }

        public void ToggleFlipVertical()
        {
            EditorState.FlipVertical = !EditorState.FlipVertical;
            _ = SaveEditorState();
        }

        public void SetPerspective(int value)
        {
            EditorState.Perspective = value;
            _ = SaveEditorState();
        }

        public void SetPerspectiveVertical(int value)
        {
            EditorState.PerspectiveVertical = value;
            _ = SaveEditorState();
        }

        // Método para guardar el estado actual
        private async Task SaveEditorState()
        {
            if (CurrentProject != null)
            {
                await _editorService.SaveEditorStateAsync(CurrentProject.Id, EditorState);
            }
        }

        public void ApplyPresetFilter(string filterName)
        {
            switch (filterName.ToLower())
            {
                case "normal":
                    EditorState.Brightness = 100;
                    EditorState.Contrast = 100;
                    EditorState.Saturation = 100;
                    break;
                case "vintage":
                    EditorState.Brightness = 120;
                    EditorState.Contrast = 110;
                    EditorState.Saturation = 60;
                    break;
                case "bw":
                    EditorState.Brightness = 90;
                    EditorState.Contrast = 150;
                    EditorState.Saturation = 0;
                    break;
                case "warm":
                    EditorState.Brightness = 120;
                    EditorState.Contrast = 110;
                    EditorState.Saturation = 150;
                    break;
                case "cool":
                    EditorState.Brightness = 90;
                    EditorState.Contrast = 120;
                    EditorState.Saturation = 130;
                    break;
                case "blue":
                    EditorState.Brightness = 90;
                    EditorState.Contrast = 120;
                    EditorState.Saturation = 180;
                    break;
                case "red":
                    EditorState.Brightness = 110;
                    EditorState.Contrast = 120;
                    EditorState.Saturation = 200;
                    break;
            }
            EditorState.AppliedFilter = filterName;
            _ = SaveEditorState();
        }
    }
}