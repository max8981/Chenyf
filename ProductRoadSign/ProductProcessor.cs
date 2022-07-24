using System;
using NPOI.SS.Formula.Functions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ProductRoadSign
{
    public class ProductProcessor
    {
        public ProductProcessor()
        {
        }
        public ProductProcessor(ExcelHelper.Excel excel)
        {
            for (int i = 1; i < excel.Count; i++)
            {
                var top = Config.TopSource.Select(_ => excel[_, i]).ToArray();
                var center = Config.CenterSource.Select(_ => excel[_, i]).Where(_=>!string.IsNullOrEmpty(_)).ToArray();
                var bottom = Config.BottomSource.Select(_ => excel[_, i]).ToArray();
                var start = excel[Config.StartSource, i];
                var end = excel[Config.EndSource, i];
                var type = excel[Config.TypeSource, i];
                if(double.TryParse(start,out var d1)&&double.TryParse(end,out var d2))
                {
                    var product = new Product
                    {
                        ProductType = type,
                        Title = string.Join(" ", top),
                        Contents = center,
                        Subscript = string.Join(" ", bottom),
                        StartDate = DateOnly.FromDateTime(DateTime.FromOADate(d1)),
                        EndDate = DateOnly.FromDateTime(DateTime.FromOADate(d2)),
                    };
                    if (product.Verify)
                        products.Add(product);
                }
            }
        }
        public ProductProcessor(IEnumerable<Product> products)
        {
            this.products = products.Where(_ => _.Verify).ToArray();
        }
        public static IEnumerable<Product> FromExcel(ExcelHelper.Excel excel)
        {
            List<Product> result = new();
            for (int i = 1; i < excel.Count; i++)
            {
                var top = Config.TopSource.Select(_ => excel[_, i]).ToArray();
                var center = Config.CenterSource.Select(_ => excel[_, i]).Where(_ => !string.IsNullOrEmpty(_)).ToArray();
                var bottom = Config.BottomSource.Select(_ => excel[_, i]).ToArray();
                var start = excel[Config.StartSource, i];
                var end = excel[Config.EndSource, i];
                var type = excel[Config.TypeSource, i];
                if (double.TryParse(start, out var d1) && double.TryParse(end, out var d2))
                {
                    var product = new Product
                    {
                        ProductType = type,
                        Title = string.Join(" ", top),
                        Contents = center,
                        Subscript = string.Join(" ", bottom),
                        StartDate = DateOnly.FromDateTime(DateTime.FromOADate(d1)),
                        EndDate = DateOnly.FromDateTime(DateTime.FromOADate(d2)),
                    };
                    result.Add(product);
                }
            }
            return result;
        }
        private Image image;
        private readonly ICollection<Product> products = new List<Product>();
        private readonly Dictionary<int, int> yearLine = new();
        private DateOnly min => products.Min(_ => _.StartDate);
        private DateOnly max => products.Max(_ => _.EndDate);
        public void AddProduct(Product product)
        {
            products.Add(product);
        }
        private Image DrawTitle()
        {
            var count = MonthDifference(min, max) + 1;
            Image month = new Image<Rgba32>(1, 1);
            var y = min.Year;
            for (int i = 0; i < count; i++)
            {
                var date = min.AddMonths(i);
                var monthText = new DrawHelper.Text(date.Month.ToString(),
                    Config.RulerColor, Config.RulerFont, Config.RulerSize);
                month = month.RightExpand(monthText.ToImage(Config.MonthPixel, true), 0, 0);
                yearLine[date.Year] = (i + 1) * Config.MonthPixel;
            }
            Image year = new Image<Rgba32>(1, 1);
            int lastPixel = 0;
            foreach (var item in yearLine)
            {
                var yearText = new DrawHelper.Text(item.Key.ToString(),
                    Config.RulerColor, Config.RulerFont, Config.RulerSize);
                year = year.RightExpand(yearText.ToImage(item.Value- lastPixel, true), 0, 0);
                lastPixel = item.Value;
            }
            using Image result = year.BottomExpand(month, 0, 0);
            year.Dispose();
            month.Dispose();
            return result.CloneAs<Rgba32>();
        }
        private Image DrawCentent(IEnumerable<Product> products)
        {
            var list = products.OrderByDescending(_ => _.EndDate).ToArray();
            Dictionary<Image, Rectangle> dic = new();
            for (int i = 0; i < list.Length; i++)
            {
                var x = MonthDifference(list[i].StartDate, min) * Config.MonthPixel;
                var end = MonthDifference(list[i].EndDate, min);
                var image = list[i].Image;
                var rect = new Rectangle(x, 0, image.Width, image.Height);
                var point = GetPoint(rect, dic.Values.ToList());
                rect.Y = point.Y;
                dic.Add(image, rect);
            };
            var pen = Pens.Dash(Color.LightGray, Config.SplitLine);
            pen.JointStyle = SixLabors.ImageSharp.Drawing.JointStyle.Miter;
            int width = yearLine.Values.Max();
            int height = dic.Values.Max(_ => _.Bottom);
            Image result = new Image<Rgba32>(width, height);
            foreach (var item in yearLine)
            {
                result.Mutate(_ => _.DrawLines(pen, new PointF[] { new Point(item.Value, 0), new Point(item.Value, height) }));
            }
            foreach (var item in dic)
            {
                result.Mutate(_ => _.DrawImage(item.Key, new Point(item.Value.X, item.Value.Y), 1));
            }
            return result;
        }
        private Image DrawBackGround()
        {
            var list = products.OrderByDescending(_ => _.EndDate).ToArray();
            Dictionary<Image, Rectangle> dic = new();
            for (int i = 0; i < list.Length; i++)
            {
                var x = MonthDifference(list[i].StartDate, min) * Config.MonthPixel;
                var end = MonthDifference(list[i].EndDate, min);
                var image = list[i].Image;
                var rect = new Rectangle(x, 0, image.Width, image.Height);
                var point = GetPoint(rect, dic.Values.ToList());
                rect.Y = point.Y;
                dic.Add(image, rect);
            };
            var pen = Pens.Dash(Color.LightGray, 1);
            pen.JointStyle = SixLabors.ImageSharp.Drawing.JointStyle.Miter;
            int width = yearLine.Values.Max();
            int height = dic.Values.Max(_ => _.Bottom);
            Image result = new Image<Rgba32>(width, height);
            foreach (var item in yearLine)
            {
                result.Mutate(_ => _.DrawLines(pen, new PointF[] { new Point(item.Value, 0), new Point(item.Value, height) }));
            }
            foreach (var item in dic)
            {
                result.Mutate(_ => _.DrawImage(item.Key, new Point(item.Value.X, item.Value.Y), 1));
            }
            return result;
        }
        public Image Draw()
        {
            var datas = products.GroupBy(_ => _.ProductType).ToDictionary(_=>_.Key,_=>_.ToArray());
            image = DrawTitle();
            foreach (var item in Config.ProductTypes)
            {
                if(datas.TryGetValue(item,out var products))
                {
                    image = image.BottomExpand(DrawCentent(products), 0, 0);
                }
            }
            return image.CloneAs<Rgba32>();
        }
        private static int MonthDifference(DateOnly d1, DateOnly d2)
        {
            return Math.Abs((d2.Year - d1.Year) * 12 + (d2.Month - d1.Month));
        }
        private static Point GetPoint(Rectangle rectangle, List<Rectangle> rectangles)
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
        public async Task<string> Preview()
        {
            string strbaser64 = "";
            try
            {
                using MemoryStream ms = new MemoryStream();
                await Draw().SaveAsPngAsync(ms);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }
        ~ProductProcessor()
        {
            image.Dispose();
        }
    }
}

