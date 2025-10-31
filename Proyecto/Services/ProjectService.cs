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

        // MÉTODOS ASÍNCRONOS NUEVOS
        public async Task<Project?> GetProjectAsync(Guid projectId)
        {
            await Task.Delay(10);
            return _projects.FirstOrDefault(p => p.Id == projectId);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await Task.Delay(10);
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

        // MÉTODOS PARA FILTROS
        public async Task ApplyFiltersAsync(Guid projectId, int brightness, int contrast, int saturation, int rotation, bool flipH, bool flipV)
        {
            await Task.Delay(10);
            var project = _projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                project.Brightness = brightness;
                project.Contrast = contrast;
                project.Saturation = saturation;
                project.Rotation = rotation;
                project.FlipHorizontal = flipH;
                project.FlipVertical = flipV;
                project.LastModified = DateTime.Now;
                SaveProjects();
            }
        }

        public async Task ResetFiltersAsync(Guid projectId)
        {
            await Task.Delay(10);
            var project = _projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                project.Brightness = 100;
                project.Contrast = 100;
                project.Saturation = 100;
                project.Rotation = 0;
                project.FlipHorizontal = false;
                project.FlipVertical = false;
                project.LastModified = DateTime.Now;
                SaveProjects();
            }
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