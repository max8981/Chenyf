using System;
using System.Reflection.Metadata;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ProductRoadSign
{
    public static class DrawHelper
    {
        public static Image ToImage(this Text text, int width, bool center = false)
        {
            text.Options.WrappingLength = width;
            var rect = TextMeasurer.Measure(text.Content, text.Options);
            int height = (int)Math.Ceiling(rect.Height);
            using Image<Rgba32> Result = new Image<Rgba32>(width, height);
            int x = center ? x = (int)((width - rect.Width) / 2) : 0;
            text.Options.Origin = new System.Numerics.Vector2(x, 0);
            Result.Mutate(_ => _.DrawText(text.Options, text.Content, text.Color));
            return Result.Clone();
        }
        public static Image SetMargins(this Image image,int margins)
        {
            int width = image.Width + margins + margins;
            int height = image.Height + margins + margins;
            Point point = new Point(margins, margins);
            using Image<Rgba32> result = new Image<Rgba32>(width,height);
            result.Mutate(_ => _.DrawImage(image, point, 1));
            return result.Clone();
        }
        public static Image SetBackGroundColorl(this Image image,string color)
        {
            image.Mutate(_ => _.BackgroundColor(Color.Parse(color)));
            return image;
        }

        public static Image CenterExpand(this Image source,Image image,int x,int y)
        {
            int width = Math.Max(source.Width, image.Width + x);
            int height = Math.Max(source.Height, image.Height + y);
            using Image<Rgba32> result = new(width, height);
            result.Mutate(_ => _.DrawImage(source, new Point(0, 0), 1));
            result.Mutate(_ => _.DrawImage(image, new Point(x, y), 1));
            return result.Clone();
        }
        public static Image BottomExpand(this Image source,Image image,int x,int y)
        {
            int width = Math.Max(source.Width, image.Width + x);
            int height = source.Height + image.Height + y;
            using Image<Rgba32> result = new(width, height);
            result.Mutate(_ => _.DrawImage(source, new Point(0, 0), 1));
            result.Mutate(_ => _.DrawImage(image, new Point(x, source.Height + y), 1));
            return result.Clone();
        }
        public static Image RightExpand(this Image source,Image image,int x,int y)
        {
            int width = source.Width + image.Width + x;
            int height = Math.Max(source.Height, image.Height + y);
            using Image<Rgba32> result = new(width, height);
            result.Mutate(_ => _.DrawImage(source, new Point(0, 0), 1));
            result.Mutate(_ => _.DrawImage(image, new Point(source.Width + x, y), 1));
            return result.Clone();
        }

        public struct Text
        {
            public Text(string content, string color, string fontName, int size)
            {
                Content = content;
                Color = Color.Parse(color);
                Font font = SystemFonts.CreateFont(fontName, size);
                Options = new TextOptions(font)
                {
                    TextAlignment = TextAlignment.Start,
                    WordBreaking = WordBreaking.BreakAll,
                    ApplyHinting = true,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
            }
            public string Content { get; set; }
            public Color Color { get; set; }
            public TextOptions Options { get; }
        }
    }
}

