using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace BLL.Concrete
{
    public class TicketService : ITicketService
    {
        private IUnitOfWork _unitOfWork;

        private const int PriceForHour = 50;
        private const int LinenPrice = 5;
        private const int TeaPrice = 1;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void GeneratePdfTicket(Ticket ticket)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Ticket.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle documentSize = new Rectangle(PageSize.A4);
            Document doc = new Document(documentSize, 36, 72, 108, 180);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            PdfPTable table = new PdfPTable(4);
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
            table.AddCell(ticket.ArriveTime.ToString());
            table.AddCell("Tea: ");
            table.AddCell(ticket.TeaCount.ToString());
            table.AddCell("Linen: ");
            table.AddCell(ticket.IsNeedLinen ? "Yes" : "No");
            table.AddCell("Price: ");
            table.AddCell(ticket.Price + " uah");
            table.AddCell("");
            table.AddCell("");


            doc.Add(table);

            doc.CloseDocument();
            fs.Close();
            writer.Close();
        }

        public Task CreateTicket(Ticket ticket)
        {
            Guard.ArgumentNotNull(ticket, nameof(ticket) + "should not be null.");

            _unitOfWork.TicketRepository.Add(ticket);

            return _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> CountTicketPrice(int routeId, string stationFrom, string stationTo, int teaCount, bool isNeedLinen)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);

            Guard.ArgumentNotNull(route, nameof(route) + " should be not null.");

            var departureStation = route.Stations.First(s => s.Station.Name == stationFrom);
            var arriveStation = route.Stations.First(s => s.Station.Name == stationTo);

            var time = arriveStation.ArriveTime - departureStation.DepartureTime;

            decimal price = ((int)time.Value.TotalHours * PriceForHour) + teaCount*TeaPrice + (isNeedLinen? LinenPrice: 0);

            return price;
        }
    }
}