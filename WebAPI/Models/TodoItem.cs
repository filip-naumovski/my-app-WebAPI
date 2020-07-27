using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TodoItem
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string chore { get; set; }
        [Column(TypeName = "bit")]
        public bool status { get; set; }

    }
}
