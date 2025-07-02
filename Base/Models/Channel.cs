using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name = "channel")]
    public class Channel : Entity<Channel>
    {
        public Channel(string name)
        {
            Name = name;
        }

        private Channel() {}

        public string Name { get; private set; } = null!;
    }
}
