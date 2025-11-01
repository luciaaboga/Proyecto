namespace Proyecto.Models
{
    public class StickerElement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty; 
        public double X { get; set; }
        public double Y { get; set; }
        public double ScaleX { get; set; } = 1.0; 
        public double ScaleY { get; set; } = 1.0;
        public double Rotation { get; set; } = 0;
        public bool IsDragging { get; set; } = false;
    }
}