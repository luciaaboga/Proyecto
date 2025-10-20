using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Contracts;
using Proyecto.Models;

namespace Proyecto.Services
{
    public class ProjectService : IProjectService
    {
        private List<Project> _projects = new List<Project>();
        private readonly ILocalStorageService _localStorage;
        private const string StorageKey = "photoEditorProjects";

        public ProjectService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _ = LoadProjectsAsync(); 
        }

        public List<Project> GetAllProjects()
        {
            return _projects.OrderByDescending(p => p.LastModified).ToList();
        }

        public void CreateProject(Project project)
        {
            _projects.Add(project);
            SaveProjects();
        }

        public void UpdateProject(Project project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.ImagePath = project.ImagePath;
                existingProject.Thumbnail = project.Thumbnail;
                existingProject.LastModified = DateTime.Now;
                SaveProjects();
            }
        }

        public void DeleteProject(Guid projectId)
        {
            var project = _projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                _projects.Remove(project);
                SaveProjects();
            }
        }

        public Project GetProjectById(Guid projectId)
        {
            return _projects.FirstOrDefault(p => p.Id == projectId);
        }

        public async void SaveProjects()
        {
            await _localStorage.SetItemAsync(StorageKey, _projects);
        }

        private async Task LoadProjectsAsync()
        {
            var projects = await _localStorage.GetItemAsync<List<Project>>(StorageKey);
            if (projects != null)
            {
                _projects = projects;
            }
        }
    }
}