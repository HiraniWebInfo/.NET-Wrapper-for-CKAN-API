﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CkanDotNet.Api.Model
{
    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string State { get; set; }
        public string RevisionId { get; set; }
        public List<string> Packages { get; set; }
        public GroupExtras Extras { get; set; }
    }
}