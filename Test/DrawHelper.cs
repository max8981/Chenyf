using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Point = SixLabors.ImageSharp.Point;

namespace Test
{
    public class DrawHelper
    {
        public DrawHelper()
        {

        }
        public static void Test()
        {
            var a = SystemFonts.Collection;
            var b = SystemFonts.Families;
            Font font = SystemFonts.CreateFont("Heiti SC", 12);
        }
        static int multiple = 40;
        static int space = 20;
        static FontCollection collection = new();
        static FontFamily family = collection.Add("/Users/maxin/Projects/Chenyf/Test/Font/msyh.ttf");
        static Font font = family.CreateFont(18,FontStyle.Italic);
        public static Image GetImage(string s,Color color,Color backgroundcolorl,int width,TextAlignment alignment=TextAlignment.Start, int size = 18)
        {
            int mixwidth = width - space;
            font = family.CreateFont(size, FontStyle.Italic);
            var textOptions = new TextOptions(font)
            {
                TextAlignment = TextAlignment.Start,
                WordBreaking = WordBreaking.BreakAll,
                WrappingLength = mixwidth,
                ApplyHinting = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            var tm = TextMeasurer.Measure(s, textOptions);
            switch (alignment)
            {
                case TextAlignment.Center: {
                        var w = (int)Math.Ceiling(tm.Width);
                        var x = (mixwidth - w) / 2;
                        textOptions.Origin = new System.Numerics.Vector2(x, 0);
                    };break;
            }
            var height = (int)Math.Ceiling(tm.Height);
            Image<Rgba32> result = new Image<Rgba32>(width, height);
            using Image<Rgba32> image = new Image<Rgba32>(mixwidth, height);
            image.Mutate(_ => _.BackgroundColor(backgroundcolorl));
            image.Mutate(_ => _.DrawText(textOptions, s, color));
            result.Mutate(_ => _.DrawImage(image, new Point(space, 0), 1));
            return result;
        }
        private static Image GetImage(string s,int width)
        {
            var textOptions = new TextOptions(font)
            {
                TextAlignment = TextAlignment.Start,
                WordBreaking = WordBreaking.BreakAll,
                WrappingLength = width,
                ApplyHinting = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            var tm = TextMeasurer.Measure(s, textOptions);
            var w = (int)Math.Ceiling(tm.Width);
            var x = (width - w) / 2;
            textOptions.Origin = new System.Numerics.Vector2(x, 0);
            var height = (int)Math.Ceiling(tm.Height);
            Image<Rgba32> result = new Image<Rgba32>(width, height);
            result.Mutate(_ => _.DrawText(textOptions, s, Color.Black));
            return result;
        }
        public static Image Merge(Image[] images)
        {
            Image<Rgba32> result = new Image<Rgba32>(images[0].Width, images.Sum(_ => _.Height) + space);
            int row = space;
            for (int i = 0; i < images.Length; i++)
            {
                result.Mutate(_ => _.DrawImage(images[i],new Point(0,row),1));
                row += images[i].Height;
            }
            result.Save("output.png");
            return result;
        }
        public static Image Out(InputModel[] models)
        {
            var min = models.Min(_ => _.StartDate);
            var max = models.Max(_ => _.EndDate);
            var datescale = MonthDifference(min, max);
            var group = models.GroupBy(_ => _.ItemType).ToArray();
            var width = datescale * multiple;
            var t = DrawTicks(min, max);
            Image<Rgba32> result = new Image<Rgba32>(width, t.Height);
            int row = t.Height;
            var list = new List<Image>();
            foreach (var item in group)
            {
                var image = DrawCategory(item.ToArray(), min, width);
                list.Add(image);
                result.Mutate(_ => _.Resize(width, result.Height + image.Height));
            }
            foreach (var item in list)
            {
                result.Mutate(_ => _.DrawImage(item, new Point(0, row), 1));
                row += item.Height;
            }
            result.Mutate(_ => _.DrawImage(t, new Point(0, 0), 1));
            result.Save("Out.png");
            return result;
        }
        public static Image DrawCategory(InputModel[] models,DateOnly min,int width)
        {
            Image<Rgba32> result = new Image<Rgba32>(width, 1);
            var rects = new List<Rectangle>();
            var list = models.OrderByDescending(_ => _.EndDate).ToArray();
            for (int i = 0; i < list.Count(); i++)
            {
                var x = MonthDifference(list[i].StartDate, min) * multiple;
                var end = MonthDifference(list[i].EndDate, min);
                var image = list[i].Image;
                var rect = new Rectangle(x, 0, image.Width, image.Height);
                var point = GetPoint(rect, rects);
                rect.Y = point.Y;
                list[i].Rectangle = rect;
                if (rect.Bottom > result.Height)
                {
                    result.Mutate(_ => _.Resize(width, result.Height + rect.Height));
                }
                rects.Add(rect);
            };
            foreach (var item in list)
            {
                result.Mutate(_ => _.DrawImage(item.Image, new Point(item.Rectangle.X, item.Rectangle.Y), 1));
            }
            result.Save("DrawCategory.png");
            return result;
        }
        
        public static int MonthDifference(DateOnly d1,DateOnly d2)
        {
            return Math.Abs((d2.Year - d1.Year) * 12 + (d2.Month - d1.Month));
        }
        private static Image DrawTicks(DateOnly d1,DateOnly d2)
        {
            var count = MonthDifference(d1, d2);
            var months = new List<Image>();
            var years = new List<Image>();
            var width = 0;
            var height1 = 0;
            var height2 = 0;
            var year = d1.Year;
            for (int i = 0; i < count; i++)
            {
                var d = d1.AddMonths(i);
                var image = GetImage(d.Month.ToString(), multiple);
                height1 = image.Height;
                if (year == d.Year)
                {
                    width += multiple;
                }
                else
                {
                    var y = GetImage(d.Year.ToString(), width);
                    year = d.Year;
                    years.Add(y);
                    height2 = y.Height;
                    width = 0;
                }
                months.Add(image);
            }
            if (width > 0)
            {
                var y = GetImage((year + 1).ToString(), width);
                years.Add(y);
                height2 = y.Height;
            }
            var result = new Image<Rgba32>(count * multiple, height1 + height2);
            var lastwidth = 0;
            for (int i = 0; i < years.Count; i++)
            {
                result.Mutate(_ => _.DrawImage(years[i], new Point(lastwidth, 0), 1));
                lastwidth += years[i].Width;
            }
            lastwidth = 0;
            for (int i = 0; i < months.Count; i++)
            {
                result.Mutate(_ => _.DrawImage(months[i], new Point(lastwidth, height2), 1));
                lastwidth += months[i].Width;
            }
            result.Save("DrawTicks.png");
            return result;
        }
        private static Point GetPoint(Rectangle rectangle,List<Rectangle> rectangles)
        {
            Point result = new(rectangle.X, rectangle.Y);
            for (int i = 0; i < rectangles.Count; i++)
            {
                if (rectangles[i].IntersectsWith(rectangle))
                {
                    result = GetPoint(new Rectangle(rectangle.X, rectangles[i].Bottom, rectangle.Width, rectangle.Height), rectangles);
                }
            }
            return result;
        }
    }
}

