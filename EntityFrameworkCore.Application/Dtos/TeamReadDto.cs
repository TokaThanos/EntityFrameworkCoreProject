﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class TeamReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
