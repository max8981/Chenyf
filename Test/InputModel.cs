using System;
using SixLabors.ImageSharp;

namespace Test
{
    public class InputModel
    {
        public InputModel()
        {
        }
        private int Width => DrawHelper.MonthDifference(StartDate,EndDate) *40;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string ItemType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public Image TitleImage => DrawHelper.GetImage(Title, Color.White, Color.DodgerBlue,Width,SixLabors.Fonts.TextAlignment.Center);
        public Image ContentImage => DrawHelper.GetImage(Content, Color.Black, Color.AliceBlue, Width, SixLabors.Fonts.TextAlignment.Start,14);
        public Image RemarkImage => DrawHelper.GetImage(Remark, Color.White, Color.DodgerBlue, Width);
        public Image Image => DrawHelper.Merge(new Image[] { TitleImage, ContentImage, RemarkImage });
        public Rectangle Rectangle { get; set; }
    }
}

