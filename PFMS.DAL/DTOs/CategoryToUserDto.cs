using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.Entities;

namespace PFMS.DAL.DTOs
{
    public class CategoryToUserDto
    {
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}
