using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teknik_Servis
{
    public partial class Animasyonlu_Giris : Form
    {
        public Animasyonlu_Giris()
        {
            InitializeComponent();
        }
        bool islem = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (islem == false)
            {
                progressBar1.Value += 10;
            }
            if (progressBar1.Value == 100)
            {
                islem = true;
            }
            if (islem == true)
            {
                Giris_Secme fr = new Giris_Secme();
                fr.Show();
                this.Hide();
                timer1.Enabled = false;
            }
        }
    }
}
