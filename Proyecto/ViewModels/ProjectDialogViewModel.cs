using System;
using Proyecto.Models;

namespace Proyecto.ViewModels
{
    public class ProjectDialogViewModel
    {
        public Project CurrentProject { get; set; }
        public bool IsEditMode { get; set; }
        public string ImagePreview { get; set; }

        public event Action<Project> OnProjectCreated;
        public event Action<Project> OnProjectUpdated;
        public event Action OnCancel;

        public ProjectDialogViewModel()
        {
            CurrentProject = new Project();
        }

        // Inicializar para crear nuevo proyecto
        public void InitializeForCreate()
        {
            IsEditMode = false;
            CurrentProject = new Project();
            ImagePreview = string.Empty;
        }

        // Inicializar para editar proyecto existente
        public void InitializeForEdit(Project project)
        {
            IsEditMode = true;
            CurrentProject = project;
            ImagePreview = project.ImagePath; 
        }

        // Manejar imagen seleccionada por el usuario
        public void SetImage(string imageDataUrl)
        {
            CurrentProject.ImagePath = imageDataUrl;
            ImagePreview = imageDataUrl;
            CurrentProject.Thumbnail = imageDataUrl;
        }

        // Confirmar creación/edición
        public void Confirm()
        {
            if (IsEditMode)
            {
                OnProjectUpdated?.Invoke(CurrentProject);
            }
            else
            {
                OnProjectCreated?.Invoke(CurrentProject);
            }
        }
        public void Cancel()
        {
            OnCancel?.Invoke();
        }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(CurrentProject.Name) &&
                   !string.IsNullOrEmpty(CurrentProject.ImagePath);
        }
    }
}