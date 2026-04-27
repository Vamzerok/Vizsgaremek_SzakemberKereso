using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("users")]
    public class User : IdentityUser<int>
    {
        [MaxLength(32)]
        public string FirstName { get; set; } = null!;
        [MaxLength(32)]
        public string LastName { get; set; } = null!;

        [InverseProperty("User")]
        public Expert? Expert { get; set; }

        [InverseProperty("InitiatingUser")]
        public ICollection<Job> InitiatedJobs { get; set; } = new List<Job>();
    }
}
