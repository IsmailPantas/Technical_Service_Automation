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
    public partial class Ana_Menu_Personel : Form
    {
        public Ana_Menu_Personel()
        {
            InitializeComponent();
        }
        public string KullaniciAdi;
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Çıkış Yapmak İstediğinize Emin Misiniz", "Bilgilendirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                Giris_Secme frm = new Giris_Secme();
                frm.Show();
                this.Hide();
            }
            else if (sonuc == DialogResult.No) { }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Musteri_Islemleri frm = new Musteri_Islemleri();
            frm.ShowDialog();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Stok_Islemleri frm = new Stok_Islemleri();
            frm.ShowDialog();
        }
        private void Ana_Menu_Personel_Load(object sender, EventArgs e)
        {
            label7.Text = KullaniciAdi;
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Tamir_Islemleri frm = new Tamir_Islemleri();
            frm.ShowDialog();
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Istatistikler frm = new Istatistikler();
            frm.ShowDialog();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Gecmis_Islemler frm = new Gecmis_Islemler();
            frm.ShowDialog();
        }
    }
}