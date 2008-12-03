using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;

using GDBfrontend.openocd;

namespace GDBfrontend.controls
{
    public partial class OpenOCDconfigControl : UserControl
    {

        public openOCDConfig cfg
        {
            get { return _cfg; }
        }
        private openOCDConfig _cfg = new openOCDConfig();
        
        public OpenOCDconfigControl()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";

        }
 
        private void OpenOCDconfig_Load(object sender, EventArgs e)
        {
            try
            {
                load_cfg();
            }
            catch
            {
            }
        }

        private void selectexe_btn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "cfg files (*.exe)|*.exe|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openOCDexe_tb.Text = openFileDialog1.FileName;

                save_cfg();

            }

        }

        private void openocdCfgfile_btn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "cfg files (*.cfg)|*.cfg|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenOCDcfgfile_tb.Text = openFileDialog1.FileName;

                save_cfg();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "cfg files (*.cfg)|*.cfg|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openocdcfgFlash_tb.Text = openFileDialog1.FileName;

                save_cfg();

            }
            
        }

      

       private void updateTextBoxes()
        {
            OpenOCDcfgfile_tb.Text = _cfg.OCDcfgPath;
            openocdcfgFlash_tb.Text = _cfg.OCDcfgFlashPath;
            openOCDexe_tb.Text = _cfg.OCDexePath;
        }

       private void saveFromTextBoxes()
       {
          _cfg.OCDcfgPath = OpenOCDcfgfile_tb.Text;
          _cfg.OCDcfgFlashPath =  openocdcfgFlash_tb.Text;
          _cfg.OCDexePath = openOCDexe_tb.Text;
       }
      

        private void save_cfg()
        {
            
            saveFromTextBoxes();
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(openOCDConfig));
                FileStream str = new FileStream("config_ocd.dat", FileMode.Create);
                ser.Serialize(str, _cfg);
                str.Close();
                _cfg.loaded = true;
            }
            catch
            {
                MessageBox.Show("OpenOCDConfig NOT saved :/", "Error savin Config File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void load_cfg()
        {

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(openOCDConfig));
                StreamReader str = new StreamReader("config_ocd.dat");
                _cfg = (openOCDConfig)ser.Deserialize(str);
                str.Close();
                updateTextBoxes();
                _cfg.loaded = true;
            }
            catch
            {
                _cfg.loaded = false;
            }
        }

     
    }

   
}
