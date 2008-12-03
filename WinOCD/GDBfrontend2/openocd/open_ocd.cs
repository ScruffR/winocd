using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;


using Telnet; //Telnet lib
using GDB_Frontend.Processes;
using GDB_Frontend.Parsing;
using GDBfrontend.openocd;

namespace GDB_Frontend.OpenOcd
{


    public class OpenOCDOutputEventArgs : EventArgs
    {
        public string InfoMessage
        {
            get { return _InfoMsg; }
        }
        string _InfoMsg;
        public OpenOCDOutputEventArgs(string info_msg)
        {
            _InfoMsg = info_msg;
            
        }
    }



    public class OpenOCD_runner
    {
       
        process_runner   OpenOCD_Process;
        StringBuilder    recv_data = new StringBuilder(500); // Unparsed Answer Data
        StringBuilder    console_log;
        bool             _log_console    = false;
        int              _OpenOCDversion = 0;
        Terminal _TelnetClient = new Terminal("localhost", 4444, 5, 100, 50);

        public event EventHandler<OpenOCDOutputEventArgs> onInfo;
        public event EventHandler<OpenOCDOutputEventArgs> onError;
        public event EventHandler onExit;

        public bool gdb_connected = false;
        
        public int OpenOCD_version
        {
            get { return _OpenOCDversion; }
        }

        public bool log_console
        {
            get { return _log_console; }
            set 
            {
              console_log  = new StringBuilder(10000);
              _log_console = true;
            }
        }
  
     

        public bool RunOpenOCD(openOCDConfig cfg, bool flash_cfg)
        {
          
                if (OpenOCD_Process == null || OpenOCD_Process.HasExited)
                {
                    OpenOCD_Process = new process_runner(false, true);

                    OpenOCD_Process.onNewErrorOutputData += new EventHandler<ConsoleOutputEventArgs>(recv_output);
                    OpenOCD_Process.onProcessKill += new EventHandler(OpenOCD_Process_onProcessKill);
                    if (flash_cfg)
                    {
                        if (OpenOCD_Process.start(@cfg.OCDexePath, " '-f " + cfg.OCDcfgFlashPath + "'", "") != process_runner.STATUS_CODES.OK)
                        {
                            throw new Exception("ErrorStartingOpenOCD");
                        }
                        else return true;
                    }
                    else
                    {
                        if (OpenOCD_Process.start(@cfg.OCDexePath, " '-f " + cfg.OCDcfgPath + "'", "") != process_runner.STATUS_CODES.OK)
                        {
                            throw new Exception("ErrorStartingOpenOCD");
                        }
                        else
                        {
                          if (!_TelnetClient.Connect())
                            {
                              //  throw new Exception("ErrorConncetingTelnet");
                            }
                           
                            return true;
                        }
                    }
                }
                else //reset openocd app
                {
                    killOpenOCD();
                    return RunOpenOCD(cfg, flash_cfg);
                }
 
        }


        public bool send_cmd(string OpenOCDcmd)
        {
            return _TelnetClient.SendResponse(OpenOCDcmd, true); // MessageBox.Show(_TelnetClient.VirtualScreen.Hardcopy());
        }
  

        public string get_console_log()
        {
            if (_log_console)
            {
                string answer = console_log.ToString();
                console_log.Remove(0, console_log.Length); //log clearen
                return answer;
            }
            else return "";
        }

        public void killOpenOCD()
        {
            _TelnetClient.Close();
            OpenOCD_Process.kill_process();
        }

        public void send_input(string input)
        {
            OpenOCD_Process.send_input(input);
        }



        void recv_output(object sender, ConsoleOutputEventArgs e)
        {

            if (_log_console)
            {
                if (console_log.Length > 10000)
                    console_log.Remove(0, console_log.Length);

                console_log.Append(e.ConsoleOut);
            }

            if (e.ConsoleOut.Contains("\n")) //End of line
            {
                   recv_data.Append(e.ConsoleOut);
                   string[] output_lines = recv_data.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                   
                   foreach (string line in output_lines)
                      parse_output_line(line);
          
                   recv_data.Remove(0, recv_data.Length);
            }
            else recv_data.Append(e.ConsoleOut);
        }

        void parse_output_line(string line)
        {
          
            if (line.Contains("svn:")) //last cmd sucessfully done
            {
                string dummy = ParseTools.extract_data_fromPatternToEnd(line,"svn:");
                try
                {
                    _OpenOCDversion = int.Parse(dummy, System.Globalization.NumberStyles.Integer);
                }
                catch
                {
                } 
            }
            else if (line.Contains("Error:"))
            {
                string dummy = ParseTools.extract_data_fromPatternToEnd(line, "Error:");
                try
                {
                    killOpenOCD();
                    if (onError != null)
                        onError(this, new OpenOCDOutputEventArgs(ParseTools.extract_data_fromPatternToEnd(line, "Error:")));
                }
                catch
                {
                    Debug.WriteLine("Error extracting ErrorData...");
                }
            }
            else if (line.Contains("Info:"))
            {
                string dummy = ParseTools.extract_data_fromPatternToEnd(line, "Info:");
                try
                {
                    if (onInfo != null)
                        onInfo(this, new OpenOCDOutputEventArgs(ParseTools.extract_data_fromPatternToEnd(line, "Info:")));
                }
                catch
                {
                    Debug.WriteLine("Error extracting InfoData...");
                }
            }
       
        }

