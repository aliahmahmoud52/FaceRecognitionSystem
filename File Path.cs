using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_1_1
{
    public partial class File_Path : Form
    {
        int id1;
        public File_Path()
        {
            InitializeComponent();
        }
        public void setId(int id)
        {
            id1 = id;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Extract k = new Extract();
            k.setId(id1);
            k.Show();
            this.Hide();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void File_Path_Load(object sender, EventArgs e)
        {

        }
    }
}
