using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botcode
{
    class Events
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public List<string>? Labels { get; set; }
        public string? Status { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public Events(string owner, string name, string? category, string? location, List<string>? labels, string? status, DateTime? begin, DateTime? end)
        {
            Owner = owner;
            Name = name;
            Category = category;
            Location = location;
            Labels = labels;
            Status = status;
            Begin = begin;
            End = end;

        }

    }

}

