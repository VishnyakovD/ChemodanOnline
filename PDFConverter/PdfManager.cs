using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace PDFConverter
{
    public class PdfManager
    {
        public PdfManager()
        {

        }
        public MemoryStream BuildDocument(EditOrderModel order)
        {
            //MemoryStream workStream = new MemoryStream();
            //Document document = new Document(PageSize.A4, 30, 30, 30, 30);
            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            //document.Open();
            //document.Add(new Paragraph(html));
            //document.Close();
            //byte[] byteInfo = workStream.ToArray();
            //workStream.Write(byteInfo, 0, byteInfo.Length);
            //workStream.Position = 0;

            //return workStream;
            return RussianPDF(html);


        }

        public MemoryStream RussianPDF(string html)
        {
            try
            {

                MemoryStream workStream = new MemoryStream();

                Document document = new Document(PageSize.A4, 30, 30, 30, 30);

                PdfWriter.GetInstance(document, workStream).CloseStream = false;

                string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\arial.ttf";
                var sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var head = new Font(sylfaen, 9f, Font.NORMAL, BaseColor.BLACK);
                var normal = new Font(sylfaen, 10f, Font.NORMAL, BaseColor.BLACK);
                var underline = new Font(sylfaen, 10f, Font.UNDERLINE, BaseColor.BLACK);

                document.Open();

                document.Add(new Paragraph(Element.ALIGN_CENTER, "Договір найму №222", head) );

                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph("м. Київ                                                                                                                                                                       дата "+DateTime.Now.Date.ToString("dd.MM.yyyy"),normal));
                document.Add(new Paragraph(" ", normal));



                PdfPTable table1 = new PdfPTable(2);
                table1.TotalWidth = document.PageSize.Width;
                table1.LockedWidth = true;
                float[] widths1 = new float[] { 1f, 4f };
                table1.SetWidths(widths1);
                table1.HorizontalAlignment = 0;
                PdfPCell table1cell11 = new PdfPCell(new Phrase("Объект:", normal));
                table1cell11.Border = 0;
                table1.AddCell(table1cell11);
                PdfPCell table1cell12 = new PdfPCell(new Phrase("Ferienhaus 'Waldesruh'", normal));
                table1cell12.Border = 0;
                table1.AddCell(table1cell12);
                PdfPCell table1cell21 = new PdfPCell(new Phrase("Адрес:", normal));
                table1cell21.Border = 0;
                table1.AddCell(table1cell21);
                PdfPCell table1cell22 = new PdfPCell(new Phrase("15344 Strausberg, Am Marienberg 45", normal));
                table1cell22.Border = 0;
                table1.AddCell(table1cell22);
                PdfPCell table1cell31 = new PdfPCell(new Phrase("Номер объекта:", normal));
                table1cell31.Border = 0;
                table1.AddCell(table1cell31);
                PdfPCell table1cell32 = new PdfPCell(new Phrase("czr04012012", normal));
                table1cell32.Border = 0;
                table1.AddCell(table1cell32);
                PdfPCell table1cell41 = new PdfPCell(new Phrase("Дата заезда:", normal));
                table1cell41.Border = 0;
                table1.AddCell(table1cell41);
                PdfPCell table1cell42 = new PdfPCell(new Phrase("12.02.2012", normal));
                table1cell42.Border = 0;
                table1.AddCell(table1cell42);
                PdfPCell table1cell51 = new PdfPCell(new Phrase("Дата выезда:", normal));
                table1cell51.Border = 0;
                table1.AddCell(table1cell51);
                PdfPCell table1cell52 = new PdfPCell(new Phrase("18.02.2012", normal));
                table1cell52.Border = 0;
                table1.AddCell(table1cell52);
                PdfPCell table1cell61 = new PdfPCell(new Phrase("Человек:", normal));
                table1cell61.Border = 0;
                table1.AddCell(table1cell61);
                PdfPCell table1cell62 = new PdfPCell(new Phrase("5", normal));
                table1cell62.Border = 0;
                table1.AddCell(table1cell62);
                document.Add(table1);
                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return workStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Try-Catch-Fehler!");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return new MemoryStream();
        }
    }
}
