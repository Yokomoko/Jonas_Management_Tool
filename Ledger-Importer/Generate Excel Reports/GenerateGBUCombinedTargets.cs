using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace Jonas_Sage_Importer.Generate_Excel_Reports {
    class GenerateGBUCombinedTargets {
        /// <summary>
        /// Generates the report.
        /// </summary>
        public static void GenerateReport() {
            using (ExcelPackage p = new ExcelPackage()) {
                //set the workbook properties and add a default sheet in it
                SetWorkbookProperties(p);
                //Create a sheet
                ExcelWorksheet ws = CreateSheet(p, "Template");
                DataTable dt = CreateDataTable(); //My Function which generates DataTable

                var percentageFormat = "0%";
                var currencyFormat = @"_-£* #,##0_-;-£* #,##0_-;_-£* ""-""_-;_-@_-";



                //Set border style
                //Insides first
                ws.Cells[8, 2, 32, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[6, 2, 32, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[14, 4, 16, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[14, 4, 16, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[17, 4, 20, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[17, 4, 20, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[21, 4, 22, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[21, 4, 22, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[28, 2, 29, 4].Style.Border.Right.Style = ExcelBorderStyle.None;
                ws.Cells[23, 2, 27, 4].Style.Border.Right.Style = ExcelBorderStyle.None;
                ws.Cells[23, 2, 27, 4].Style.Border.Bottom.Style = ExcelBorderStyle.None;
                //Medium thickness places
                ws.Cells[6, 2, 32, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws.Cells[6, 2, 6, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[7, 2, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[6, 4, 29, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[13, 2, 13, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[14, 2, 14, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[14, 8, 16, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[16, 7, 16, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[14, 6, 22, 6].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[20, 7, 20, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[21, 8, 22, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[21, 2, 21, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[22, 2, 22, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[22, 4, 32, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[32, 2, 32, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[27, 2, 28, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                //Merging cells and create a center heading for out table
                ws.Cells[3, 2].Value = "Combined EPOS / FM and CCR Key Weekly Figures Status";
                ws.Cells[3, 2, 3, 3].Merge = true;
                ws.Cells[3, 2, 3, 3].Style.Font.Bold = true;
                ws.Cells[3, 2, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(2).Width = 72.25;

                ws.Cells[4, 2].Value = "(We need to all sell to make this happen)";
                ws.Cells[4, 2, 4, 3].Merge = true;
                ws.Cells[4, 2, 4, 3].Style.Font.Bold = true;
                ws.Cells[4, 2, 4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 2].Value = "Sales (NickTB to populate)";
                ws.Cells[6, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[6, 2, 7, 4].Style.Font.Bold = true;
                ws.Cells[6, 3].Value = "Gross";
                ws.Cells[6, 4].Value = "Nett";
                ws.Cells[6, 2, 6, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[6, 2, 6, 4].Style.Fill.BackgroundColor.SetColor(Color.Beige);
                ws.Cells[8, 2].Value = "Total values of Sales Booked Last Week";
                ws.Cells[9, 2].Value = "Running total of sales booked for the month so far (Gross and Net)";
                ws.Cells[10, 2].Value = "Total Gross pipeline value";
                ws.Cells[11, 2].Value = "Total forecast to close this week";
                ws.Cells[12, 2].Value = "Total forecast to close next week";

                ws.Cells[14, 2].Value = "BACKLOG (EPOS Group / FM / CCR)";
                ws.Cells[14, 2].Style.Font.Bold = true;
                ws.Cells[14, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[14, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[14, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Beige);

                var today = DateTime.Today;
                var friday = today.AddDays(-(int)today.DayOfWeek).AddDays(5);

                ws.Cells[14, 3].Value = "Friday " + friday.ToShortDateString();
                ws.Cells[14, 3, 14, 4].Merge = true;
                ws.Cells[14, 3, 14, 4].Style.Font.Bold = true;
                ws.Cells[14, 3, 14, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[14, 5].Value = "Previous Week - " + friday.AddDays(-7).ToString("dd/MM");
                ws.Cells[14, 5, 14, 6].Merge = true;
                ws.Cells[14, 5, 14, 6].Style.Font.Bold = true;
                ws.Cells[14, 5, 14, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[14, 7].Value = "Backlog Growth";
                ws.Cells[14, 7, 14, 8].Merge = true;
                ws.Cells[14, 7, 14, 8].Style.Font.Bold = true;
                ws.Cells[14, 7, 14, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[15, 3].Value = "Gross";
                ws.Cells[15, 4].Value = "Nett";
                ws.Cells[15, 5].Value = "Gross";
                ws.Cells[15, 6].Value = "Nett";
                ws.Cells[15, 7].Value = "Amount";
                ws.Cells[15, 8].Value = "%";

                ws.Cells[16, 2].Value = "Total backlog value (PS+Lic+Hardware)";
                ws.Cells[16, 3, 22, 6].Style.Numberformat.Format = currencyFormat;

                ws.Cells[14, 3, 14, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[16, 3, 17, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[19, 3, 20, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;

                ws.Cells[16, 3, 17, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
                ws.Cells[19, 3, 20, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
                ws.Cells[14, 3, 14, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);

                ws.Cells[16, 7].Formula = "C16 - E16";
                ws.Cells[16, 7].Style.Numberformat.Format = currencyFormat;

                ws.Cells[16, 8].Style.Numberformat.Format = percentageFormat;
                ws.Cells[16, 8].Formula = "G16 / E16";

                ws.Cells[17, 2].Value = "Total Installed (ie invoiced) this week (PS+Lic+Hardware)";
                ws.Cells[19, 2].Value = "Total Backlog booked for this month, not installed (PS+Lic+Hardware)";
                ws.Cells[20, 2].Value = "Running total of Equipment installed so far this month (PS+Lic+Hardware)";

                ws.Cells[21, 7, 21, 8].Merge = true;
                ws.Cells[21, 7].Value = "Difference (Gross)";
                ws.Cells[21, 7, 21, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[21, 7, 21, 8].Style.Font.Bold = true;

                ws.Cells[22, 2].Value = "Predicted Equipment Invoices for the month";
                ws.Cells[22, 2].Style.Font.Bold = true;
                ws.Cells[22, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[22, 2, 22, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[22, 2, 22, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Beige);

                ws.Cells[22, 7].Formula = "C22 - E22";
                ws.Cells[22, 7].Style.Numberformat.Format = currencyFormat;
                ws.Cells[22, 8].Formula = "G22 / E22";
                ws.Cells[22, 8].Style.Numberformat.Format = percentageFormat;


                ws.Cells[23, 2].Value = "Our Monthly Gross Equipment installations directly affect the profit that we make";
                ws.Cells[23, 2].Style.Font.Bold = true;

                ws.Cells[24, 2].Value = "Gross install <£250K is making a loss";
                ws.Cells[24, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[24, 2].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);

                ws.Cells[25, 2].Value = "Gross install £250K to £350 is making an adequate Profit";
                ws.Cells[25, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[25, 2].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                ws.Cells[26, 2].Value = "Gross install >£350K is making exceeding our target";
                ws.Cells[26, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[26, 2].Style.Fill.BackgroundColor.SetColor(Color.Green);

                ws.Cells[28, 2].Value = "AR (SB to populate)";
                ws.Cells[28, 2, 28, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[28, 2, 28, 4].Style.Fill.BackgroundColor.SetColor(Color.Beige);

                ws.Cells[30, 2].Value = "Total AR Value Aged";
                ws.Cells[31, 2].Value = "Cash collected this week";
                ws.Cells[32, 2].Value = "Running Total of Cash Collected this month";

                ws.Cells[10, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[10, 4].Style.Fill.BackgroundColor.SetColor(Color.DimGray);
                ws.Cells[30, 4, 32, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[30, 4, 32, 4].Style.Fill.BackgroundColor.SetColor(Color.DimGray);


                //Calculate calculated fields
                ws.Calculate();
                /*
                var condFormatting = ws.ConditionalFormatting;
                var condFormattingGreen = condFormatting.AddGreaterThan(ws.Cells[16, 7, 16, 8]);
                condFormattingGreen.Formula = "0";
                condFormattingGreen.Style.Fill.PatternType = ExcelFillStyle.Solid;
                condFormattingGreen.Style.Fill.BackgroundColor.Color = Color.DarkSeaGreen;
                var condFormattingNormal = condFormatting.AddEqual(ws.Cells[16, 7, 16, 8]);
                */

                //Populate Common Conditional Formatting
                ExcelFillStyle fsSolid = ExcelFillStyle.Solid;
                var formula = "0";
                Color grn = Color.DarkSeaGreen;
                Color trans = Color.Transparent;
                Color red = Color.IndianRed;


                #region Backlog Growth Formatting
                var condFormattingGreen = ws.ConditionalFormatting.AddGreaterThan(ws.Cells[16, 7, 16, 8]);
                condFormattingGreen.Formula = formula;
                condFormattingGreen.Style.Fill.PatternType = fsSolid;
                condFormattingGreen.Style.Fill.BackgroundColor.Color = grn;

                var condFormattingNormal = ws.ConditionalFormatting.AddEqual(ws.Cells[16, 7, 16, 8]);
                condFormattingNormal.Formula = formula;
                condFormattingNormal.Style.Fill.PatternType = fsSolid;
                condFormattingNormal.Style.Fill.BackgroundColor.Color = trans;

                var condFormattingRed = ws.ConditionalFormatting.AddLessThan(ws.Cells[16, 7, 16, 8]);
                condFormattingRed.Formula = formula;
                condFormattingRed.Style.Fill.PatternType = fsSolid;
                condFormattingRed.Style.Fill.BackgroundColor.Color = red;
                #endregion

                var diffFormattingGrn = ws.ConditionalFormatting.AddGreaterThan(ws.Cells[22, 7, 22, 8]);
                diffFormattingGrn.Formula = formula;
                diffFormattingGrn.Style.Fill.PatternType = fsSolid;
                diffFormattingGrn.Style.Fill.BackgroundColor.Color = grn;

                var diffFormattingNormal = ws.ConditionalFormatting.AddEqual(ws.Cells[22, 7, 22, 8]);
                diffFormattingNormal.Formula = formula;
                diffFormattingNormal.Style.Fill.PatternType = fsSolid;
                diffFormattingNormal.Style.Fill.BackgroundColor.Color = trans;

                var diffFormattingRed = ws.ConditionalFormatting.AddLessThan(ws.Cells[22, 7, 22, 8]);
                diffFormattingRed.Formula = formula;
                diffFormattingRed.Style.Fill.PatternType = fsSolid;
                diffFormattingRed.Style.Fill.BackgroundColor.Color = red;

                //Generate A File Name
                Byte[] bin = p.GetAsByteArray();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string file = $"GBU Report {friday.ToString("yyMMdd")}" + ".xlsx";
                var pathString = System.IO.Path.Combine(path, file);
                //

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.WriteAllBytes(pathString, bin);






                //These lines will open it in Excel
                ProcessStartInfo pi = new ProcessStartInfo(pathString);
                Process.Start(pi);
            }
        }

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName) {
            p.Workbook.Worksheets.Add(sheetName);
            ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

            return ws;
        }

        /// <summary>
        /// Sets the workbook properties and adds a default sheet.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private static void SetWorkbookProperties(ExcelPackage p) {
            //Here setting some document properties
            p.Workbook.Properties.Author = "Zeeshan Umar";
            p.Workbook.Properties.Title = "EPPlus Sample";


        }

        private static void CreateHeader(ExcelWorksheet ws, ref int rowIndex, DataTable dt) {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns) //Creating Headings
            {
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting the background color of header cells to Gray
                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.Gray);

                //Setting Top/left,right/bottom borders.
                var border = cell.Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Setting Value in cell
                cell.Value = "Heading " + dc.ColumnName;

                colIndex++;
            }
        }

        private static void CreateData(ExcelWorksheet ws, ref int rowIndex, DataTable dt) {
            int colIndex = 0;
            foreach (DataRow dr in dt.Rows) // Adding Data into rows
            {
                colIndex = 1;
                rowIndex++;

                foreach (DataColumn dc in dt.Columns) {
                    var cell = ws.Cells[rowIndex, colIndex];

                    //Setting Value in cell
                    cell.Value = Convert.ToInt32(dr[dc.ColumnName]);

                    //Setting borders of cell
                    var border = cell.Style.Border;
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }
            }
        }

        private static void CreateFooter(ExcelWorksheet ws, ref int rowIndex, DataTable dt) {
            int colIndex = 0;
            foreach (DataColumn dc in dt.Columns) //Creating Formula in footers
            {
                colIndex++;
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting Sum Formula
                cell.Formula = "Sum(" + ws.Cells[3, colIndex].Address + ":" + ws.Cells[rowIndex - 1, colIndex].Address + ")";

                //Setting Background fill color to Gray
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            }
        }

        /// <summary>
        /// Adds the custom shape.
        /// </summary>
        /// <param name="ws">Worksheet</param>
        /// <param name="colIndex">Column Index</param>
        /// <param name="rowIndex">Row Index</param>
        /// <param name="shapeStyle">Shape style</param>
        /// <param name="text">Text for the shape</param>
        private static void AddCustomShape(ExcelWorksheet ws, int colIndex, int rowIndex, eShapeStyle shapeStyle, string text) {
            ExcelShape shape = ws.Drawings.AddShape("cs" + rowIndex.ToString() + colIndex.ToString(), shapeStyle);
            shape.From.Column = colIndex;
            shape.From.Row = rowIndex;
            shape.From.ColumnOff = Pixel2MTU(5);
            shape.SetSize(100, 100);
            shape.RichText.Add(text);
        }

        /// <summary>
        /// Adds the image in excel sheet.
        /// </summary>
        /// <param name="ws">Worksheet</param>
        /// <param name="colIndex">Column Index</param>
        /// <param name="rowIndex">Row Index</param>
        /// <param name="filePath">The file path</param>


        /// <summary>
        /// Adds the comment in excel sheet.
        /// </summary>
        /// <param name="ws">Worksheet</param>
        /// <param name="colIndex">Column Index</param>
        /// <param name="rowIndex">Row Index</param>
        /// <param name="comments">Comment text</param>
        /// <param name="author">Author Name</param>
        private static void AddComment(ExcelWorksheet ws, int colIndex, int rowIndex, string comment, string author) {
            //Adding a comment to a Cell
            var commentCell = ws.Cells[rowIndex, colIndex];
            commentCell.AddComment(comment, author);
        }

        /// <summary>
        /// Pixel2s the MTU.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <returns></returns>
        public static int Pixel2MTU(int pixels) {
            int mtus = pixels * 9525;
            return mtus;
        }

        /// <summary>
        /// Creates the data table with some dummy data.
        /// </summary>
        /// <returns>DataTable</returns>
        private static DataTable CreateDataTable() {
            DataTable dt = new DataTable();
            for (int i = 0; i < 10; i++) {
                dt.Columns.Add(i.ToString());
            }

            for (int i = 0; i < 10; i++) {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns) {
                    dr[dc.ToString()] = i;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }


    }
}
