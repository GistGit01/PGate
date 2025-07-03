using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name ="customer")]
    public class Customer:Entity<Customer>
    {
        private Customer() {}

        [Column(StringLength = 100)]
        public string Name { get; private set; }

        [Column(StringLength = 100)]
        public string Email { get; private set; }

        [Column(StringLength = 50)]
        public string Password { get; private set; }

        [Column(StringLength = 50)]
        public string SecretKey { get; private set; }
    }
}
