namespace TechExpoWorld.Common
{
    public static class GlobalConstants
    {
        public static class System
        {
            public const string Name = "TechExpoWorld";
        }

        public static class Admin
        {
            public const string AreaName = "Administration";
            public const string RoleName = "Administrator";
            public const string Email = "admin@tew.com";
            public const string Password = "admin1";
        }

        public static class Cache
        {
            public const string LatestStatisticsAndNewsArticlesCacheKey = nameof(LatestStatisticsAndNewsArticlesCacheKey);
        }

        public static class TempData
        {
            public const string GlobalMessageKey = nameof(GlobalMessageKey);

            public const string CreatedAuthor = "Thank you for becomming an author!";

            public const string CreatedNewsArticle = "Your news article was added successfully!";
            public const string EditedNewsArticle = "Your news article was edited successfully!";
            public const string DeletedNewsArticle = "Your news article was deleted successfully!";

            public const string CreatedComment = "Your comment was added successfully!";

            public const string CreatedEvent = "Your event was added successfully!";
            public const string EditedEvent = "Your event was edited successfully!";
            public const string DeletedEvent = "Your event was deleted successfully!";

            public const string CreatedAttendee = "Thank you for becomming an attendee!";

            public const string BookedTicket = "You have booked a ticket successfully!";
            public const string CancelledTicket = "You have cancelled a ticket successfully!";
        }

        public static class NewsArticle
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
            public const int AuthorIdMaxLength = 68;

            public const string DisplaySearchByText = "Search by text";
            public const string DisplaySortByDate = "Sort by date";
            public const string DisplayImageUrl = "Image URL";
            public const string DisplaySelectCategory = "Select Category";
            public const string DisplaySelectTags = "Select Tags";

            public const string ErrorCategory = "News category does not exist.";
            public const string ErrorTag = "Tag option does not exist.";
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class Tag
        {
            public const int NameMaxLength = 50;
        }

        public static class Comment
        {
            public const int ContentMinLength = 2;
            public const int ContentMaxLength = 500;
            public const int ApplicationUserIdMaxLength = 68;

            public const string DisplayAddAComment = "Add a comment";
        }

        public static class Author
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 80;
            public const int ApplicationUserIdMaxLength = 68;

            public const string DisplayPhoneNumber = "Phone Number";
            public const string DisplayPhotoUrl = "Photo URL";
        }

        public static class Event
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
            public const int LocationMinLength = 2;
            public const int LocationMaxLength = 100;
            public const int ApplicationUserIdMaxLength = 68;

            public const int TicketCountMinRange = 1;
            public const int TicketCountMaxRange = 2000;
            public const int TicketPriceMinRange = 0;
            public const int TicketPriceMaxRange = 100000;

            public const string DisplayStartDate = "Start Date";
            public const string DisplayEndDate = "End Date";
            public const string DisplayTotalPhysicalTickets = "Total Physical Tickets";
            public const string DisplayTotalVirtualTickets = "Total Virtual Tickets";
            public const string DisplayPricePhysical = "Price - Physical";
            public const string DisplayPriceVirtual = "Price - Virtual";

            public const string DisplayFormatDateTime = "{0:yyyy-MM-dd HH:mm}";
        }

        public static class Ticket
        {
            public const int TypeMaxLength = 20;
            public const int AttendeeIdMaxLength = 68;

            public const int PricePrecision = 8;
            public const int PriceScale = 2;
        }

        public static class Attendee
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int WorkEmailMinLength = 6;
            public const int WorkEmailMaxLength = 40;
            public const int JobTitleMinLength = 2;
            public const int JobTitleMaxLength = 50;
            public const int CompanyNameMinLength = 2;
            public const int CompanyNameMaxLength = 50;
            public const int ApplicationUserIdMaxLength = 68;

            public const string DisplayPhoneNumber = "Phone Number";
            public const string DisplayWorkEmail = "Work Email";
            public const string DisplayJobTitle = "Job Title";
            public const string DisplayCompanyName = "Company Name";
            public const string DisplayCountry = "Country";
            public const string DisplayJobType = "Job Type";
            public const string DisplayCompanyType = "Company Type";
            public const string DisplayCompanySector = "Company Sector";
            public const string DisplayCompanySize = "Company Size";

            public const string ErrorCountry = "Country does not exist.";
            public const string ErrorJobType = "Job type does not exist.";
            public const string ErrorCompanyType = "Company type does not exist.";
            public const string ErrorCompanySector = "Company sector does not exist.";
            public const string ErrorCompanySize = "Company size does not exist.";
        }

        public static class Country
        {
            public const int NameMaxLength = 50;
        }

        public static class JobType
        {
            public const int NameMaxLength = 50;
        }

        public static class CompanyType
        {
            public const int NameMaxLength = 50;
        }

        public static class CompanySector
        {
            public const int NameMaxLength = 80;
        }

        public static class CompanySize
        {
            public const int NameMaxLength = 50;
        }
    }
}
