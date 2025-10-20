using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Proyecto.Contracts;
using Proyecto.Models;

namespace Proyecto.ViewModels
{
    public class HomeViewModel
    {
        private readonly IProjectService _projectService;
        public ObservableCollection<Project> Projects { get; set; }
        public Project SelectedProject { get; set; }
        public bool ShowProjectDialog { get; set; }
        public bool IsEditMode { get; set; }

        public HomeViewModel(IProjectService projectService)
        {
            _projectService = projectService;
            Projects = new ObservableCollection<Project>();
            LoadProjects();
        }
        public void LoadProjects()
        {
            Projects.Clear();
            var projects = _projectService.GetAllProjects();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }

        // Abrir modal para crear nuevo proyecto
        public void ShowCreateDialog()
        {
            IsEditMode = false;
            SelectedProject = new Project(); 
            ShowProjectDialog = true;
        }

        // Abrir modal para editar proyecto existente
        public void ShowEditDialog(Project project)
        {
            IsEditMode = true;
            SelectedProject = project;
            ShowProjectDialog = true;
        }

        // Cerrar modal
        public void CloseDialog()
        {
            ShowProjectDialog = false;
            SelectedProject = null;
        }

        // Crear nuevo proyecto
        public void CreateProject(Project project)
        {
            _projectService.CreateProject(project);
            LoadProjects();
            CloseDialog();
        }

        // Actualizar proyecto existente
        public void UpdateProject(Project project)
        {
            _projectService.UpdateProject(project);
            LoadProjects(); 
            CloseDialog();
        }

        // Eliminar proyecto
        public void DeleteProject(Project project)
        {
            _projectService.DeleteProject(project.Id);
            LoadProjects();     
        }
    }
}