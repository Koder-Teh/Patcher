using Patcher.Other;
using Patcher.Patching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patcher
{
    internal class Enviroment
    {
        public Args _args { get; set; }

        public Template _template { get; set; }

        public HandlerFile _handler { get; set; }
    }
}
