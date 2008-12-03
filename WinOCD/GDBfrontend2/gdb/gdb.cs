using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;

using GDB_Frontend.Processes;
using GDB_Frontend.Parsing;

namespace GDBfrontend.gdb
{


    public class GDBcmdReplyEventArgs : EventArgs
    {
        string _replyTextData;
        public string  replyTextData
        {
            get { return _replyTextData;  }
        }

        string _sendCmd;
        public string  sendCmd
        {
            get { return _sendCmd; }
        }
       
        public GDBcmdReplyEventArgs(string replyTextData, string sendCmd)
        {
            _replyTextData = replyTextData;
            _sendCmd = sendCmd;

        }
    }


    public class GDBrawReplyEventArgs : EventArgs
    {
      public string output;
      public GDBrawReplyEventArgs(string output)
      {
          this.output = output;
      }

    }

    public class GDBnewLocationEventArgs : EventArgs
    {
        public int address;
        public int linenumber;
        public string function;
        public string filename;
        public string full_filename;
    }

    public class gdb_runner
    {
        public event EventHandler onExit;

        process_runner gdb_Process;
        StringBuilder console_log;
        Queue cmd_FIFO = new Queue();

        //bool _log_console = false;
        bool _startUpDone = false;

        public event EventHandler<GDBcmdReplyEventArgs>    onCMDdone;
        public event EventHandler<GDBcmdReplyEventArgs>    onRunningStart;
        public event EventHandler<GDBcmdReplyEventArgs>    onCMDerror;

        public event EventHandler<GDBnewLocationEventArgs> onStopped;
        public event EventHandler<GDBnewLocationEventArgs> onConnected;

        public event EventHandler onStartupDone;

        public event EventHandler<GDBrawReplyEventArgs> onRawReply;

        public bool RunGDB(GDBConfig cfg)
        {

            if (gdb_Process == null || gdb_Process.HasExited)
            {
                gdb_Process = new process_runner(true, false);

                gdb_Process.onNewStdOutputData += new EventHandler<ConsoleOutputEventArgs>(recv_output);
                gdb_Process.onProcessKill += new EventHandler(GDB_Process_onProcessKill);

                if (gdb_Process.start(@cfg.GDBexePath, cfg.GDBcmdArgs, "") != process_runner.STATUS_CODES.OK)
                {
                    throw new Exception("ErrorStartingGDB");
                }
                else return true;
            }
            else //reset openocd app
            {
                killGDB();
                return RunGDB(cfg);
            }
        }

        void recv_output(object sender, ConsoleOutputEventArgs e)
        {
/*
            if (_log_console)
            {
                if (console_log.Length > 10000)
                    console_log.Remove(0, console_log.Length);

                console_log.Append(e.ConsoleOut);
            }
            */

            if (e.ConsoleOut.Contains("(gdb)")) //End cmd
            {

                if (onRawReply != null)
                    onRawReply(this, new GDBrawReplyEventArgs(e.ConsoleOut));

                foreach (string singleAnswer in e.ConsoleOut.Split(new string[] { "(gdb)" }, StringSplitOptions.None))
                {
                    if(parse_output(singleAnswer))
                       send_nextFIFOcmd();
                }
            }
   
        }

        bool parse_output(string TextData)
        {
            bool sendNextFifoCmd= false;

            if (TextData.Contains("^done")) //last cmd sucessfully done
            {
                string send_cmd = (string)cmd_FIFO.Dequeue(); //cmd entfernen, da bearbeitet.
                sendNextFifoCmd = true;
                if (onCMDdone != null)
                {
                    onCMDdone(this, new GDBcmdReplyEventArgs(TextData, send_cmd)); 
                }
            }
            else if (TextData.Contains("^running"))
            {
                string send_cmd="";
            
                send_cmd = (string)cmd_FIFO.Dequeue();
                sendNextFifoCmd = true;
             
                if (onRunningStart != null)
                {
                    onRunningStart(this, new GDBcmdReplyEventArgs(TextData, send_cmd));
                }
            }
            else if (TextData.Contains("*stopped")) //out of band reply
            {
                if (onStopped!=null)
                {
                    onStopped(this, parseSourceLocation(TextData));
                }
            }
            else if (TextData.Contains("^connected"))
            {
                string send_cmd = (string)cmd_FIFO.Dequeue();
                sendNextFifoCmd = true;

                if (onConnected != null)
                {
                    onConnected(this, parseSourceLocation(TextData));
                }
            }
            else if (TextData.Contains("^error"))
            {
                if (cmd_FIFO.Count != 0)
                {
                    string send_cmd = (string)cmd_FIFO.Dequeue();
                    sendNextFifoCmd = true;

                    if (onCMDerror != null)
                    {
                        onCMDerror(this, new GDBcmdReplyEventArgs(TextData, send_cmd));
                    }
                }
                else
                {
                    if (onCMDerror != null)
                    {
                        onCMDerror(this, new GDBcmdReplyEventArgs(TextData, "Out of Band:"));
                    }
                }
               // reply.error_message = parser.extract_data_betweenPatterns(data, "msg=\"", "\r");
            }
            else if (!_startUpDone)
            {
                _startUpDone = true;
                if (onStartupDone != null) onStartupDone(this, EventArgs.Empty);
            }

            return sendNextFifoCmd;
        }

        public void send_cmd(string cmd)
        {
            cmd_FIFO.Enqueue(cmd);
            if (cmd_FIFO.Count == 1)
            {
                gdb_Process.send_input(cmd);
            }
        }

        void send_nextFIFOcmd()
        {
            if (cmd_FIFO.Count != 0)
            {
                gdb_Process.send_input( (string)cmd_FIFO.Peek() );
            }
        }


        GDBnewLocationEventArgs parseSourceLocation(string data)
        {

            GDBnewLocationEventArgs loc = new GDBnewLocationEventArgs();
            string dummy;

            loc.function = ParseTools.extract_data_betweenPatterns(data, "func=\"", "\"");
            loc.filename = ParseTools.extract_data_betweenPatterns(data, "file=\"", "\"");
            loc.full_filename = ParseTools.extract_data_betweenPatterns(data, "fullname=\"", "\"");


            dummy = ParseTools.extract_data_betweenPatterns(data, "line=\"", "\"");
            if (dummy != "")
                loc.linenumber = int.Parse(dummy);
            else
                loc.linenumber = -1;

            dummy = ParseTools.extract_data_betweenPatterns(data, "addr=\"0x", "\"");
            if (dummy != "")
                loc.address = int.Parse(dummy, System.Globalization.NumberStyles.HexNumber);
            else
                loc.address = -1;

            return loc;
        }

     /*   public bool log_console
        {
            get { return _log_console; }
            set
            {
                console_log = new StringBuilder(10000);
                _log_console = true;
            }
        }
      */

        public void killGDB()
        {
            gdb_Process.kill_process();
        }

        void GDB_Process_onProcessKill(object sender, EventArgs e)
        {
            if (onExit != null)
            {
                onExit(this, EventArgs.Empty);
            }
            
        }
        

    }
}
