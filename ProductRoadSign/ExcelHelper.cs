using System;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ProductRoadSign
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {
        }
        private static IWorkbook? workbook;
        public static Excel Input_xls(Stream file)
        {
            workbook = new HSSFWorkbook(file);
            return Input(workbook);
        }
        public static Excel Input_xlsx(Stream file)
        {
            workbook = new XSSFWorkbook(file);
            return Input(workbook);
        }
        private static Excel Input(IWorkbook workbook)
        {
            var sheet = workbook.GetSheetAt(0);
            Dictionary<int,string> catalog = new();
            var a = sheet.GetRow(0).ToDictionary(_=>_.ColumnIndex,_=>_.StringCellValue);
            var excel = new Excel
            {
                Rows = new List<Row>(),
                Columns = a.Select(_ => new Column { Name = _.Value,Value=new List<Cell>() }).ToList(),
            };


            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var r = new Row
                {
                    Index = i,
                    Value = new List<Cell>(),
                };
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    var c = row.GetCell(j);
                    if (c != null)
                    {
                        var cell = new Cell
                        {
                            Name = a[j],
                            Value = c.CellType == CellType.Numeric?c.NumericCellValue.ToString():c.ToString(),
                            Index = i,
                            Row = r,
                            Column = excel.Columns[j],
                        };
                        r.Value.Add(cell);
                        excel.Columns[j].Value.Add(cell);
                    }
                }
                excel.Rows.Add(r);
            }
            return excel;
        }
        public class Excel
        {
            public string this[string colName, int row] { get
                {
                    var result = string.Empty;
                    if (Columns.Any(_ => _.Name == colName))
                    {
                        var column = Columns.First(_ => _.Name == colName);
                        if (row < column.Value.Count)
                        {
                            result = column.Value[row].Value;
                        }
                    }
                    return result;
                } }
            public List<Row> Rows { get; set; }
            public List<Column> Columns { get; set; }
            public int Count => Rows.Count;
        }
        public struct Row
        {
            public int Index { get; set; }
            public List<Cell> Value { get; set; } 
        }
        public struct Column
        {
            public string Name { get; set; }
            public List<Cell> Value { get; set; }
        }
        public struct Cell
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public Column Column { get; set; }
            public Row Row { get; set; }
        }
    }
}

