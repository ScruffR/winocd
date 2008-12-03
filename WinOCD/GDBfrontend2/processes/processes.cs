using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Threading;


namespace GDB_Frontend.Processes
{
    public class ConsoleOutputEventArgs : EventArgs
    {
       string _ConsoleOut;
       public string ConsoleOut
       {
           get { return _ConsoleOut; }
       }

       public ConsoleOutputEventArgs(string ConsoleOut)
       {
           _ConsoleOut = ConsoleOut;
       }
    }

    class process_runner
    {
        public enum STATUS_CODES { OK, ALLREADY_STARTED, UNKNOWN_PROG };

        public event EventHandler<ConsoleOutputEventArgs> onNewStdOutputData;
        public event EventHandler<ConsoleOutputEventArgs> onNewErrorOutputData;
        public event EventHandler onProcessKill;
 
        private Process my_proc = new Process();
        private volatile bool proc_started = false;
        private bool stdOut_read;
        private bool stdErr_read;
        private Thread ReadstdOut_Thread;
        private Thread ReadstdError_Thread;

        public process_runner(bool rd_stdOut, bool rd_stdErr)
        {
            stdOut_read = rd_stdOut;
            stdErr_read = rd_stdErr;
            if(stdOut_read)
            ReadstdOut_Thread = new Thread(read_stdOut);
            if(stdErr_read)
            ReadstdError_Thread = new Thread(read_stdErr);
        }

    

        void read_stdOut()
        {
            char[] output = new char[10000];
            int cnt;
            
            while (!my_proc.HasExited || !my_proc.StandardOutput.EndOfStream)
            {
                cnt = my_proc.StandardOutput.Read(output, 0, 10000); //wartet/blockiert bis min 1 und max 10.000 Zeichen gelesen worden

                    if (onNewStdOutputData != null)
                     onNewStdOutputData(this, new ConsoleOutputEventArgs(new string(output, 0, cnt)));
                 
            }

            proc_started = false;
        }

        void read_stdErr()
        {
            char[] output_err = new char[10000];
            int cnt;

            while (!my_proc.HasExited || !my_proc.StandardError.EndOfStream)
            {
                cnt = my_proc.StandardError.Read(output_err, 0, 10000); //wartet/blockiert bis min 1 und max 10.000 Zeichen gelesen worden
        
                    if (onNewErrorOutputData != null)
                     onNewErrorOutputData(this, new ConsoleOutputEventArgs(new string(output_err, 0, cnt)));
            }
            proc_started = false;
        }
        


        /// <summary>
        /// Starts an commandline Tool in the Background and allows to read its output while it´s running.
        /// </summary>
        /// <param name="filename">Command Line Tool to start</param>
        /// <param name="arguments">Arguments</param>
        /// <param name="path">Tools Working Directory</param>
        /// <returns>Status Code</returns>
        public STATUS_CODES start(string filename, string arguments, string path)
        {
            if (!proc_started)
            {
                my_proc.Exited += new EventHandler(onExit);
                my_proc.StartInfo.FileName = filename;
                my_proc.StartInfo.WorkingDirectory = path;
                my_proc.StartInfo.UseShellExecute = false;
                my_proc.StartInfo.CreateNoWindow = true;
                my_proc.StartInfo.RedirectStandardInput = true;

                if (stdOut_read)
                   my_proc.StartInfo.RedirectStandardOutput = true;
                
                if (stdErr_read)
                   my_proc.StartInfo.RedirectStandardError = true;

                my_proc.StartInfo.Arguments = arguments;
          
                try
                {
                    proc_started = true; 
                    my_proc.Start();
                }
                catch
                {
                    proc_started = false;
                    return STATUS_CODES.UNKNOWN_PROG;
                }

                if (stdOut_read)
                   ReadstdOut_Thread.Start();
                
                if(stdErr_read)
                   ReadstdError_Thread.Start();
  
                return STATUS_CODES.OK;
            }
            return STATUS_CODES.ALLREADY_STARTED;
        }

   

        /// <summary>
        /// Sends input to the command line Tool.
        /// </summary>
        /// <param name="input">Input Data</param>
        public void send_input(string input)
        {
            if (proc_started)
            {
                my_proc.StandardInput.WriteLine(input);
                
              
            }
        }

        /// <summary>
        /// Kills the command line Tool in the Background
        /// </summary>
        public void kill_process()
        {
            if (proc_started)
            {
                if (!my_proc.HasExited)
                {
                    my_proc.Kill();
                    my_proc.WaitForExit();
                    
                }
                proc_started = false;
            }
        }

       
        public bool HasExited
        {
            get { return my_proc.HasExited; }  
        }

        private void onExit(object sender, EventArgs e)
        {
            if (onProcessKill != null)
                onProcessKill(this, EventArgs.Empty);
        }


    }


}
