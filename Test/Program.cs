// See https://aka.ms/new-console-template for more information
using ProductRoadSign;
using SixLabors.ImageSharp;
using Test;

Console.WriteLine("Hello, World!");
FileStream fs = new FileStream(@"/Users/maxin/Downloads/工作簿1.xlsx", FileMode.Open);
var excel = ProductRoadSign.ExcelHelper.Input(fs);
var v = excel["Chinese", 7];
var a = Color.TryParseHex("#000000", out Color color);
var pp = new ProductProcessor();
var product = new ProductRoadSign.Product()
{
    Title = "名称：\tyinoxinxi",
    Contents=new string[] {
        "主分机：\t主机",
        "型号：\tzcj96_02",
        "启用时间：\t20181108000000",
        "电子发票标识：\t非电子发票",
        "风险预警级别：\t无",
        "events { }\nhttp {\n    server {\n        listen 80;\n\n        location / {\n            root      /usr/share/nginx/html;\n            try_files $uri $uri/ /index.html =404;\n        }\n    }\n}"
    },
    Subscript= "设置“ContentType”并选择“保存”按钮 。",
    StartDate=new DateOnly(2022,12,1),
    EndDate=new DateOnly(2025,1,1),
};
var testdata = new Product[]
{
    new Product
    {
        ProductType="类目1",
        Title="GL20 V1R1",
        Contents=new string[]{
            "浏览器发出请求。",
            "返回默认页，通常为 index.html。",
            "index.html 启动应用。",
            "Blazor 的路由器进行加载，然后呈现 RazorMain 组件。",
        },
        Subscript="About.razor：About 组件。",
        StartDate=new DateOnly(2020,4,1),
        EndDate=new DateOnly(2021,4,1),
    },
    new Product
    {
        ProductType="类目1",
        Title="GL20 V1R2",
        Contents=new string[]{
            "浏览器发出请求。",
            "返回默认页，通常为 index.html。",
            "index.html 启动应用。",
            "Blazor 的路由器进行加载，然后呈现 RazorMain 组件。",
        },
        Subscript="About.razor：About 组件。",
        StartDate=new DateOnly(2020,6,1),
        EndDate=new DateOnly(2020,10,1),
    },
    new Product
    {
        ProductType="类目2",
        Title="GL20 V1R3",
        Contents=new string[]{
            "浏览器发出请求。",
            "返回默认页，通常为 index.html。",
            "index.html 启动应用。",
            "Blazor 的路由器进行加载，然后呈现 RazorMain 组件。",
        },
        Subscript="About.razor：About 组件。",
        StartDate=new DateOnly(2019,11,1),
        EndDate=new DateOnly(2020,9,1),
    },
    new Product
    {
        ProductType="类目3",
        Title="GL20 V1R4",
        Contents=new string[]{
            "浏览器发出请求。",
            "返回默认页，通常为 index.html。",
            "index.html 启动应用。",
            "Blazor 的路由器进行加载，然后呈现 RazorMain 组件。",
        },
        Subscript="About.razor：About 组件。",
        StartDate=new DateOnly(2020,10,1),
        EndDate=new DateOnly(2021,2,1),
    },
    new Product
    {
        ProductType="类目3",
        Title="GL20 V1R5",
        Contents=new string[]{
            "浏览器发出请求。",
            "返回默认页，通常为 index.html。",
            "index.html 启动应用。",
            "Blazor 的路由器进行加载，然后呈现 RazorMain 组件。",
        },
        Subscript="About.razor：About 组件。",
        StartDate=new DateOnly(2021,2,1),
        EndDate=new DateOnly(2021,8,1),
    }
};
foreach (var item in testdata)
{
    pp.AddProduct(item);
}
pp.Draw().SaveAsPng("Draw.png");
Console.WriteLine("do!");