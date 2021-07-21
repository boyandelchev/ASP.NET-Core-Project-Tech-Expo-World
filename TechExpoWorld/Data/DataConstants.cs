namespace TechExpoWorld.Data
{
    public class DataConstants
    {
        public class NewsArticle
        {
            public const int TitleMaxLength = 100;
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
            public const int NameMaxLength = 60;
            public const int PhoneNumberMaxLength = 30;
            public const int AddressMaxLength = 80;
        }
    }
}
