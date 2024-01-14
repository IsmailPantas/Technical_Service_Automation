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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Teknik_Servis
{
    public partial class Personel_Giris : Form
    {
        SqlConnection conn;
        SqlDataReader dr;
        SqlCommand comm;
        public Personel_Giris()
        {
            InitializeComponent();
        }
        public bool islem = false;
        private void button1_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(@"Data Source="".\SQLEXPRESS"";Initial Catalog=DbTeknikServis;Integrated Security=True;");
            comm = new SqlCommand();
            conn.Open();
            comm.Connection = conn;
            comm.CommandText = "Select * From TblPersonel where K_Adi= '" + TxtKullanıcı.Text + "' And Sifre='" + TxtSifre.Text + "' ";
            dr = comm.ExecuteReader();
            if (dr.Read())
            {
                Ana_Menu_Personel anaMenuPersonel = new Ana_Menu_Personel();
                anaMenuPersonel.KullaniciAdi = TxtKullanıcı.Text;
                anaMenuPersonel.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LnkSifreUnuttum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Sifre_Yenile_Personel sifre = new Sifre_Yenile_Personel();
            sifre.Show();
            this.Hide();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Giris_Secme frm = new Giris_Secme();
            frm.Show();
            this.Hide();
        }
        private void Personel_Giris_Load(object sender, EventArgs e)
        {

        }
    }
}
