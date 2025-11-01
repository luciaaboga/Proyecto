namespace Proyecto.Models
{
    public class EditorState
    {
        public int Brightness { get; set; } = 100;
        public int Contrast { get; set; } = 100;
        public int Saturation { get; set; } = 100;
        public int Rotation { get; set; } = 0;
        public bool FlipHorizontal { get; set; } = false;
        public bool FlipVertical { get; set; } = false;
        public string AppliedFilter { get; set; } = "normal";
        public List<EditorElement> Elements { get; set; } = new List<EditorElement>();
        public List<StickerElement> Stickers { get; set; } = new List<StickerElement>();
        public List<TextElement> TextElements { get; set; } = new List<TextElement>();
        public bool IsCropping { get; set; } = false;
        public CropArea CropArea { get; set; } = new CropArea();
    }

    public class EditorElement
    {
        public string Type { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty; 
        public double X { get; set; }
        public double Y { get; set; }
        public double Scale { get; set; } = 1.0;
        public double Rotation { get; set; } = 0;
    }

    public class TextElement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = string.Empty;
        public string Color { get; set; } = "#000000";
        public string FontFamily { get; set; } = "Arial";
        public int FontSize { get; set; } = 24;
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public double X { get; set; }
        public double Y { get; set; }
        public double ScaleX { get; set; } = 1.0;
        public double ScaleY { get; set; } = 1.0;
        public double Rotation { get; set; } = 0;
        public bool IsDragging { get; set; } = false;
    }

    public class CropArea
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double Width { get; set; } = 100;
        public double Height { get; set; } = 100;
        public bool IsDragging { get; set; } = false;
        public string DragHandle { get; set; } = string.Empty;
    }
}