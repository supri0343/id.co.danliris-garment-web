using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Dtos.GarmentSubcon.SubconReprocess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.PDFTemplates.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessPDFTemplate
    {
        public static MemoryStream Generate(GarmentSubconReprocessDto subconReprocess)
        {
            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell cellRightNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellJustifyNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
            PdfPCell cellJustifyAllNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL };

            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };


            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A5.Rotate(), 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            Paragraph title = new Paragraph("REPROSES SUBCON", header_font);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);


            #region Identity


            PdfPTable tableIdentity = new PdfPTable(3);
            tableIdentity.SetWidths(new float[] { 1f, 4f, 3f});
            PdfPCell cellIdentityContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };

            cellIdentityContentLeft.Phrase = new Phrase("No Reproses", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + subconReprocess.ReprocessNo, normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("PT. DAN LIRIS", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("Tgl Reproses", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + subconReprocess.Date.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("BANARAN, GROGOL, SUKOHARJO", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("Tipe Reproses", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + subconReprocess.ReprocessType, normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            PdfPCell cellIdentity = new PdfPCell(tableIdentity);
            tableIdentity.ExtendLastRow = false;
            tableIdentity.SpacingAfter = 10f;
            tableIdentity.SpacingBefore = 10f;
            document.Add(tableIdentity);

            #endregion

            #region TableContent
            double total = 0;
            if (subconReprocess.ReprocessType == "SUBCON JASA GARMENT WASH")
            {
                PdfPTable tableContent = new PdfPTable(9);
                tableContent.SetWidths(new float[] { 0.8f, 2f, 2f, 3f, 2f, 1.5f, 1.2f, 1.2f, 3f });

                cellCenter.Phrase = new Phrase("No", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Packing List", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("RO", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Article", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Warna", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Unit", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Jumlah", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Keterangan", bold_font);
                tableContent.AddCell(cellCenter);

                int indexItem = 0;
                foreach (var item in subconReprocess.Items)
                {
                    var rowspan = item.Details.Count;

                    cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                    cellCenter.Rowspan = rowspan;
                    tableContent.AddCell(cellCenter);
                    indexItem++;

                    cellLeft.Phrase = new Phrase(item.ServiceSubconSewingNo, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    cellLeft.Phrase = new Phrase(item.RONo, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    cellLeft.Phrase = new Phrase(item.Article, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    foreach (var detail in item.Details)
                    {
                        cellLeft.Phrase = new Phrase(detail.Color, normal_font);
                        cellLeft.Rowspan = 1;
                        tableContent.AddCell(cellLeft);

                        cellLeft.Phrase = new Phrase(detail.Unit.Code, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase(detail.Quantity.ToString(), normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase(detail.Uom.Unit, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellLeft.Phrase = new Phrase(detail.Remark, normal_font);
                        tableContent.AddCell(cellLeft);
                        total += detail.Quantity;
                    }
                }

                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 6;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase("PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);
                cellLeft.Phrase = new Phrase("", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }
            else
            {
                PdfPTable tableContent = new PdfPTable(8);
                tableContent.SetWidths(new float[] { 0.8f, 2f, 2f, 3f, 1f, 1.2f, 1.2f, 3f });

                cellCenter.Phrase = new Phrase("No", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Packing List", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("RO", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Article", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Size", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Jumlah", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Keterangan", bold_font);
                tableContent.AddCell(cellCenter);

                int indexItem = 0;
                foreach (var item in subconReprocess.Items)
                {
                    var rowspan = item.Details.Count;

                    cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                    cellCenter.Rowspan = rowspan;
                    tableContent.AddCell(cellCenter);
                    indexItem++;

                    cellLeft.Phrase = new Phrase(item.ServiceSubconCuttingNo, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    cellLeft.Phrase = new Phrase(item.RONo, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    cellLeft.Phrase = new Phrase(item.Article, normal_font);
                    cellLeft.Rowspan = rowspan;
                    tableContent.AddCell(cellLeft);

                    foreach (var detail in item.Details)
                    {
                        cellLeft.Phrase = new Phrase(detail.Size.Size, normal_font);
                        cellLeft.Rowspan = 1;
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase(detail.Quantity.ToString(), normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase(detail.Uom.Unit, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellLeft.Phrase = new Phrase(detail.Color, normal_font);
                        tableContent.AddCell(cellLeft);
                        total += detail.Quantity;
                    }
                }

                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 5;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase("PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);
                cellLeft.Phrase = new Phrase("", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }

            #endregion
            
            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            PdfPCell cellSignatureContent = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            cellSignatureContent.Phrase = new Phrase("Penerima Oleh\n\n\n\n\n(                              )", normal_font);
            tableSignature.AddCell(cellSignatureContent);
            var bag = subconReprocess.ReprocessType == "SUBCON JASA GARMENT WASH" ? "Bagian Sewing" : "Bagian Cutting";
            cellSignatureContent.Phrase = new Phrase(bag + "\n\n\n\n\n(                              )", normal_font);
            tableSignature.AddCell(cellSignatureContent);


            PdfPCell cellSignature = new PdfPCell(tableSignature); // dont remove
            tableSignature.ExtendLastRow = false;
            tableSignature.SpacingBefore = 10f;
            document.Add(tableSignature);

            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}