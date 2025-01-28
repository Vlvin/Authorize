
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Authorize.DBModels {


    [PrimaryKey(nameof(Id), nameof(Email))]
    public class User {
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? Salt { get; set; }

        [Column(TypeName = "char(64)")]
        public string? Hash { get; set; }

    }
}