        void OpenOCD_Process_onProcessKill(object sender, EventArgs e)
        {
            if (onExit != null)
            {
                onExit(this, EventArgs.Empty);
            }
            
        }


    }



  


    /*
    class open_ocd_config_maker
    {

        public enum STATUS_CODES { OK, FILE_IO_ERROR};
        public enum TARGET_DEVICE { ATMEL_ARM7_SAM7S };
        public open_ocd_config_maker(TARGET_DEVICE device)
        {
            switch (device)
            {
                case TARGET_DEVICE.ATMEL_ARM7_SAM7S:
                 //Use Default Values...
                    
                break;
            }
        }

        public open_ocd_config_maker(string existing_config_file)
        {
            m_cfg_filename = existing_config_file;
        }



        
        // Dämon Config
        private int m_telnet_port = 4444;
        private int m_gdb_port = 3333;
       
        // Interface Options
        private string m_interface = "ft2232";
        private string m_ft2232_device_desc = "\"Olimex OpenOCD JTAG TINY A\"";
        private string m_ft2232_layout = "\"olimex-jtag\"";
        private string m_ft2232_vid_pid = "0x15ba 0x0004";
        private int m_jtag_speed = 2;
        private int m_jtag_nsrst_delay = 200;
        private int m_jtag_ntrst_delay = 200;

        // use combined on interfaces or targets that can't set TRST/SRST separately
        private string m_reset_config = "srst_only srst_pulls_trst"; // #reset_config trst_and_srst separate

        // jtag scan chain
        private string m_jtag_device = "4 0x1 0xf 0xe";
       
        // target configuration
        private string m_daemon_startup = "reset";
        private string m_run_and_halt_time = "0 30";


        //target <type> <startup mode>
        //target arm7tdmi <reset mode> <chainpos> <endianness> <variant>
        private string m_target = "arm7tdmi little run_and_init 0 arm7tdmi";

        // target_script 0 reset sam7flash.script.txt
        // working_area 0 0x00200000 0x4000 nobackup
        // flash bank at91sam7 0 0 0 0 0
        // target_script 0 reset sam7s_reset.script
        private string m_working_area = "0 0x00200000 0x4000 nobackup";
        private string m_flash_bank = "at91sam7 0 0 0 0 0";

        private string m_open_ocd_filename = "openocd-ftd2xx.exe";
        private string m_cfg_filename = "openocd.cfg";

        public string get_openocd_exe_name()
        {
            return m_open_ocd_filename;
        }

        public string get_config_filename()
        {
            return m_cfg_filename;
        }


        public STATUS_CODES generate_config_file(string filename)
        {
            StreamWriter cfg_file;
            
            try
            {
               cfg_file = new StreamWriter(filename, false);
               m_cfg_filename = filename;
            }
            catch { return STATUS_CODES.FILE_IO_ERROR; }

            cfg_file.WriteLine("#Open_OCD Config File");
            cfg_file.WriteLine("telnet_port "+m_telnet_port);
            cfg_file.WriteLine("gdb_port "+m_gdb_port);
            cfg_file.WriteLine(" ");
            cfg_file.WriteLine("interface " + m_interface);
            cfg_file.WriteLine("ft2232_device_desc " + m_ft2232_device_desc);
            cfg_file.WriteLine("ft2232_layout " + m_ft2232_layout);
            cfg_file.WriteLine("ft2232_vid_pid " + m_ft2232_vid_pid);
            cfg_file.WriteLine("jtag_speed  " + m_jtag_speed);
            cfg_file.WriteLine("jtag_nsrst_delay " + m_jtag_nsrst_delay);
            cfg_file.WriteLine("jtag_ntrst_delay " + m_jtag_ntrst_delay);
            cfg_file.WriteLine("reset_config " + m_reset_config);
            cfg_file.WriteLine(" ");
            cfg_file.WriteLine("jtag_device " + m_jtag_device);
            cfg_file.WriteLine("target " + m_target );
            cfg_file.WriteLine("daemon_startup " + m_daemon_startup);
            cfg_file.WriteLine("run_and_halt_time " + m_run_and_halt_time);
            cfg_file.WriteLine("flash bank " + m_flash_bank);
            cfg_file.WriteLine("working_area " + m_working_area);

            
            cfg_file.Close();

            return STATUS_CODES.OK;
        }

    }
    */

}
