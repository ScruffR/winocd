using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDBfrontend.gdb
{
    public class GDBConfig
    {
        string _GDBcmdArgs;
        public string GDBcmdArgs
        {
            get { return _GDBcmdArgs; }
            set { _GDBcmdArgs = value; }
        }

        string _GDBexePath;
        public string GDBexePath
        {
            get { return _GDBexePath; }
            set { _GDBexePath = value; }
        }

        public GDBConfig(string GDBcmdArgs, string GDBexePath)
        {
            _GDBcmdArgs = GDBcmdArgs;
            _GDBexePath = GDBexePath;

        }

    }
}
