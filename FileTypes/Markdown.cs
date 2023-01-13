﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Markdown : FileType
    {
        public override string Id { get; } = "Markdown";
        public override string[] Extensions { get; } = { "md" };
    }
}
