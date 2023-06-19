using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.PDFTemplates.GarmentSubcon
{
    public class GarmentSubconInvoicePackingListPDFTemplate
    {
        public static MemoryStream Generate(GarmentSubconInvoicePackingListDto garmentSubconInvoicePacking, GarmentSubconContractDto garmentSubconContract)
        {
            Document document = new Document(PageSize.A5.Rotate(), 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP };
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
            PdfPTable tableTiltle = new PdfPTable(1);
            cellCenterNoBorder.Phrase = new Paragraph("INVOICE/PACKINGLIST\n\n\n", bold_font);
            tableTiltle.AddCell(cellCenterNoBorder);

            PdfPCell cellTitle = new PdfPCell(tableTiltle);
            tableTiltle.ExtendLastRow = false;
            document.Add(tableTiltle);

            PdfPTable tableHeader = new PdfPTable(2);
            tableHeader.SetWidths(new float[] { 2f, 7f, });
            PdfPCell cellIdentityContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };

            cellIdentityContentLeft.Phrase = new Phrase("No Invoice", normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconInvoicePacking.InvoiceNo, normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("Tanggal", normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconInvoicePacking.Date.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("Supplier", normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconInvoicePacking.Supplier.Name, normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("", normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("  " + garmentSubconInvoicePacking.Supplier.Address, normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("No SK", normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconInvoicePacking.ContractNo, normal_font);
            tableHeader.AddCell(cellIdentityContentLeft);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            #region content

            PdfPTable tableContent = new PdfPTable(8);
            tableContent.SetWidths(new float[] { 1f, 4.5f, 2f, 2.5f, 2.5f, 2.5f , 2.5f , 2.5f });

            cellCenter.Phrase = new Phrase("No", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Description", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah Kemasan", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan Kemasan", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Hrg Satuan", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Harga Total", bold_font);
            tableContent.AddCell(cellCenter);



            //        if (garmentSubconInvoicePacking.Items.Count == 0)
            //        {
            //            cellCenter.Phrase = new Phrase(indexItem + 1.ToString(), normal_font);
            //            tableContent.AddCell(cellCenter);

            //cellCenter.Phrase = new Phrase(garmentSubconContract.FinishedGoodType, normal_font);
            //tableContent.AddCell(cellCenter);

            //cellCenter.Phrase = new Phrase("", normal_font);
            //            tableContent.AddCell(cellCenter);

            //            cellCenter.Phrase = new Phrase("", normal_font);
            //            tableContent.AddCell(cellCenter);

            //            cellCenter.Phrase = new Phrase("-", normal_font);
            //            tableContent.AddCell(cellCenter);

            //            cellCenter.Phrase = new Phrase("Rp " + (garmentSubconInvoicePacking.CIF), normal_font);
            //            tableContent.AddCell(cellCenter);

            //            cellCenter.Phrase = new Phrase("Rp " + (garmentSubconInvoicePacking.CIF * 0), normal_font);
            //            tableContent.AddCell(cellCenter);
            //        }
            //        else
            //        { 
            int indexItem = 1;

            if (garmentSubconInvoicePacking.BCType == "BC 2.6.2")
            {
                if (garmentSubconInvoicePacking.ReceiptItems.Count > 0)
                {
                    foreach (var a in garmentSubconInvoicePacking.ReceiptItems)
                    {
                        cellCenter.Phrase = new Phrase(indexItem.ToString(), normal_font);
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase(garmentSubconContract.FinishedGoodType, normal_font);
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase(a.Quantity.ToString(), normal_font);
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase(a.Uom.Unit, normal_font);
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase("-", normal_font);
                        cellCenter.Rowspan = 1;
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase("-", normal_font);
                        cellCenter.Rowspan = 1;
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase("Rp. " + a.PricePerDealUnit.ToString("n", CultureInfo.GetCultureInfo("id-ID")), normal_font);
                        tableContent.AddCell(cellCenter);

                        cellCenter.Phrase = new Phrase("Rp. " + a.TotalPrice.ToString("n", CultureInfo.GetCultureInfo("id-ID")), normal_font);
                        tableContent.AddCell(cellCenter);

                        indexItem++;
                    }
                }
            }
            else if (garmentSubconInvoicePacking.BCType == "BC 2.6.1")
            {
                foreach (var a in garmentSubconInvoicePacking.Items)
                {
                    var sumQtyPack = 0;
                    var UomPack = "";
                    foreach(var dl in a.deliveryLetterOutList)
                    {
                        UomPack = dl.Items[0].UomSatuanUnit;
                        foreach (var dli in dl.Items)
                        {
                            sumQtyPack += dli.QtyPacking;

                        }
                    }
                    cellCenter.Phrase = new Phrase(indexItem.ToString(), normal_font);
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(garmentSubconContract.FinishedGoodType, normal_font);
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(a.Quantity.ToString(), normal_font);
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(a.Uom.Unit, normal_font);
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(sumQtyPack.ToString("n", CultureInfo.GetCultureInfo("id-ID")), normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(UomPack, normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase("Rp. " + garmentSubconContract.CIF.ToString("n", CultureInfo.GetCultureInfo("id-ID")), normal_font);
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase("Rp. " + (garmentSubconContract.CIF * a.Quantity).ToString("n", CultureInfo.GetCultureInfo("id-ID")), normal_font);
                    tableContent.AddCell(cellCenter);

                    indexItem++;
                }
            }
                
            
            //}


            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);

            #endregion

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);
            tableSignature.SetWidths(new float[] { 2f, 7f, });
            //PdfPCell cellIdentityContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };

            if (garmentSubconInvoicePacking.BCType == "BC 2.6.2")
            {
                cellIdentityContentLeft.Phrase = new Phrase("NW", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
                cellIdentityContentLeft.Phrase = new Phrase(": -" , normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);

                cellIdentityContentLeft.Phrase = new Phrase("GW", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
                cellIdentityContentLeft.Phrase = new Phrase(": -", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
            }
            else if (garmentSubconInvoicePacking.BCType == "BC 2.6.1")
            {
                double sumTotGW = 0;
                double sumTotNW = 0;
                foreach (var a in garmentSubconInvoicePacking.Items)
                {
                    sumTotGW += a.TotalGW;
                    sumTotNW += a.TotalNW;
                }
                cellIdentityContentLeft.Phrase = new Phrase("NW", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
                cellIdentityContentLeft.Phrase = new Phrase(": "+ string.Format("{0:0.0000}", sumTotNW) + " KG", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);

                cellIdentityContentLeft.Phrase = new Phrase("GW", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
                cellIdentityContentLeft.Phrase = new Phrase(": " + string.Format("{0:0.0000}", sumTotGW) + " KG", normal_font);
                tableSignature.AddCell(cellIdentityContentLeft);
            }

            #region Padding
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder); cellLeftNoBorder.Phrase = new Paragraph("", normal_font);

            #endregion


            cellLeftNoBorder.Phrase = new Paragraph("       " +"(UDIK WIJANARKO)", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Paragraph("     Kasie Urusan Kepabeaan", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

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
}
