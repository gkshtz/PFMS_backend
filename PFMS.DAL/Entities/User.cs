using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{ 
    public class User: IIdentifiable
    {
        [Key]
        [Column("userId")]
        public Guid Id { get; set; }

        [Column("firstName")]
        public string FirstName { get; set; }

        [Column("lastName")]
        public string LastName { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
