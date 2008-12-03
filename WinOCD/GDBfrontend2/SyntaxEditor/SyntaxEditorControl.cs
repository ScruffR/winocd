using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Alsing.Windows.Forms.SyntaxBox;
using Alsing.Windows.Forms;
using GDBfrontend.gdb;

namespace GDBfrontend.SyntaxEditor
{
    public partial class SyntaxEditorControl : UserControl
    {

        private int debugged_tabIndex = 0;
        private int previous_tabIndex = 0;

        private gdb_runner _GDBinstance;

        public SyntaxEditorControl()
        {
            InitializeComponent();
        }

        public void InitControl(gdb_runner GDBinstance)
        {
            _GDBinstance = GDBinstance;
            _GDBinstance.onConnected += new EventHandler<GDBnewLocationEventArgs>(_GDBinstance_showLocation);
            _GDBinstance.onStopped   += new EventHandler<GDBnewLocationEventArgs>(_GDBinstance_showLocation);
            _GDBinstance.onRunningStart+=new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onRunningStart);
     
        }

        private void _GDBinstance_showLocation(object sender, GDBnewLocationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBnewLocationEventArgs>(_GDBinstance_showLocation), new object[] { sender, e });
            }
            else
            {
                showTab(e.full_filename, e.filename, e.linenumber);
                toolStripStatusLabel1.Text = "stopped @0x"+e.address;
            }
        }

   

        private void _GDBinstance_onRunningStart(object sender, GDBcmdReplyEventArgs e)
        {
          
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onRunningStart), new object[] { sender, e });
            }
            else
            {
                toolStripStatusLabel1.Text = "running...";
            }

        }


  

        private void showTab(string filename, string title, int linenumber)
        {
            bool tab_open = false;

            if (filename == "" || title == "" || linenumber == -1)
            {
                MessageBox.Show("Can´t find Source Location! Try to (Re)Flash the program!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (TabPage tp in DebugTabControl.TabPages) //Überprüfe ob Datei geöffnet
            {
                if (tp.Name == title)
                {
                    DebugTabControl.SelectedTab = tp; //tab umschalten
                    previous_tabIndex = debugged_tabIndex;
                    debugged_tabIndex = DebugTabControl.TabPages.IndexOf(tp);

                    tab_open = true;
                    break;
                }
            }

            if (tab_open == false) //Datei noch in keinem Tab geöffnet -> Datei öffnen/Neuen Tab hinzufügen
            {
                DebuggerTabPage tp = new DebuggerTabPage(filename);
                tp.Name = title;
                tp.Text = title;
                DebugTabControl.TabPages.Add(tp);
                DebugTabControl.SelectedTab = tp; //tab umschalten

                previous_tabIndex = debugged_tabIndex;
                debugged_tabIndex = DebugTabControl.TabPages.IndexOf(tp);
            }

            if (previous_tabIndex != debugged_tabIndex) //Highlighting im vorherigen Tab ausschalten
            {
                ((DebuggerTabPage)DebugTabControl.TabPages[previous_tabIndex]).syntaxBox.HighLightCurrentDebugLine = false;
            }

            // Aktuelle Seite debuggen...
            DebuggerTabPage debugged_tab = (DebuggerTabPage)DebugTabControl.TabPages[debugged_tabIndex];

            debugged_tab.syntaxBox.HighLightCurrentDebugLine = true;

            if (linenumber == 0) linenumber = 1;
            debugged_tab.syntaxBox.CurrentDebugRow = linenumber - 1;
            debugged_tab.syntaxBox.GotoLine(linenumber - 1);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DebuggerTabPage active_tab = (DebuggerTabPage)DebugTabControl.TabPages[DebugTabControl.SelectedIndex];
            int cursorLineNum = active_tab.syntaxBox.Caret.Position.Y + 1;
                //debugged_tab.syntaxBox.Caret.CurrentRow.Breakpoint; 
            _GDBinstance.send_cmd("-exec-until " + active_tab.Text + ":" + cursorLineNum.ToString());


            
        }

        private void evaluateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebuggerTabPage active_tab = (DebuggerTabPage)DebugTabControl.TabPages[DebugTabControl.SelectedIndex];
            string selection = active_tab.syntaxBox.Selection.Text;
   
            if (selection != "")
            {
                _GDBinstance.send_cmd("-var-create - * " + selection);
            }
        }


    }


    public class DebuggerTabPage : TabPage
    {

        public SyntaxBoxControl syntaxBox;

        public DebuggerTabPage(string filename)
        {
            syntaxBox = new SyntaxBoxControl();

            this.Controls.Add(syntaxBox);
            syntaxBox.Caret.Blink = false;
            syntaxBox.Dock = System.Windows.Forms.DockStyle.Fill;
            syntaxBox.Document.SyntaxFile = "C++.syn";
            syntaxBox.Open(filename);
            syntaxBox.CurrenDebugLineColor = Color.Cyan;
            syntaxBox.HighLightCurrentDebugLine = false;
            syntaxBox.ReadOnly = true;
          
        }
    }
}
