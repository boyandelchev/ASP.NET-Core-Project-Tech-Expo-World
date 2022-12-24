namespace TechExpoWorld.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class NewsArticle
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
        }

        public class NewsCategory
        {
            public const int NameMaxLength = 50;
        }

        public class Tag
        {
            public const int NameMaxLength = 50;
        }

        public class Author
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 80;
        }

        public class Comment
        {
            public const int ContentMinLength = 2;
            public const int ContentMaxLength = 500;
        }

        public class Event
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 50000;
            public const int LocationMinLength = 2;
            public const int LocationMaxLength = 100;
        }

        public class Attendee
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
            public const int CountryMinLength = 2;
            public const int CountryMaxLength = 50;
        }

        public class Ticket
        {
            public const int TypeMaxLength = 20;
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
