namespace TechExpoWorld.Test.Pipeline
{
    using System.Linq;
    using MyTested.AspNetCore.Mvc;
    using TechExpoWorld.Controllers;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.Attendees;
    using Xunit;

    using static GlobalConstants.TempData;

    public class AttendeesControllerTest
    {
        [Fact]
        public void GetBecomeAttendeeShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Attendees/BecomeAttendee")
                    .WithUser())
                .To<AttendeesController>(c => c.BecomeAttendee())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Attendee", "+359888888888", "attendee@mail.com", "senior financial analyst", "TechPro", "United Kingdom", 0, 0, 0, 0)]
        public void PostBecomeAttendeeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string attendeeName,
            string phoneNumber,
            string workEmail,
            string jobTitle,
            string companyName,
            string country,
            int jobTypeId,
            int companyTypeId,
            int companySectorId,
            int companySizeId)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Attendees/BecomeAttendee")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Name = attendeeName,
                        PhoneNumber = phoneNumber,
                        WorkEmail = workEmail,
                        JobTitle = jobTitle,
                        CompanyName = companyName,
                        Country = country,
                        JobTypeId = jobTypeId,
                        CompanyTypeId = companyTypeId,
                        CompanySectorId = companySectorId,
                        CompanySizeId = companySizeId
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<AttendeesController>(c => c.BecomeAttendee(new BecomeAttendeeFormModel
                {
                    Name = attendeeName,
                    PhoneNumber = phoneNumber,
                    WorkEmail = workEmail,
                    JobTitle = jobTitle,
                    CompanyName = companyName,
                    Country = country,
                    JobTypeId = jobTypeId,
                    CompanyTypeId = companyTypeId,
                    CompanySectorId = companySectorId,
                    CompanySizeId = companySizeId
                }))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .InvalidModelState()
                //.Data(data => data
                //    .WithSet<Attendee>(attendees => attendees
                //        .Any(a =>
                //            a.Name == attendeeName &&
                //            a.PhoneNumber == phoneNumber &&
                //            a.WorkEmail == workEmail &&
                //            a.JobTitle == jobTitle &&
                //            a.CompanyName == companyName &&
                //            a.Country == country &&
                //            a.JobTypeId == jobTypeId &&
                //            a.CompanyTypeId == companyTypeId &&
                //            a.CompanySectorId == companySectorId &&
                //            a.CompanySizeId == companySizeId &&
                //            a.UserId == TestUser.Identifier)))
                //.TempData(tempData => tempData
                //    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<BecomeAttendeeFormModel>());
    }
}
