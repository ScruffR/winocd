using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GDB_Frontend.OpenOcd;
using GDBfrontend.gdb;
using GDBfrontend.controls;
using GDBfrontend.SyntaxEditor;


namespace GDBfrontend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenOCD_runner OpenOCDinstance = new OpenOCD_runner();
        gdb_runner GDBinstance = new gdb_runner();

        
        private void Form1_Load(object sender, EventArgs e)
        {
            openOCDstatusControl1.InitControl(OpenOCDinstance);
            gdBstatusControl1.InitControl(GDBinstance);
            syntaxEditorControl1.InitControl(GDBinstance);
            try
            {
                OpenOCDinstance.RunOpenOCD(openOCDconfig1.cfg, false);
            }
            catch { MessageBox.Show("Cannot open OpenOCD. Paths ok?","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation); }
            GDBinstance.RunGDB(new GDBConfig("--interpreter=mi -silent", "arm-elf-gdb.exe"));
       
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenOCDinstance.killOpenOCD();
            GDBinstance.killGDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenOCDinstance.RunOpenOCD(openOCDconfig1.cfg, false);
        }

        private void openOCDstatusControl1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GDBinstance.send_cmd("-exec-continue");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenOCDinstance.send_cmd("halt");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            GDBinstance.send_cmd("-exec-next");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            GDBinstance.send_cmd("-exec-step");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            //GDBinstance.send_cmd("-interpreter-exec console \"add-symbol-file test.elf 0x00100000\"");

            GDBinstance.send_cmd("-file-exec-and-symbols test.elf");
            GDBinstance.send_cmd("-target-select remote localhost:3333");
            GDBinstance.send_cmd("-interpreter-exec console \"monitor arm7_9 force_hw_bkpts enable\"");

           // OpenOCDinstance.send_cmd("reset");
             
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GDBinstance.send_cmd("-target-download");
            OpenOCDinstance.send_cmd("reset");
    
            //OpenOCDinstance.send_cmd("flash write_image test.elf");
    
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            GDBinstance.send_cmd("-exec-finish");
       
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            OpenOCDinstance.send_cmd("reset");
        }
    }
}
