using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using Oblig1.Controllers;
using Oblig1.DAL;
using Oblig1.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EnhetsTesting
{
    public class UnitTest
    {



        private const string _loggetInn = "LoggetInn";
        private const string _ikkeLoggetInn = "";



        private readonly Mock<IBillettRepository> mockRep = new Mock<IBillettRepository>();
        private readonly Mock<ILogger<BillettController>> mockLog = new Mock<ILogger<BillettController>>();



        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();




        [Fact]
        public async Task HentAlleRuterOK()
        {

            var Avgangstider = new Avgangstider { TidId = 1, Tid = new TimeSpan(12, 00, 00) };
            var Avgangstider1 = new Avgangstider { TidId = 2, Tid = new TimeSpan(14, 00, 00) };
            var listAvgang = new List<Avgangstider>();

            listAvgang.Add(Avgangstider);
            listAvgang.Add(Avgangstider1);

            var Stasjoner = new Stasjoner { StasjonId = 1, StasjonNavn = "Stovner" };
            var Buss = new Busser { BussId = 1, BussNavn = "Oslo" };
            var Rute = new Ruter { RuteId = 1, Pris = 10 };
            var Avgang = new Avganger { StoppId = 1, Stopp = Stasjoner, Tider = listAvgang, Rute = Rute };
            var Avgang1 = new Avganger { StoppId = 2, Stopp = Stasjoner, Tider = listAvgang, Rute = Rute };
            var listAvganger = new List<Avganger>();

            listAvganger.Add(Avgang);
            listAvganger.Add(Avgang1);

            var bussrute1 = new Buss_Rute { Buss_RuteId = 1, TidFra = new TimeSpan(10, 00, 00), TidTil = new TimeSpan(16, 00, 00), Buss = Buss, Rute = Rute };
            var ruteliste = new List<Buss_Rute>();
            ruteliste.Add(bussrute1);
            mockRep.Setup(k => k.HentAlleRuter()).ReturnsAsync(ruteliste);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentAlleRuter() as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal ((List<Buss_Rute>)resultat.Value,  ruteliste);

      }



        [Fact]
        public async Task HentAlleRuterIkkeOK()
        {


            var biletter = new List<Buss_Rute>(); // tom liste

            mockRep.Setup(k => k.HentAlleRuter()).ReturnsAsync(It.IsAny<List<Buss_Rute>>());



            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;


            // Act
            var resultat = await billettController.HentAlleRuter() as BadRequestObjectResult;



            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Rutene ble ikke hentet!", resultat.Value);
        }


        [Fact]
        public async Task SjekkLoggetInnHentAlleRuter()
        {


            var biletter = new List<Buss_Rute>(); // tom liste

            mockRep.Setup(k => k.HentAlleRuter()).ReturnsAsync(It.IsAny<List<Buss_Rute>>());



            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;


            // Act
            var resultat = await billettController.HentAlleRuter() as UnauthorizedObjectResult;



            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn!", resultat.Value);
        }



        [Fact]
        public async Task OppdaterReturOk()
        {

            mockRep.Setup(k => k.OppdaterRetur(It.IsAny<Reise>())).ReturnsAsync(true);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.OppdaterRetur(It.IsAny<Reise>()) as OkObjectResult;

            // Assert 
            Assert.Equal("Reisen ble oppdatert!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }



        [Fact]
        public async Task OppdaterReturIkkeOk()
        {

            mockRep.Setup(k => k.OppdaterRetur(It.IsAny<Reise>())).ReturnsAsync(false);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.OppdaterRetur(It.IsAny<Reise>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Reisen ble ikke oppdatert!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }



        [Fact]
        public async Task EndreBussOk()
        {

            var Buss = new Busser { BussId = 1, BussNavn = "Oslo" };

            mockRep.Setup(k => k.EndreBuss(Buss)).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBuss(Buss) as OkObjectResult;

            // Assert 
            Assert.Equal("Bussen ble endret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }


        [Fact]
        public async Task EndreBussIkkeOk()
        {


            mockRep.Setup(k => k.EndreBuss(It.IsAny<Busser>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBuss(It.IsAny<Busser>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Bussen ble ikke endret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }



        [Fact]
        public async Task SjekkLoggetInnEndreBuss()
        {


            mockRep.Setup(k => k.EndreBuss(It.IsAny<Busser>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBuss(It.IsAny<Busser>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal("Ikke logget inn!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }








        [Fact]
        public async Task EndreStasjonOk()
        {

            var Stasjon = new Stasjoner { StasjonId = 1, StasjonNavn = "Eidsvoll" };

            mockRep.Setup(k => k.EndreStasjon(Stasjon)).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreStasjon(Stasjon) as OkObjectResult;

            // Assert 
            Assert.Equal("Stasjon ble endret", resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }


        [Fact]
        public async Task EndreStasjonIkkeOk()
        {


            mockRep.Setup(k => k.EndreStasjon(It.IsAny<Stasjoner>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreStasjon(It.IsAny<Stasjoner>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Stasjon ble ikke endret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }



        [Fact]
        public async Task SjekkLoggetInnEndreStasjon()
        {


            mockRep.Setup(k => k.EndreStasjon(It.IsAny<Stasjoner>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreStasjon(It.IsAny<Stasjoner>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal("Ikke logget inn!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }














        [Fact]
        public async Task HentEnRuteOk()
        {


            var Avgangstider = new Avgangstider { TidId = 1, Tid = new TimeSpan(12, 00, 00) };
            var Avgangstider1 = new Avgangstider { TidId = 2, Tid = new TimeSpan(14, 00, 00) };
            var listAvgang = new List<Avgangstider>();

            listAvgang.Add(Avgangstider);
            listAvgang.Add(Avgangstider1);

            var Stasjoner = new Stasjoner { StasjonId = 1, StasjonNavn = "Stovner" };
            var Buss = new Busser { BussId = 1, BussNavn = "Oslo" };
            var Rute = new Ruter { RuteId = 1, Pris = 10 };
            var Avgang = new Avganger { StoppId = 1, Stopp = Stasjoner, Tider = listAvgang, Rute = Rute };
            var Avgang1 = new Avganger { StoppId = 2, Stopp = Stasjoner, Tider = listAvgang, Rute = Rute };
            var listAvganger = new List<Avganger>();

            listAvganger.Add(Avgang);
            listAvganger.Add(Avgang1);

            var bussrute1 = new Buss_Rute { Buss_RuteId = 1, TidFra = new TimeSpan(10, 00, 00), TidTil = new TimeSpan(16, 00, 00), Buss = Buss, Rute = Rute };

            mockRep.Setup(k => k.HentEnRute(It.IsAny<int>())).ReturnsAsync(bussrute1);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.HentEnRute(It.IsAny<int>()) as OkObjectResult;


            // Assert 
            Assert.Equal((Buss_Rute) resultat.Value, bussrute1);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }



        [Fact]
        public async Task HentEnRuteIkkeOk()
        {


       
            mockRep.Setup(k => k.HentEnRute(It.IsAny<int>())).ReturnsAsync(It.IsAny<Buss_Rute>);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.HentEnRute(It.IsAny<int>()) as BadRequestObjectResult;


            // Assert 
            Assert.Equal(resultat.Value, "Rute ble ikke hentet!");
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }



        [Fact]
        public async Task SjekkLoggetInnHentEnRute()
        {


            mockRep.Setup(k => k.HentEnRute(It.IsAny<int>())).ReturnsAsync(It.IsAny<Buss_Rute>);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.HentEnRute(It.IsAny<int>()) as UnauthorizedObjectResult;


            // Assert 
            Assert.Equal(resultat.Value, "Ikke logget inn!");
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }




        [Fact]
        public async Task SlettRuteOk()
        {

            mockRep.Setup(k => k.SlettRute(It.IsAny<int>())).ReturnsAsync(true);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.SlettRute(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal(true, resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }




        [Fact]
        public async Task SlettRuteIkkeOk()
        {

            mockRep.Setup(k => k.SlettRute(It.IsAny<int>())).ReturnsAsync(false);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.SlettRute(It.IsAny<int>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Rute ble ikke slettet!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }


        [Fact]
        public async Task SjekkLoggetInnSlettRute()
        {

            mockRep.Setup(k => k.SlettRute(It.IsAny<int>())).ReturnsAsync(true);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.SlettRute(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal("Ikke logget inn!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }


        [Fact]
        public async Task SjekkLoggetInnOppdaterRetur()
        {

            mockRep.Setup(k => k.OppdaterRetur(It.IsAny<Reise>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;


            // Act
            var resultat = await billettController.OppdaterRetur(It.IsAny<Reise>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal("Ikke logget inn!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }




        [Fact]
        public async Task LeggTilRuteOk()
        {



            mockRep.Setup(k => k.LeggTilRute(It.IsAny<Rute>())).ReturnsAsync(true);


            var billettController = new BillettController(mockRep.Object, mockLog.Object);


            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            var resultat = await billettController.LeggTilRute(It.IsAny<Rute>()) as OkObjectResult;

            // Assert 
            Assert.Equal("Rute ble lagret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }

        [Fact]
        public async Task LeggTilRuteIkkeOk()
        {


            mockRep.Setup(k => k.LeggTilRute(It.IsAny<Rute>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;


            // Act
            var resultat = await billettController.LeggTilRute(It.IsAny<Rute>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Rute ble ikke lagret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }




        [Fact]
        public async Task LeggTilTidOk()
        {

            var Avgang = new Avganger { StoppId = 1 };

            var Tid = new Tid { Id = Avgang.StoppId, Hours = 6, Minutes = 10, Seconds = 0};

            mockRep.Setup(k => k.LeggTilTid(Tid)).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LeggTilTid(Tid) as OkObjectResult;

            // Assert 
            Assert.Equal("Tiden ble lagret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }

        [Fact]
        public async Task LeggTilTidIkkeOk()
        {

            mockRep.Setup(k => k.LeggTilTid(It.IsAny<Tid>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LeggTilTid(It.IsAny<Tid>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal("Tiden ble ikke lagret!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
        }



        [Fact]
        public async Task SjekkLoggetInnLeggTilTid()
        {

            mockRep.Setup(k => k.LeggTilTid(It.IsAny<Tid>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LeggTilTid(It.IsAny<Tid>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal("Ikke logget inn!", resultat.Value);
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }





        [Fact]
        public async Task SjekkLoggetInnLeggTilRute()
        {
            mockRep.Setup(k => k.LeggTilRute(It.IsAny<Rute>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LeggTilRute(It.IsAny<Rute>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn!", resultat.Value);
        }

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }






        [Fact]
        public void LoggUtOk()
        {
            var billettController = new BillettController(mockRep.Object, mockLog.Object);



            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            // Act
            billettController.LoggUt();



            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }


        [Fact]
        public void SjekkloggetInnOk()
        {
            var billettController = new BillettController(mockRep.Object, mockLog.Object);



            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



           billettController.SjekkLoggetIn();

            // Assert 
            Assert.Equal(_loggetInn, mockSession[_loggetInn]);
            // Act
        }

        [Fact]
        public void SjekkloggetInnIkkeOk()
        {
            var billettController = new BillettController(mockRep.Object, mockLog.Object);



            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;



            var result = billettController.SjekkLoggetIn();

            // Assert 
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
            // Act
        }



    }
}
