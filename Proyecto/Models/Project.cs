using System;

namespace Proyecto.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public string Thumbnail { get; set; } = string.Empty;
        
        // Nuevas propiedades para filtros
        public int Brightness { get; set; } = 100;
        public int Contrast { get; set; } = 100;
        public int Saturation { get; set; } = 100;
        public int Rotation { get; set; } = 0;
        public bool FlipHorizontal { get; set; } = false;
        public bool FlipVertical { get; set; } = false;
        public string OriginalImagePath { get; set; } = string.Empty;
    }
}