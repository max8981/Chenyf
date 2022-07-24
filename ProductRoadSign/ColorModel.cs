using System;
namespace ProductRoadSign
{
    public struct ColorModel
    {
        public ColorModel(string name, SixLabors.ImageSharp.Color color)
        {
            Color = color;
            Name = name;
            Value = $"#{color.ToHex()}";
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public SixLabors.ImageSharp.Color Color { get; set; }
    }
}

