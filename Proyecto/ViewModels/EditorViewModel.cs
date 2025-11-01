using Proyecto.Contracts;
using Proyecto.Models;

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
                // Sincronizar estado del editor con el proyecto
                EditorState.Brightness = CurrentProject.Brightness;
                EditorState.Contrast = CurrentProject.Contrast;
                EditorState.Saturation = CurrentProject.Saturation;
                EditorState.Rotation = CurrentProject.Rotation;
                EditorState.FlipHorizontal = CurrentProject.FlipHorizontal;
                EditorState.FlipVertical = CurrentProject.FlipVertical;
            }
        }

        public async Task ApplyFiltersAsync()
        {
            if (CurrentProject != null)
            {
                await _editorService.ApplyFiltersAsync(CurrentProject.Id, EditorState);
            }
        }

        public async Task ResetFiltersAsync()
        {
            EditorState = new EditorState();
            if (CurrentProject != null)
            {
                await _editorService.ResetFiltersAsync(CurrentProject.Id);
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
        public void SetBrightness(int value) => EditorState.Brightness = value;
        public void SetContrast(int value) => EditorState.Contrast = value;
        public void SetSaturation(int value) => EditorState.Saturation = value;
        public void SetRotation(int value) => EditorState.Rotation = value;
        public void ToggleFlipHorizontal() => EditorState.FlipHorizontal = !EditorState.FlipHorizontal;
        public void ToggleFlipVertical() => EditorState.FlipVertical = !EditorState.FlipVertical;

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
                    EditorState.Brightness = 110;
                    EditorState.Contrast = 90;
                    EditorState.Saturation = 85;
                    break;
                case "bw":
                    EditorState.Brightness = 100;
                    EditorState.Contrast = 120;
                    EditorState.Saturation = 0;
                    break;
                case "warm":
                    EditorState.Brightness = 105;
                    EditorState.Contrast = 95;
                    EditorState.Saturation = 120;
                    break;
                case "cool":
                    EditorState.Brightness = 95;
                    EditorState.Contrast = 110;
                    EditorState.Saturation = 80;
                    break;
                case "blue":
                    EditorState.Brightness = 95;
                    EditorState.Contrast = 105;
                    EditorState.Saturation = 110;
                    break;
                case "red":
                    EditorState.Brightness = 105;
                    EditorState.Contrast = 100;
                    EditorState.Saturation = 130;
                    break;
            }
            EditorState.AppliedFilter = filterName;
        }
    }
}