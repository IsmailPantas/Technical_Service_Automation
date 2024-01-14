using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teknik_Servis
{
    public partial class Admin_Giris : Form
    {
        SqlConnection conn;
        SqlDataReader dr;
        SqlCommand comm;
        public Admin_Giris()
        {
            InitializeComponent();
        }
        public bool islem = false;
        private void button1_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
            comm = new SqlCommand();
            conn.Open();
            comm.Connection = conn;
            comm.CommandText = "Select * From TblAdmin where AdminAd= '" + TxtKullanıcı.Text + "' And AdminSifre='" + TxtSifre.Text + "' ";
            dr = comm.ExecuteReader();
            if (dr.Read())
            {
                Ana_Menu_Admin anaMenuAdmin = new Ana_Menu_Admin();
                anaMenuAdmin.KullaniciAdi = TxtKullanıcı.Text;
                anaMenuAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LnkSifreUnuttum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Sifre_Yenile_Admin sifre = new Sifre_Yenile_Admin();
            sifre.Show();
            this.Hide();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Giris_Secme frm = new Giris_Secme();
            frm.Show();
            this.Hide();
        }
    }
}
