using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace GDBfrontend.gdb
{
    public partial class GDBstatusControl : UserControl
    {
        public GDBstatusControl()
        {
            InitializeComponent();
        }

        gdb_runner _GDBinstance;

        private StringBuilder rawReplyData = new StringBuilder(10000);

        public void InitControl(gdb_runner GDBinstance)
        {
            _GDBinstance = GDBinstance;
            _GDBinstance.onStartupDone += new EventHandler(_GDBinstance_onStartupDone);
            _GDBinstance.onConnected += new EventHandler<GDBnewLocationEventArgs>(_GDBinstance_onConnected);
            _GDBinstance.onCMDdone += new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onCMDdone);
            _GDBinstance.onCMDerror += new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onCMDerror);
      
            _GDBinstance.onRawReply += new EventHandler<GDBrawReplyEventArgs>(_GDBinstance_onRawReply);
        }


        void _GDBinstance_onRawReply(object sender, GDBrawReplyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBrawReplyEventArgs>(_GDBinstance_onRawReply), new object[] { sender, e });
            }
            else
            {

                if (rawReplyData.Length > 1000) rawReplyData.Remove(0, rawReplyData.Length);
                rawReplyData.Append(e.output);
                
                textBox1.AppendText(e.output + "\r\n");
            }
        }


        void _GDBinstance_onCMDerror(object sender, GDBcmdReplyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onCMDerror), new object[] { sender, e });
            }
            else
            {

                listBox1.Items.Add(new myListboxItem("cmd error  :" + e.replyTextData, Brushes.Red, 1));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }


        void _GDBinstance_onCMDdone(object sender, GDBcmdReplyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBcmdReplyEventArgs>(_GDBinstance_onCMDdone), new object[] { sender, e });
            }
            else
            {

                listBox1.Items.Add(new myListboxItem("cmd done  :" + e.sendCmd, Brushes.Black, 0));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        void _GDBinstance_onConnected(object sender, GDBnewLocationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<GDBnewLocationEventArgs>(_GDBinstance_onConnected), new object[] { sender, e });
            }
            else
            {
                
                listBox1.Items.Add(new myListboxItem("GDB connected! Pos" + e.filename +" [0x"+e.address+"] Funk"+e.function+" Line:"+e.linenumber, Brushes.Black, 0));
                listBox1.SelectedIndex = listBox1.Items.Count - 1; 
            }
        }

        void _GDBinstance_onStartupDone(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(_GDBinstance_onStartupDone), new object[] { sender, e });
            }
            else
            {
                listBox1.Items.Add(new myListboxItem("GDB succesfully startet!", Brushes.Black, 0));
                listBox1.SelectedIndex = listBox1.Items.Count - 1; 
            }
        }



       

        class myListboxItem
        {
            public Brush ItemBrush { get { return _brush; } }
            public string ItemText { get { return _text; } }
            public int ImageIndex { get { return _index; } }


            string _text;
            Brush _brush;
            int _index;
            public myListboxItem(string _text, Brush _brush, int imageIndex)
            {
                this._text = _text;
                this._brush = _brush;
                this._index = imageIndex;
            }

        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor, Color.Azure);


            e.DrawBackground();

            //Brush circBrush = Brushes.Blue;
            // e.Bounds ist das Rechteck von dem angewählten item
            Rectangle recText = e.Bounds;
            Rectangle recCirc = e.Bounds;

            recCirc.Width = recCirc.Height;

            recText.X += recCirc.Width;



            if (e.Index != -1)
            {
                e.Graphics.DrawString(((myListboxItem)listBox1.Items[e.Index]).ItemText, e.Font, Brushes.Black, recText, StringFormat.GenericDefault);
                e.Graphics.DrawImage(imageList1.Images[((myListboxItem)listBox1.Items[e.Index]).ImageIndex], new Point(recCirc.X, recCirc.Y));
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(rawReplyData.ToString(), "Raw GDB MI Output", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        }
}
