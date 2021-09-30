namespace TechExpoWorld.Test.Pipeline
{
    using System.Collections.Generic;
    using MyTested.AspNetCore.Mvc;
    using TechExpoWorld.Controllers;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Events.Models;
    using Xunit;

    using static Data.Events;

    public class EventsControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap("/Events/All")
                .To<EventsController>(c => c.All())
                .Which(controller => controller
                    .WithData(TenEvents))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<EventServiceModel>>());

        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap("/Events/Details/1/Event")
                .To<EventsController>(c => c.Details(1, "Event"))
                .Which(controller => controller
                    .WithData(OneEvent))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EventDetailsViewModel>()
                    .Passing(model =>
                    {
                        Assert.Equal(10, model.EventDetails.TotalPhysicalTickets);
                        Assert.Equal(10, model.EventDetails.TotalVirtualTickets);
                    }));

        [Fact]
        public void MyTicketsShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Events/MyTickets")
                    .WithUser())
                .To<EventsController>(c => c.MyTickets())
                .Which(controller => controller
                    .WithData(new Ticket { Attendee = new Attendee { UserId = TestUser.Identifier } }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MyTicketsViewModel>());

        [Fact]
        public void BuyPhysicalTicketShouldReturnRedirectToAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Events/BuyPhysicalTicket/1")
                    .WithUser())
                .To<EventsController>(c => c.BuyPhysicalTicket(1))
                .Which(controller => controller
                    .WithData(new Ticket
                    {
                        Attendee = new Attendee { UserId = TestUser.Identifier }
                    })
                    .WithData(new Event { Id = 1 }))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<EventsController>(c => c.All()));

        [Fact]
        public void BuyVirtualTicketShouldReturnRedirectToAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Events/BuyVirtualTicket/1")
                    .WithUser())
                .To<EventsController>(c => c.BuyVirtualTicket(1))
                .Which(controller => controller
                    .WithData(new Ticket
                    {
                        Attendee = new Attendee { UserId = TestUser.Identifier }
                    })
                    .WithData(new Event { Id = 1 }))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<EventsController>(c => c.All()));
    }
}
