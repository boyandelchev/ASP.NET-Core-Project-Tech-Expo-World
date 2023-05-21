namespace TechExpoWorld
{
    public class GlobalConstants
    {
        public class TempData
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

        public class Cache
        {
            public const string LatestStatisticsAndNewsArticlesCacheKey = nameof(LatestStatisticsAndNewsArticlesCacheKey);
        }

        public class User
        {
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Admin
        {
            public const string AreaName = "Admin";
            public const string AdministratorRoleName = "Administrator";
            public const string AdminEmail = "admin@tew.com";
            public const string AdminPassword = "admin1";
        }

        public class NewsArticle
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
            public const int AuthorIdLength = 36;

            public const string DisplaySearchByText = "Search by text";
            public const string DisplaySortByDate = "Sort by date";
            public const string DisplayImageUrl = "Image URL";
            public const string DisplaySelectCategory = "Select Category";
            public const string DisplaySelectTags = "Select Tags";

            public const string ErrorCategory = "News category does not exist.";
            public const string ErrorTag = "Tag option does not exist.";
        }

        public class Category
        {
            public const int NameMaxLength = 50;
        }

        public class Tag
        {
            public const int NameMaxLength = 50;
        }

        public class Author
        {
            public const int IdLength = 36;
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 80;

            public const string DisplayPhoneNumber = "Phone Number";
            public const string DisplayPhotoUrl = "Photo URL";
        }

        public class Comment
        {
            public const int ContentMinLength = 2;
            public const int ContentMaxLength = 500;

            public const string DisplayAddAComment = "Add a comment";
        }

        public class Event
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
            public const int LocationMinLength = 2;
            public const int LocationMaxLength = 100;

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

        public class Attendee
        {
            public const int IdLength = 36;
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

        public class Ticket
        {
            public const int TypeMaxLength = 20;
            public const int PricePrecision = 8;
            public const int PriceScale = 2;
        }

        public class Country
        {
            public const int NameMaxLength = 50;
        }

        public class JobType
        {
            public const int NameMaxLength = 50;
        }

        public class CompanyType
        {
            public const int NameMaxLength = 50;
        }

        public class CompanySector
        {
            public const int NameMaxLength = 80;
        }

        public class CompanySize
        {
            public const int NameMaxLength = 50;
        }
    }
}
