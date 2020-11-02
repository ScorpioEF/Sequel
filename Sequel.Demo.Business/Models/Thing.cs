using System;
using System.Collections.Generic;
using System.Text;

namespace Sequel.Demo.Business.Models
{
    public class Thing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<OtherStuff> OtherStuffs { get; set; }
    }
}
