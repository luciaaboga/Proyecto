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

        // Eliminar proyecto
        public void DeleteProject(Project project)
        {
            _projectService.DeleteProject(project.Id);
            LoadProjects();
        }
    }
}