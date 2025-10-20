using System.Collections.Generic;
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
    }
}