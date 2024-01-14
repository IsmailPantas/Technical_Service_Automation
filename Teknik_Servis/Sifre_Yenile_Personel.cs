using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teknik_Servis
{
    public partial class Sifre_Yenile_Personel : Form
    {
        SqlConnection conn;
        SqlDataReader dr;
        SqlCommand comm;
        string YenilemeMaili = "sifremiyenileme33@hotmail.com";
        string YenilemeSifresi = "sifreyenile33_";
        string yeniSifre;
        void kodolustur()
        {
            Random rastgele = new Random();
            int kod = rastgele.Next(10000, 100000);
            textBox1.Text = kod.ToString();
        }
        public Sifre_Yenile_Personel()
        {
            InitializeComponent();
        }
        private void Sifre_Yenile_Personel_Load(object sender, EventArgs e)
        {
            kodolustur();
        }
        private void BtnKodGoster_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.White;
            textBox1.Visible = true;
        }
        private void BtnGonder_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == TxtDogrulama.Text)
            {
                conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
                comm = new SqlCommand();
                conn.Open();
                comm.Connection = conn;
                comm.CommandText = "Select * From TblPersonel where K_Adi= '" + TxtKullanici.Text + "' And Eposta='" + TxtPosta.Text + "' ";
                dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    Random rand = new Random();
                    yeniSifre = rand.Next(10000, 100000).ToString();
                    dr.Close();
                    comm = new SqlCommand("UPDATE TblPersonel SET Sifre = @YeniSifre WHERE K_Adi = @K_Adi", conn);
                    comm.Parameters.AddWithValue("@YeniSifre", yeniSifre);
                    comm.Parameters.AddWithValue("@K_Adi", TxtKullanici.Text);
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Şifre Yenileme İşleminiz Başarılı . E-Postanızı kontrol ediniz.");
                    SmtpClient sc = new SmtpClient();
                    sc.Port = 587;
                    sc.Host = "smtp.outlook.com";
                    sc.EnableSsl = true;
                    sc.Credentials = new NetworkCredential(YenilemeMaili, YenilemeSifresi);
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(YenilemeMaili, "Şifre Sıfırlama");
                    mail.To.Add(TxtPosta.Text);
                    mail.Subject = "Şifre Sıfırlama İsteği";
                    mail.IsBodyHtml = true;
                    mail.Body = $" Sistemimiz tarafından oluşturulan yeni şifreniz : {yeniSifre} . Bidaha unutma olur mu 😊";
                    sc.Send(mail);
                }
                else
                {
                    MessageBox.Show("Girdiğiniz Bilgiler Doğru Değil", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Doğrulama Kodunuzu Kontrol Ediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Personel_Giris frm = new Personel_Giris();
            frm.Show();
            this.Hide();
        }
    }
}
