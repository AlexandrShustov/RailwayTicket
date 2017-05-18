using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Concrete;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class TicketServiceTests
    {
        private TicketService _ticketService;

        [TestInitialize]
        public void Setup()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            mock.Setup(u => u.RouteRepository.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new Route
            {
                Stations = new List<RouteStation>
                {
                    new RouteStation
                    {
                        ArriveTime = new DateTime(2017, 5, 1, 12, 0, 0),
                        DepartureTime = new DateTime(2017, 5, 1, 14, 0, 0),
                        Station = new Station {Name = "Kharkov"}
                    },
                    new RouteStation
                    {
                        ArriveTime = new DateTime(2017, 5, 1, 16, 0, 0),
                        DepartureTime = new DateTime(2017, 5, 1, 17, 0, 0),
                        Station = new Station {Name = "Kiev"}
                    }
                }
            }));

            _ticketService = new TicketService(mock.Object);
        }

        [TestMethod]
        public async Task CountPriceMethodReturns100IfTripDurationTwoHours()
        {
            var res = await _ticketService.CountTicketPrice(1, "Kharkov", "Kiev", 0, false);

            Assert.AreEqual(100, res);
        }
    }
}
