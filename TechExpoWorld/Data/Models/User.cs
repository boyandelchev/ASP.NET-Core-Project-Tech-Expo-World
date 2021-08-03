namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
