using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace sentiment_analysis_v3
{
    public partial class Form1 : Form
    {
        private PrivateFontCollection pfc = new PrivateFontCollection(); //custom font
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Select your font from the resources.
            int fontLength = Properties.Resources.rudimental_bold.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.rudimental_bold;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            pfc.AddMemoryFont(data, fontLength);

            // using the font
            label1.Font = new Font(pfc.Families[0], 28);
            this.Icon = Properties.Resources.icons8_emojis_64; //sets icon to custom icon
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 statementinput = new Form2();
            this.Hide();
            statementinput.StartPosition = FormStartPosition.Manual;
            statementinput.Location = this.Location;
            statementinput.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 test = new Form4();
            test.StartPosition = FormStartPosition.Manual;
            test.Location = this.Location;
            this.Hide();
            test.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pfc.Dispose(); //this does something to stop the form from crashing. it works
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 documentinput = new Form5();
            documentinput.StartPosition = FormStartPosition.Manual;
            documentinput.Location = this.Location;
            this.Hide();
            documentinput.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
