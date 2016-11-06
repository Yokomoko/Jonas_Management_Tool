using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BL_JonasSageImporter;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Data.SqlClient;

namespace Jonas_Sage_Importer.Generate_Excel_Reports {
    class GenerateGBUCombinedTargets {
        #region Enums

        private enum FilterTypes {
            Total_Backlog = 0,
            Installed_This_Month = 1,
            Installed_This_Month_Excluding_This_Week = 2,
            This_Week = 3,
            Forecast_This_Month = 4,
            Forecast_Future_Months = 5,
            No_Forecast = 6,
            Stuck = 7,
            Cancelled = 8
        }
        #endregion

        /// <summary>
        /// Generates the report.
        /// </summary>
        public static void GenerateReport() {
            using (ExcelPackage p = new ExcelPackage()) {
                //set the workbook properties and add a default sheet in it
                SetWorkbookProperties(p);
                //Create a sheet
                ExcelWorksheet ws1 = CreateSheet(p, "Summary", 1);
                ExcelWorksheet ws2 = CreateSheet(p, "Breakout", 2);

                var percentageFormat = "0%";
                var currencyFormat = @"_-£* #,##0_-;-£* #,##0_-;_-£* ""-""_-;_-@_-";



                //Set border style
                //Insides first
                ws1.Cells[8, 2, 32, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws1.Cells[6, 2, 32, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws1.Cells[14, 4, 16, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws1.Cells[14, 4, 16, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws1.Cells[17, 4, 20, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws1.Cells[17, 4, 20, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws1.Cells[21, 4, 22, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws1.Cells[21, 4, 22, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws1.Cells[28, 2, 29, 4].Style.Border.Right.Style = ExcelBorderStyle.None;
                ws1.Cells[23, 2, 27, 4].Style.Border.Right.Style = ExcelBorderStyle.None;
                ws1.Cells[23, 2, 27, 4].Style.Border.Bottom.Style = ExcelBorderStyle.None;
                //Medium thickness places
                ws1.Cells[6, 2, 32, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws1.Cells[6, 2, 6, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws1.Cells[7, 2, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws1.Cells[6, 4, 29, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws1.Cells[13, 2, 13, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[14, 2, 14, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[14, 8, 16, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws1.Cells[16, 7, 16, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[14, 6, 22, 6].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws1.Cells[20, 7, 20, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[21, 8, 22, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws1.Cells[21, 2, 21, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[22, 2, 22, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[22, 4, 32, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws1.Cells[32, 2, 32, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws1.Cells[27, 2, 28, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                //Merging cells and create a center heading for out table
                ws1.Cells[3, 2].Value = "Combined EPOS / FM and CCR Key Weekly Figures Status";
                ws1.Cells[3, 2, 3, 3].Merge = true;
                ws1.Cells[3, 2, 3, 3].Style.Font.Bold = true;
                ws1.Cells[3, 2, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Column(2).Width = 72.25;

                ws1.Cells[4, 2].Value = "(We need to all sell to make this happen)";
                ws1.Cells[4, 2, 4, 3].Merge = true;
                ws1.Cells[4, 2, 4, 3].Style.Font.Bold = true;
                ws1.Cells[4, 2, 4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws1.Cells[6, 2].Value = "Sales (NickTB to populate)";
                ws1.Cells[6, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[6, 2, 7, 4].Style.Font.Bold = true;
                ws1.Cells[6, 3].Value = "Gross";
                ws1.Cells[6, 4].Value = "Nett";
                ws1.Cells[6, 2, 6, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[6, 2, 6, 4].Style.Fill.BackgroundColor.SetColor(Color.Beige);
                ws1.Cells[8, 2].Value = "Total values of Sales Booked Last Week";
                ws1.Cells[9, 2].Value = "Running total of sales booked for the month so far (Gross and Net)";
                ws1.Cells[10, 2].Value = "Total Gross pipeline value";
                ws1.Cells[11, 2].Value = "Total forecast to close this week";
                ws1.Cells[12, 2].Value = "Total forecast to close next week";

                ws1.Cells[14, 2].Value = "BACKLOG (EPOS Group / FM / CCR)";
                ws1.Cells[14, 2].Style.Font.Bold = true;
                ws1.Cells[14, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[14, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws1.Cells[14, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Beige);

                var today = DateTime.Today;
                var friday = today.AddDays(-(int)today.DayOfWeek).AddDays(5);

                ws1.Cells[14, 3].Value = "Friday " + friday.ToShortDateString();
                ws1.Cells[14, 3, 14, 4].Merge = true;
                ws1.Cells[14, 3, 14, 4].Style.Font.Bold = true;
                ws1.Cells[14, 3, 14, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws1.Cells[14, 5].Value = "Previous Week - " + friday.AddDays(-7).ToString("dd/MM");
                ws1.Cells[14, 5, 14, 6].Merge = true;
                ws1.Cells[14, 5, 14, 6].Style.Font.Bold = true;
                ws1.Cells[14, 5, 14, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws1.Cells[14, 7].Value = "Backlog Growth";
                ws1.Cells[14, 7, 14, 8].Merge = true;
                ws1.Cells[14, 7, 14, 8].Style.Font.Bold = true;
                ws1.Cells[14, 7, 14, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws1.Cells[15, 3].Value = "Gross";
                ws1.Cells[15, 4].Value = "Nett";
                ws1.Cells[15, 5].Value = "Gross";
                ws1.Cells[15, 6].Value = "Nett";
                ws1.Cells[15, 7].Value = "Amount";
                ws1.Cells[15, 8].Value = "%";

                ws1.Cells[16, 2].Value = "Total backlog value (PS+Lic+Hardware)";
                ws1.Cells[16, 3, 22, 6].Style.Numberformat.Format = currencyFormat;

                ws1.Cells[14, 3, 14, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[16, 3, 17, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[19, 3, 20, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;

                ws1.Cells[16, 3, 17, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
                ws1.Cells[19, 3, 20, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
                ws1.Cells[14, 3, 14, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);

                ws1.Cells[16, 7].Formula = "C16 - E16";
                ws1.Cells[16, 7].Style.Numberformat.Format = currencyFormat;

                ws1.Cells[16, 8].Style.Numberformat.Format = percentageFormat;
                ws1.Cells[16, 8].Formula = "G16 / E16";

                ws1.Cells[17, 2].Value = "Total Installed (ie invoiced) this week (PS+Lic+Hardware)";
                ws1.Cells[19, 2].Value = "Total Backlog booked for this month, not installed (PS+Lic+Hardware)";
                ws1.Cells[20, 2].Value = "Running total of Equipment installed so far this month (PS+Lic+Hardware)";

                ws1.Cells[21, 7, 21, 8].Merge = true;
                ws1.Cells[21, 7].Value = "Difference (Gross)";
                ws1.Cells[21, 7, 21, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[21, 7, 21, 8].Style.Font.Bold = true;

                ws1.Cells[22, 2].Value = "Predicted Equipment Invoices for the month";
                ws1.Cells[22, 2].Style.Font.Bold = true;
                ws1.Cells[22, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[22, 2, 22, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws1.Cells[22, 2, 22, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Beige);
                ws1.Cells[22, 3].Formula = "+C20 + C19";
                ws1.Cells[22, 4].Formula = "+D20 + D19";
                ws1.Cells[22, 5].Formula = "+E20 + E19";
                ws1.Cells[22, 6].Formula = "+F20 + F19";

                ws1.Cells[22, 7].Formula = "C22 - E22";
                ws1.Cells[22, 7].Style.Numberformat.Format = currencyFormat;
                ws1.Cells[22, 8].Formula = "G22 / E22";
                ws1.Cells[22, 8].Style.Numberformat.Format = percentageFormat;


                ws1.Cells[23, 2].Value =
                    "Our Monthly Gross Equipment installations directly affect the profit that we make";
                ws1.Cells[23, 2].Style.Font.Bold = true;

                ws1.Cells[24, 2].Value = "Gross install <£250K is making a loss";
                ws1.Cells[24, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws1.Cells[24, 2].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);

                ws1.Cells[25, 2].Value = "Gross install £250K to £350k is making an adequate Profit";
                ws1.Cells[25, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[25, 2].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                ws1.Cells[26, 2].Value = "Gross install >£350K is making exceeding our target";
                ws1.Cells[26, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[26, 2].Style.Fill.BackgroundColor.SetColor(Color.Green);

                ws1.Cells[28, 2].Value = "AR (SB to populate)";
                ws1.Cells[28, 2, 28, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[28, 2, 28, 4].Style.Fill.BackgroundColor.SetColor(Color.Beige);

                ws1.Cells[30, 2].Value = "Total AR Value Aged";
                ws1.Cells[31, 2].Value = "Cash collected this week";
                ws1.Cells[32, 2].Value = "Running Total of Cash Collected this month";

                ws1.Cells[10, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[10, 4].Style.Fill.BackgroundColor.SetColor(Color.DimGray);
                ws1.Cells[30, 4, 32, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[30, 4, 32, 4].Style.Fill.BackgroundColor.SetColor(Color.DimGray);


                //Calculate calculated fields
                ws1.Calculate();


                //Generate Second Worksheet

                ws2.Column(2).Width = 20;
                ws2.Column(3).Width = 10;
                ws2.Column(4).Width = 10;
                ws2.Column(5).Width = 10;

                for (int i = 2; i < 25; i++) {
                    ws2.Cells[i, 2, i, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                for (int i = 13; i < 25; i++) {
                    ws2.Cells[i, 2, i, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[i, 2, i, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[i, 2, i, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[i, 2, i, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[i, 2].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    if (i < 6) {
                        ws2.Cells[14, i].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    }
                }
                ws2.Cells[2, 2, 8, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);


                ws2.Cells[2, 2, 2, 5].Style.Font.Bold = true;
                ws2.Cells[2, 3].Value = "Gross";
                ws2.Cells[2, 4].Value = "Nett";
                ws2.Cells[2, 5].Value = "Gross %";
                ws2.Cells[3, 2].Value = "Forecast - This Month";
                ws2.Cells[4, 2].Value = "Forecast - Future Months";
                ws2.Cells[5, 2].Value = "No forecast";
                ws2.Cells[6, 2].Value = "Total Backlog";
                ws2.Cells[8, 2].Value = "Stuck **";
                ws2.Cells[6, 5].Style.Font.Bold = true;
                ws2.Cells[8, 2, 8, 5].Style.Font.Color.SetColor(Color.Red);
                ws2.Cells[7, 3].Formula = "Summary!C16";
                ws2.Cells[7, 4].Formula = "Summary!D16";
                ws2.Row(7).Hidden = true;

                ws2.Cells[10, 2, 11, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                ws2.Cells[10, 2].Value = "Installed This Month";
                ws2.Cells[11, 2].Value = "Cancelled This Month";
                ws2.Cells[11, 2, 11, 5].Style.Font.Color.SetColor(Color.Red);


                ws2.Cells[13, 2, 13, 5].Merge = true;
                ws2.Cells[13, 2, 13, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws2.Cells[13, 2].Style.Font.Bold = true;
                ws2.Cells[13, 2, 13, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws2.Cells[14, 3, 14, 5].Style.Font.Bold = true;
                ws2.Cells[13, 2, 24, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                ws2.Cells[13, 2].Value = "Backlog by Product Type (Gross)";
                ws2.Cells[14, 2].Value = "Gross >";
                ws2.Cells[15, 2].Value = "Absolute";
                ws2.Cells[16, 2].Value = "CBMS";
                ws2.Cells[17, 2].Value = "Development";
                ws2.Cells[18, 2].Value = "FM";
                ws2.Cells[19, 2].Value = "Infinity";
                ws2.Cells[20, 2].Value = "Pixel";
                ws2.Cells[21, 2].Value = "QT";
                ws2.Cells[22, 2].Value = "Quantum";
                ws2.Cells[23, 2].Value = "RS";
                ws2.Cells[24, 2].Value = "Grand Total";
                ws2.Cells[24, 2, 24, 6].Style.Font.Bold = true;
                ws2.Cells[14, 3].Value = "Hardware";
                ws2.Cells[14, 4].Value = "Software";
                ws2.Cells[14, 5].Value = "Pro Services";

                ws2.Cells[24, 3].Formula = "SUM(C15:C23)";
                ws2.Cells[24, 4].Formula = "SUM(D15:D23)";
                ws2.Cells[24, 5].Formula = "SUM(E15:E23)";
                ws2.Cells[24, 6].Formula = "SUM(C24:E24)";
                ws2.Cells[24, 3, 24, 6].Style.Numberformat.Format = currencyFormat;
                ws2.Cells[4, 3, 4, 8].Style.Numberformat.Format = currencyFormat;
                ws2.Cells[10, 3, 11, 4].Style.Numberformat.Format = currencyFormat;

                ws2.Cells[3, 5, 6, 5].Style.Numberformat.Format = percentageFormat;
                ws2.Cells[3, 8].Style.Numberformat.Format = percentageFormat;

                //Populate Common Conditional Formatting
                ExcelFillStyle fsSolid = ExcelFillStyle.Solid;
                var formula = "0";
                Color grn = Color.DarkSeaGreen;
                Color trans = Color.Transparent;
                Color red = Color.IndianRed;

                var ef = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
                SqlParameter filterId = new SqlParameter("@CogsFilter, 0", SqlDbType.Int);

                var totalSalesBacklog = from tsb in ef.GetNetandGrossCogs((int?)FilterTypes.Total_Backlog)
                                        select new {
                                            tsb.GrossValue,
                                            tsb.NetValue
                                        };
                var thisMonthForecast = from tsb in ef.GetNetandGrossCogs((int?)FilterTypes.Forecast_This_Month)
                                        select new {
                                            tsb.GrossValue,
                                            tsb.NetValue
                                        };



                #region Backlog Growth Formatting
                var condFormattingGreen = ws1.ConditionalFormatting.AddGreaterThan(ws1.Cells[16, 7, 16, 8]);
                condFormattingGreen.Formula = formula;
                condFormattingGreen.Style.Fill.PatternType = fsSolid;
                condFormattingGreen.Style.Fill.BackgroundColor.Color = grn;

                var condFormattingNormal = ws1.ConditionalFormatting.AddEqual(ws1.Cells[16, 7, 16, 8]);
                condFormattingNormal.Formula = formula;
                condFormattingNormal.Style.Fill.PatternType = fsSolid;
                condFormattingNormal.Style.Fill.BackgroundColor.Color = trans;

                var condFormattingRed = ws1.ConditionalFormatting.AddLessThan(ws1.Cells[16, 7, 16, 8]);
                condFormattingRed.Formula = formula;
                condFormattingRed.Style.Fill.PatternType = fsSolid;
                condFormattingRed.Style.Fill.BackgroundColor.Color = red;
                #endregion

                var diffFormattingGrn = ws1.ConditionalFormatting.AddGreaterThan(ws1.Cells[22, 7, 22, 8]);
                diffFormattingGrn.Formula = formula;
                diffFormattingGrn.Style.Fill.PatternType = fsSolid;
                diffFormattingGrn.Style.Fill.BackgroundColor.Color = grn;

                var diffFormattingNormal = ws1.ConditionalFormatting.AddEqual(ws1.Cells[22, 7, 22, 8]);
                diffFormattingNormal.Formula = formula;
                diffFormattingNormal.Style.Fill.PatternType = fsSolid;
                diffFormattingNormal.Style.Fill.BackgroundColor.Color = trans;

                var diffFormattingRed = ws1.ConditionalFormatting.AddLessThan(ws1.Cells[22, 7, 22, 8]);
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

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName, int sheetId) {
            p.Workbook.Worksheets.Add(sheetName);
            ExcelWorksheet ws = p.Workbook.Worksheets[sheetId];
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
