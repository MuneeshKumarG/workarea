﻿using System;
using Syncfusion.XlsIO;
using System.IO;
using System.Reflection;
using SampleBrowser.Maui.Core;
using Microsoft.Maui.Controls;

namespace SampleBrowser.Maui.XlsIO
{
    /// <summary>
    /// Interaction logic for Formula.xaml
    /// </summary>
    public partial class Formula : SampleView
    {
        #region Constructor
        /// <summary>
        /// Formulas constructor
        /// </summary>
        public Formula()
        {
            this.InitializeComponent();
#if ANDROID || IOS
            this.btnRead.HorizontalOptions = LayoutOptions.Center;
            this.btnWrite.HorizontalOptions = LayoutOptions.Center;
            this.btnInput.HorizontalOptions = LayoutOptions.Center;
#endif
        }
        #endregion

        #region Events
        /// <summary>
        /// Loads the input template
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event</param>
        /// <param name="e">contains the event data</param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            string inputPath = "SampleBrowser.Maui.Resources.XlsIO.FormulaTemplate.xlsx";

            Assembly assembly = typeof(Formula).GetTypeInfo().Assembly;
            Stream input = assembly.GetManifestResourceStream(inputPath);

            MemoryStream stream = new MemoryStream();
            input.CopyTo(stream);

            stream.Position = 0;
            DependencyService.Get<ISave>().SaveAndView("FormulaTemplate.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);

            input.Dispose();
        }
        /// <summary>
        /// Writes formula to spreadsheet
        /// </summary>
        /// <param name="sender">Contains a reference to the control/object that raised the event</param>
        /// <param name="e">Contains the event data</param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Step 2 : Instantiate the excel application object.
                Syncfusion.XlsIO.IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //A new workbook is created.[Equivalent to creating a new workbook in MS Excel]
                IWorkbook workbook = application.Workbooks.Create(1);
                //The first worksheet object in the worksheets collection is accessed.
                IWorksheet sheet = workbook.Worksheets[0];

                #region Insert Array Formula

                sheet.Range["A2"].Text = "Array formulas";
                sheet.Range["B2:E2"].FormulaArray = "{10,20,30,40}";
                sheet.Names.Add("ArrayRange", sheet.Range["B2:E2"]);
                sheet.Range["B3:E3"].FormulaArray = "ArrayRange+100";
                sheet.Range["A2"].CellStyle.Font.Bold = true;
                sheet.Range["A2"].CellStyle.Font.Size = 14;

                #endregion

                #region Excel functions

                sheet.Range["A5"].Text = "Formula";
                sheet.Range["B5"].Text = "Result";

                sheet.Range["A7"].Text = "ABS(ABS(-B3))";
                sheet.Range["B7"].Formula = "ABS(ABS(-B3))";

                sheet.Range["A9"].Text = "SUM(B3,C3)";
                sheet.Range["B9"].Formula = "SUM(B3,C3)";

                sheet.Range["A11"].Text = "MIN({10,20,30;5,15,35;6,16,36})";
                sheet.Range["B11"].Formula = "MIN({10,20,30;5,15,35;6,16,36})";

                sheet.Range["A13"].Text = "LOOKUP(B3,B3:E8)";
                sheet.Range["B13"].Formula = "LOOKUP(B3,B3:E3)";

                sheet.Range["A5:B5"].CellStyle.Font.Bold = true;
                sheet.Range["A5:B5"].CellStyle.Font.Size = 14;

                #endregion

                #region Simple formulas
                sheet.Range["C7"].Number = 10;
                sheet.Range["C9"].Number = 10;
                sheet.Range["A15"].Text = "C7+C9";
                sheet.Range["B15"].Formula = "C7+C9";

                #endregion

                sheet.Range["B1"].Text = "Excel formula support";
                sheet.Range["B1"].CellStyle.Font.Bold = true;
                sheet.Range["B1"].CellStyle.Font.Size = 14;
                sheet.Range["B1:E1"].Merge();
                sheet.Range["A1:A15"].AutofitColumns();

                sheet.Calculate();

                string filename = "Formula.xlsx";
                //Saving the workbook to disk.
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                DependencyService.Get<ISave>().SaveAndView(filename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);

                stream.Dispose();

                //No exception will be thrown if there are unsaved workbooks.
                excelEngine.ThrowNotSavedOnDestroy = false;
            }
        }

        /// <summary>
        /// Reads formula from spreadsheet
        /// </summary>
        /// <param name="sender">Contains a reference to the control/object that raised the event</param>
        /// <param name="e">Contains the event data</param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Step 2 : Instantiate the excel application object.
                Syncfusion.XlsIO.IApplication application = excelEngine.Excel;

                string inputPath = "SampleBrowser.Maui.Resources.XlsIO.FormulaTemplate.xlsx";

                Assembly assembly = typeof(Formula).GetTypeInfo().Assembly;
                Stream input = assembly.GetManifestResourceStream(inputPath);
                IWorkbook workbook = application.Workbooks.Open(input);

                //The first worksheet object in the worksheets collection is accessed.
                IWorksheet sheet = workbook.Worksheets[0];

                //Read computed Formula Value. 
                this.txtboxValue.Text = sheet.Range["A11"].FormulaNumberValue.ToString();

                //Read Formula
                this.txtboxFormula.Text = sheet.Range["A11"].Formula;

                input.Dispose();

            }
        }
        #endregion

    }
}