using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GDB_Frontend.OpenOcd;

namespace GDBfrontend.controls
{
    public partial class OpenOCDstatusControl : UserControl
    {
        OpenOCD_runner _OpenOCDinstance;
        const int MAX_LIST_ELEMENTS = 10;

        public OpenOCDstatusControl()
        {
            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            toolStripMenuItem1.Image = imageList1.Images[2];
        }

        public void InitControl(OpenOCD_runner OpenOCDinstance)
        {
            _OpenOCDinstance = OpenOCDinstance;
            _OpenOCDinstance.onExit  += new EventHandler(_OpenOCDinstance_onExit);
            _OpenOCDinstance.onInfo += new EventHandler<OpenOCDOutputEventArgs>(_OpenOCDinstance_onInfo);
            _OpenOCDinstance.onError += new EventHandler<OpenOCDOutputEventArgs>(_OpenOCDinstance_onError);

        }

        public void clearListbox()
        {
            listBox1.Items.Clear();
        }

        void _OpenOCDinstance_onExit(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(_OpenOCDinstance_onExit), new object[] { sender, e });
            }
            else
            {
                listBox1.Items.Add(new myListboxItem("OpenOCD terminated!", Brushes.Black,1));
                listBox1.SelectedIndex = listBox1.Items.Count - 1; 
            }
        }

        void _OpenOCDinstance_onInfo(object sender, OpenOCDOutputEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<OpenOCDOutputEventArgs>(_OpenOCDinstance_onInfo), new object[] { sender, e });
            }
            else
            {
                listBox1.Items.Add(new myListboxItem(CleanLine(e.InfoMessage), Brushes.Blue,0));
                listBox1.SelectedIndex = listBox1.Items.Count - 1; 
            }
        }

        void _OpenOCDinstance_onError(object sender, OpenOCDOutputEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<OpenOCDOutputEventArgs>(_OpenOCDinstance_onError), new object[] { sender, e });
            }
            else
            {

                listBox1.Items.Add( new myListboxItem( CleanLine(e.InfoMessage), Brushes.Red,1));
                listBox1.SelectedIndex = listBox1.Items.Count - 1; 
            }
        }

        string CleanLine(string input)
        {
            return input.Replace('\r', ' ').Replace('\n', ' ').Trim();
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                Clipboard.SetText(((myListboxItem)listBox1.Items[listBox1.SelectedIndex]).ItemText);
               
        }



    }

  class myListboxItem
    {
        public Brush ItemBrush { get { return _brush; } }
        public string ItemText { get { return _text;  } }
        public int ImageIndex { get { return _index; } }


        string _text;
        Brush  _brush;
        int    _index;
        public myListboxItem(string _text, Brush _brush, int imageIndex )
        {
            this._text = _text;
            this._brush = _brush;
            this._index = imageIndex;
        }

    }
}
