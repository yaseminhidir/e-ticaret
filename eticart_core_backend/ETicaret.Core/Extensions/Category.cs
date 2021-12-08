using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class Category
    {
        [NotMapped] //Db'de böyle bi alan olmadığı içins
        public string FullName { get; set; }
    }
}
