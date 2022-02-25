﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SampleBrowser.Maui.Core;
using Syncfusion.Pdf;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Graphics;

namespace SampleBrowser.Maui.Pdf
{
    public partial class Certificate : SampleView
    {
        #region Constructor
        /// <summary>
        /// Initializes component.
        /// </summary>
        public Certificate()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        /// <summary>
        /// Creates slides with simple text, table and image in a PowerPoint Presentation.
        /// </summary>
        private void OnButtonClicked(object sender, EventArgs e)
        {
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Set PDF landscape page orientiation. 
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            //Set page margins. 
            document.PageSettings.Margins.All = 0;
            //Add page to the PDF document. 
            PdfPage page = document.Pages.Add();
            //Get the page size to draw the contents to PDF page. 
            SizeF pageSize = page.GetClientSize();

            //Get the image file stream from assembly.
            Assembly assembly = typeof(Certificate).GetTypeInfo().Assembly;
            string basePath = "SampleBrowser.Maui.Resources.Pdf.";
            Stream imageStream = assembly.GetManifestResourceStream(basePath + "certificate.jpg");

            //Load the PDF document from stream.
            PdfBitmap bitmapImage = new PdfBitmap(imageStream);
            //Draw the PDF bitmap image to PDF page with provided size. 
            page.Graphics.DrawImage(bitmapImage, new RectangleF(0, 0, pageSize.Width, pageSize.Height));

            //Create font with different size. 
            PdfFont nameFont = new PdfStandardFont(PdfFontFamily.Helvetica, 22);
            PdfFont controlFont = new PdfStandardFont(PdfFontFamily.Helvetica, 19);
            PdfFont dateFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
            PdfBrush textBrush = new PdfSolidBrush(new PdfColor(20, 58, 86));

            string name = "John Milton";
            string courseName = "Microsoft Azure Fundamentals";

            //Find the X position based on text and font size. 
            float x = calculateXPosition(name, nameFont, pageSize.Width);
            //Draw the string to specified location. 
            page.Graphics.DrawString(name, nameFont, textBrush, new RectangleF(x, 253, 0, 0));

            //Find the X position based on text and font size. 
            x = calculateXPosition(courseName, controlFont, pageSize.Width);
            //Draw the string to specified location. 
            page.Graphics.DrawString(courseName, controlFont, textBrush, new RectangleF(x, 340, 0, 0));

            ////Get date value from date picker field. 
            //var dateTimeOffset = date.Date;
            //DateTime time = dateTimeOffset.Value.DateTime;
            //Get the formatted Date to add in PDF page. 
            string formatedDate = "on October 10, 2021";// + time.ToString("MMMM d, yyyy");

            //Find the X position based on text and font size. 
            x = calculateXPosition(formatedDate, dateFont, pageSize.Width);
            //Draw the string to specified location. 
            page.Graphics.DrawString(formatedDate, dateFont, textBrush, new RectangleF(x, 385, 0, 0));
            
            using MemoryStream stream = new();
            //Saves the presentation to the memory stream.
            document.Save(stream);
            stream.Position = 0;
            //Saves the memory stream as file.
            DependencyService.Get<ISave>().SaveAndView("Certificate.pdf", "application/pdf", stream);
        }
        #endregion

        float calculateXPosition(string text, PdfFont font, float pageWidth)
        {
            //Measure the text size based on the font size. 
            SizeF textSize = font.MeasureString(text, new SizeF(pageWidth, 0));
            return (pageWidth - textSize.Width) / 2;
        }

    }
}
