using System;
using System.Drawing;
using System.IO;
using BLL.Abstract;
using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace BLL.Concrete
{
    public class PdfTicketService : ITicketService
    {
        public async void GenerateTicket(Ticket ticket)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Ticket.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle documentSize = new Rectangle(PageSize.A4);
            Document doc = new Document(documentSize, 36, 72, 108, 180);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            PdfPTable table = new PdfPTable(4);
            table.LockedWidth = false;
            table.WidthPercentage = 90;
            PdfPCell cell = new PdfPCell(new Phrase("TRAIN TICKET"));
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            table.AddCell("Passanger name: ");
            table.AddCell(ticket.PassangerName);
            table.AddCell("Train: ");
            table.AddCell(ticket.TrainNumber.ToString());
            table.AddCell("Departure: ");
            table.AddCell(ticket.DepartureStationName);
            table.AddCell("Carriage: ");
            table.AddCell(ticket.CarriageNumber.ToString());
            table.AddCell("Arrive: ");
            table.AddCell(ticket.ArriveStationName);
            table.AddCell("Place: ");
            table.AddCell(ticket.PlaceNumber.ToString());
            table.AddCell("Departure date/time: ");
            table.AddCell(ticket.DepartureTime.ToString());
            table.AddCell("Arrive date/time: ");
            table.AddCell(ticket.DepartureTime.ToString());

            doc.Add(table);

            doc.CloseDocument();
        }
    }
}