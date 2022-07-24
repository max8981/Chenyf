using System;
using NPOI.SS.Formula.Functions;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ProductRoadSign
{
    public class Product
    {
        public Product()
        {
            ProductType = "";
            Title = "";
            Contents = new string[]{ };
            Subscript = "";
        }
        public string ProductType { get; set; } 
        public string Title { get; set; }
        public string[] Contents { get; set; }
        public string Subscript { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool Verify => EndDate > StartDate;
        private int MonthSpan=> Math.Abs((EndDate.Year - StartDate.Year) * 12 + (EndDate.Month - StartDate.Month));
        private int Width => (MonthSpan * Config.MonthPixel) - (Config.Margins * 2);
        public Image Image => Draw();
        public Image Draw()
        {
            Image image = new Image<Rgba32>(Width, 1);
            var top = new DrawHelper.Text(Title,Config.TopColor, Config.TopFont, Config.TopSize);
            image = image.BottomExpand(top.ToImage(Width, true).SetBackGroundColorl(Config.TopBackGroundColorl), 0, 0);
            foreach (var item in Contents)
            {
                var content= new DrawHelper.Text(item, Config.CenterColor, Config.CenterFont, Config.CenterSize);
                image = image.BottomExpand(content.ToImage(Width).SetBackGroundColorl(Config.CenterBackGroundColorl), 0, 0);
            }
            var bottom= new DrawHelper.Text(Subscript, Config.BottomColor, Config.BottomFont,Config.BottomSize);
            image = image.BottomExpand(bottom.ToImage(Width).SetBackGroundColorl(Config.BottomBackGroundColorl), 0, 0);
            image = image.SetMargins(Config.Margins);
            using Image result = image.CloneAs<Rgba32>();
            image.Dispose();
            return result.CloneAs<Rgba32>();
        }
        public string Preview()
        {
            string strbaser64 = "";
            try
            {
                using MemoryStream ms = new MemoryStream();
                Image.SaveAsPng(ms);
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
        ~Product()
        {
            Image.Dispose();
        }
    }
}

