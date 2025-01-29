
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorize.DBModels {


    [PrimaryKey(nameof(Id), nameof(Email))]
    public class User : IdentityUser {

    }
}