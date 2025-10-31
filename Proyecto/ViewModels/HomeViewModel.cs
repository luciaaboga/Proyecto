using Proyecto.Contracts;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto.ViewModels
{
    public class HomeViewModel
    {
        private readonly IProjectService _projectService;

        public HomeViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public List<Project> Projects => _projectService.GetAllProjects();

        public void DeleteProject(Project project)
        {
            _projectService.DeleteProject(project.Id);
        }
    }
}