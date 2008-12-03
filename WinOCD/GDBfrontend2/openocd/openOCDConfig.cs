using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDBfrontend.openocd
{
    public class openOCDConfig
    {


        public openOCDConfig()
        {
            _loaded = false;
        }

        bool _loaded;
        public bool loaded
        {
            get { return _loaded; }
            set { _loaded = value; }
        }

        string _OCDexePath;
        public string OCDexePath
        {
            get { return _OCDexePath; }
            set { _OCDexePath = value; }
        }

        string _OCDcfgPath;
        public string OCDcfgPath
        {
            get { return _OCDcfgPath; }
            set { _OCDcfgPath = value; }
        }

        string _OCDcfgFlashPath;
        public string OCDcfgFlashPath
        {
            get { return _OCDcfgFlashPath; }
            set { _OCDcfgFlashPath = value; }
        }
    }

   

}
