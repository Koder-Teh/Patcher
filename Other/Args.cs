using System;
using System.Collections.Generic;
using System.Text;

namespace Patcher.Other
{
    internal class Args
    {
        public string[] args { get; set; }

        public FileInfo file { get; set; }

        public FileInfo file_config { get; set; }

        public bool options_mask = false;

        public void UpdateOptions()
        {
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--mask":
                        {
                            options_mask = true;
                            break;
                        }
                }
            }
        }
    }
}
