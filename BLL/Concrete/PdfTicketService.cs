using System.IO;
using BLL.Abstract;
using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Concrete
{
    public class PdfTicketService : ITicketService
    {
        public void GenerateTicket(Ticket ticket)
        {
            FileStream fs = new FileStream(@"X:\Ticket.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle documentSize = new Rectangle(PageSize.A5);
            Document doc = new Document(documentSize, 36, 72, 108, 180);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            documentSize.BackgroundColor = new BaseColor(System.Drawing.Color.WhiteSmoke);

            Paragraph title = new Paragraph("Train Ticket");
            title.Alignment = Element.ALIGN_CENTER;

            Paragraph userName = new Paragraph("Name: " + ticket.PassangerName);
            Paragraph train = new Paragraph("Train number: " + ticket.TrainNumber);
            Paragraph departureDate = new Paragraph("Departure: " + ticket.DepartureTime + " From: " + ticket.DepartureStationName);
            Paragraph arriveDate = new Paragraph("Arrive: " + ticket.ArriveTime + " To: " + ticket.ArriveStationName);
            Paragraph carriageNumber = new Paragraph("Carriage number: " + ticket.CarriageNumber);
            Paragraph placeNumber = new Paragraph("Place number: " + ticket.PlaceNumber);
            doc.Add(userName);
            doc.Add(train);
            doc.Add(departureDate);
            doc.Add(arriveDate);
            doc.Add(carriageNumber);
            doc.Add(placeNumber);

            doc.CloseDocument();
        }
    }
}