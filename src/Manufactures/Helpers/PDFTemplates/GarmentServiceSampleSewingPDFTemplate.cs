using Manufactures.Dtos.GarmentSample;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Manufactures.Helpers.PDFTemplates
{
    public class GarmentServiceSampleSewingPDFTemplate
    {
        public static MemoryStream Generate(GarmentServiceSampleSewingDto garmentSampleSewing)
        {
            Document document = new Document(PageSize.A5.Rotate(), 40, 40, 100, 40);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.PageEvent = new GarmentServiceSampleSewingPDFHeader(garmentSampleSewing);
            document.Open();

            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            PdfPCell cellCenterBottomNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell cellRightNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellJustifyNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
            PdfPCell cellJustifyAllNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL };

            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };


            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            #region Header
            //PdfPTable tableTiltle = new PdfPTable(1);
            //cellCenterNoBorder.Phrase = new Paragraph("Sample JASA GARMENT WASH\n\n\n", bold_font);
            //tableTiltle.AddCell(cellCenterNoBorder);

            //PdfPCell cellTitle = new PdfPCell(tableTiltle);
            //tableTiltle.ExtendLastRow = false;
            //document.Add(tableTiltle);

            //PdfPTable tableHeader = new PdfPTable(3);
            //tableHeader.SetWidths(new float[] { 1f, 1f, 1f });

            //PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            //cellHeaderContentLeft.AddElement(new Phrase("PT DAN LIRIS", normal_font));
            //cellHeaderContentLeft.AddElement(new Phrase("SUKOHARJO", normal_font));
            //cellHeaderContentLeft.AddElement(new Phrase("BANARAN, GROGOL", normal_font));
            //tableHeader.AddCell(cellHeaderContentLeft);

            //PdfPCell cellHeaderContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
            //cellHeaderContentCenter.AddElement(new Paragraph("Tanggal Sample    : " + garmentSampleSewing.ServiceSampleSewingDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            //cellHeaderContentCenter.AddElement(new Paragraph("No Sample            : " + garmentSampleSewing.ServiceSampleSewingNo, normal_font));
            //tableHeader.AddCell(cellHeaderContentCenter);

            //PdfPCell cellHeaderContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
            //cellHeaderContentRight.AddElement(new Phrase("Buyer : " + garmentSampleSewing.Buyer.Name, normal_font));

            //tableHeader.AddCell(cellHeaderContentRight);

            //PdfPCell cellHeader = new PdfPCell(tableHeader);
            //tableHeader.ExtendLastRow = false;
            //tableHeader.SpacingAfter = 15f;
            //document.Add(tableHeader);
            #endregion

            List<GarmentSampleSewingItemVM> itemData = new List<GarmentSampleSewingItemVM>();

            foreach (var item in garmentSampleSewing.Items)
            {
                foreach (var detail in item.Details)
                {
                    var data = itemData.FirstOrDefault(x =>x.RoNo == item.RONo&& x.Color == detail.Color);

                    GarmentSampleSewingItemVM garmentSampleSewingItemVM = new GarmentSampleSewingItemVM();
                    garmentSampleSewingItemVM.RoNo = item.RONo;
                    garmentSampleSewingItemVM.Article = item.Article;
                    garmentSampleSewingItemVM.Comodity = item.Comodity.Code + " - " + item.Comodity.Name;

                    if (data == null)
                    {
                        garmentSampleSewingItemVM.DesignColor = detail.DesignColor;
                        garmentSampleSewingItemVM.Color = detail.Color;
                        garmentSampleSewingItemVM.Unit = detail.Unit.Code;
                        garmentSampleSewingItemVM.Quantity = detail.Quantity;
                        garmentSampleSewingItemVM.UomUnit = detail.Uom.Unit;
                        garmentSampleSewingItemVM.Remark = detail.Remark;
                        garmentSampleSewingItemVM.UnitName = item.Unit.Name;
                        itemData.Add(garmentSampleSewingItemVM);
                    }
                    else
                    {
                        data.Quantity += detail.Quantity;
                    }
                }
            }

            #region content

            PdfPTable tableContent = new PdfPTable(8);
            List<float> widths = new List<float>();
            widths.Add(2f);
            widths.Add(3f);
            widths.Add(5f);
            widths.Add(5f);
            widths.Add(2f);
            widths.Add(2f);
            widths.Add(4f);
            widths.Add(4f);

            tableContent.SetWidths(widths.ToArray());

            cellCenter.Phrase = new Phrase("Unit", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("RO", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Artikel", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Warna", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);

            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Komoditi", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            

            double grandTotal = 0;
            foreach (var i in itemData)
            {
                cellCenter.Phrase = new Phrase(i.Unit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.RoNo, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                //cellCenter.Phrase = new Phrase(i.UnitName, normal_font);
                //cellCenter.Rowspan = 1;
                //tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Article, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Color, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase(i.Quantity.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.UomUnit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Remark, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Comodity, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                
                grandTotal += i.Quantity;
            }

            cellRight.Phrase = new Phrase("TOTAL", bold_font);
            cellRight.Rowspan = 1;
            cellRight.Colspan = 4;
            tableContent.AddCell(cellRight);
            cellCenter.Phrase = new Phrase(grandTotal.ToString(), bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase(" ", normal_font);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = 3;
            tableContent.AddCell(cellCenter);

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);
            #endregion
            Paragraph qtyUomPacking = new Paragraph("Jumlah Kemasan : " + garmentSampleSewing.QtyPacking + "      " + "Satuan Kemasan : " + garmentSampleSewing.UomUnit, normal_font);
            qtyUomPacking.SpacingAfter = 5f;
            document.Add(qtyUomPacking);
            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            cellCenterTopNoBorder.Phrase = new Paragraph("Penerima\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Bag. Sewing\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterBottomNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterBottomNoBorder);
            cellCenterBottomNoBorder.Phrase = new Paragraph($"Dicetak : {DateTimeOffset.Now.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID"))}", normal_font);
            tableSignature.AddCell(cellCenterBottomNoBorder);
            cellCenterBottomNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterBottomNoBorder);

            PdfPCell cellSignature = new PdfPCell(tableSignature);
            tableSignature.ExtendLastRow = false;
            document.Add(tableSignature);

            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }

    public class GarmentSampleSewingItemVM
    {
        public string RoNo { get; set; }
        public string Article { get; set; }
        public string Comodity { get; set; }
        public string DesignColor { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string UomUnit { get; set; }
        public string Remark { get; set; }
        public string UnitName { get; set; }
        public string Color { get; set; }
    }

    class GarmentServiceSampleSewingPDFHeader : PdfPageEventHelper
    {
        private BaseFont _baseFont;
        private BaseFont _infoFont;
        private IServiceProvider serviceProvider;
        private GarmentServiceSampleSewingDto garmentSampleSewing;
        int clientTimeZoneOffset;
        PdfContentByte cb;
        PdfTemplate _headerTemplate, footerTemplate;
        PdfReader totalCount;


        public GarmentServiceSampleSewingPDFHeader(GarmentServiceSampleSewingDto garmentSampleSewing)
        {
            this.garmentSampleSewing = garmentSampleSewing;
            //this.serviceProvider = serviceProvider;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //PrintTime = DateTime.Now;
                _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                //headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                //handle exception here
            }
            catch (System.IO.IOException ioe)
            {
                //handle exception here
            }

        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            cb = writer.DirectContent;
            cb.BeginText();

            float height = writer.PageSize.Height, width = writer.PageSize.Width;
            float marginLeft = document.LeftMargin, marginTop = document.TopMargin, marginRight = document.RightMargin, marginBottom = document.BottomMargin;

            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _infoFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED), 8);

            //var headOfficeX = marginLeft;
            //var headOfficeY = height - marginTop + 130;

            //string[] headOffices = {
            //        "PT DAN LIRIS",
            //        "JL. Merapi No.23",
            //        "Banaran, Grogol, Kab. Sukoharjo",
            //    };
            //for (int i = 0; i < headOffices.Length; i++)
            //{
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, headOffices[i], headOfficeX, headOfficeY + 10 - (i * 12), 0);
            //}

            var titleY = height - marginTop + 80;
            var infoY = height - marginTop + 50;

            string titleString = "Sample JASA GARMENT WASH";

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titleString, width / 2, titleY, 0);
            cb.EndText();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            var baseFontNormal = new Font(Font.HELVETICA, 9f, Font.NORMAL, BaseColor.Black);
            var baseFontBold = new Font(Font.HELVETICA, 9f, Font.BOLD, BaseColor.Black);
            cb = writer.DirectContent;

            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _infoFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            footerTemplate = cb.CreateTemplate(50, 50);

            var pdfTab = new PdfPTable(3);
            float[] tblCell = new float[] { 1f, 1f, 1f };
            pdfTab.SetWidths(tblCell);

            //string paymentmethods = "";
            //List<DateTimeOffset> coba = new List<DateTimeOffset>();
            //foreach (GarmentInternNoteItemViewModel item in viewModel.items)
            //{
            //    foreach (GarmentInternNoteDetailViewModel detail in item.details)
            //    {
            //        coba.Add(detail.paymentDueDate);
            //        paymentmethods = detail.deliveryOrder.paymentMethod;
            //    }
            //}
            //DateTimeOffset coba1 = coba.Min(p => p);

            //PdfPCell newTblCell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell newTblCellLeft = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP};
            PdfPCell newTblCellRight = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell newTblCellMiddle = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP };

            newTblCellRight.Phrase = new Phrase("PT DAN LIRIS", baseFontNormal);
            pdfTab.AddCell(newTblCellRight);
            newTblCellRight.Phrase = new Phrase(" ", baseFontNormal);
            pdfTab.AddCell(newTblCellRight);
            newTblCellLeft.Phrase = new Phrase("Tanggal Sample " + "   :   " + garmentSampleSewing.ServiceSubconSewingDate.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), baseFontNormal);
            pdfTab.AddCell(newTblCellLeft);
            newTblCellRight.Phrase = new Phrase("BANARAN, GROGOL, SUKOHARJO", baseFontNormal);
            pdfTab.AddCell(newTblCellRight);
            newTblCellRight.Phrase = new Phrase(" ", baseFontNormal);
            pdfTab.AddCell(newTblCellRight);
            newTblCellLeft.Phrase = new Phrase("No Sample " + "            :   " + garmentSampleSewing.ServiceSubconSewingNo, baseFontNormal);
            pdfTab.AddCell(newTblCellLeft);
            newTblCellLeft.Phrase = new Phrase(" ", baseFontNormal);
            pdfTab.AddCell(newTblCellLeft);
            newTblCellLeft.Phrase = new Phrase(" ", baseFontNormal);
            pdfTab.AddCell(newTblCellLeft);
            newTblCellLeft.Phrase = new Phrase("Buyer " + "                    :   " + garmentSampleSewing.Buyer.Name, baseFontNormal);
            pdfTab.AddCell(newTblCellLeft);



            //         if ( coba1 < viewModel.inDate)
            //{
            //             PdfPTable tableRemark = new PdfPTable(2);
            //             float[] widths = new float[] { 1f, 0.5f };
            //             tableRemark.SetWidths(widths);
            //             PdfPCell CellRemarkContent = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };

            //             CellRemarkContent.Phrase = new Phrase("Remark :\n\n\n\n\n\n\n", baseFontNormal);
            //             tableRemark.AddCell(CellRemarkContent);
            //             CellRemarkContent.Phrase = new Phrase("TTD\n\n\n\n\n\n\n", baseFontNormal);
            //             tableRemark.AddCell(CellRemarkContent);

            //             PdfPCell cellRemark = new PdfPCell(tableRemark); // don't remove
            //             tableRemark.ExtendLastRow = false;
            //             tableRemark.SpacingAfter = 10f;
            //             document.Add(tableRemark);
            //         }


            string text = "Page " + writer.CurrentPageNumber + " of " + writer.PageNumber;

            {
                cb.BeginText();
                cb.SetFontAndSize(_infoFont, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = _infoFont.GetWidthPoint(text, 8);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetBottom(30));
            }

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;

            //pdfTab.LockedWidth = true;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - document.TopMargin + 65, writer.DirectContent);

            //cb.MoveTo(40, document.PageSize.GetBottom(50));
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            //cb.Stroke();

            //totalCount.Close();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}
