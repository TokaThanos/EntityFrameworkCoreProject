using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
