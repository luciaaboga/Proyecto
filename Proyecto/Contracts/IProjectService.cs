using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto.Models;

namespace Proyecto.Contracts
{
    public interface IProjectService
    {
        // Obtener todos los proyectos
        List<Project> GetAllProjects();

        // Crear nuevo proyecto
        void CreateProject(Project project);

        // Actualizar proyecto existente
        void UpdateProject(Project project);

        // Eliminar proyecto
        void DeleteProject(Guid projectId);

        // Obtener proyecto por ID
        Project GetProjectById(Guid projectId);

        // Guardar todos los proyectos explícitamente
        void SaveProjects();

        // Métodos asíncronos para el editor
        Task<Project?> GetProjectAsync(Guid projectId);
        Task UpdateProjectAsync(Project project);
        
        // Nuevos métodos para filtros
        Task ApplyFiltersAsync(Guid projectId, int brightness, int contrast, int saturation, int rotation, bool flipH, bool flipV);
        Task ResetFiltersAsync(Guid projectId);
    }
}