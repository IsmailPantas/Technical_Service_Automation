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

namespace Teknik_Servis
{
    public partial class Stok_Islemleri : Form
    {
        public Stok_Islemleri()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        private void Stok_Islemleri_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT * FROM TblParca";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                CmbParca.Items.Add(dr["Parca"]);
            }
            baglanti.Close();
            //////////////////////////////////////////////
            SqlCommand komut1 = new SqlCommand();
            komut1.CommandText = "SELECT Bakiye FROM TblBakiye";
            komut1.Connection = baglanti;
            komut1.CommandType = CommandType.Text;
            SqlDataReader dr1;
            baglanti.Open();
            dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                label10.Text = dr1["Bakiye"].ToString();
            }
            baglanti.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (CmbParca.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Bir Parça Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True"))
                {
                    string queryString = "SELECT Stok FROM TblParca WHERE Parca = @parcaAdi";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@parcaAdi", CmbParca.SelectedItem.ToString());
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int stok = reader.GetInt32(0);
                            label4.Visible = true;
                            label4.Text = stok.ToString();
                        }
                    }
                    else
                    {
                        label4.Text = "Veri bulunamadı.";
                    }
                    reader.Close();
                    /////////////////////////////////////////
                    string queryString1 = "SELECT AlısFiyat FROM TblParca WHERE Parca = @p2";
                    SqlCommand command1 = new SqlCommand(queryString1, connection);
                    command1.Parameters.AddWithValue("@p2", CmbParca.SelectedItem.ToString());
                    SqlDataReader reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        decimal alis = reader1.GetDecimal(0);
                        kasa.Visible = true;
                        kasa.Text = alis.ToString();
                    }
                    reader1.Close();
                    ///////////////////////////////////////////
                    string queryString2 = "SELECT SatisFiyat FROM TblParca WHERE Parca = @p3";
                    SqlCommand command2 = new SqlCommand(queryString2, connection);
                    command2.Parameters.AddWithValue("@p3", CmbParca.SelectedItem.ToString());
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            decimal satis = reader2.GetDecimal(0);
                            label7.Visible = true;
                            label7.Text = satis.ToString();
                        }
                    }
                    else
                    {
                        label7.Text = "Veri bulunamadı.";
                    }
                    reader2.Close();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (CmbParca.SelectedItem == null || NmrAdet == null)
            {
                MessageBox.Show("Eksik Parça veya Adet Bilgisi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (NmrAdet.Value > 50)
            {
                MessageBox.Show("Tek Seferde En Fazla 50 Ürün Eklenebilir", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlCommand komut = new SqlCommand("SELECT AlısFiyat FROM TblParca WHERE Parca = @p1", baglanti);
                komut.Parameters.AddWithValue("@p1", CmbParca.SelectedItem.ToString());
                double adet, fiyat, toplam;
                adet = Convert.ToDouble(NmrAdet.Text);
                object deger = komut.ExecuteScalar();
                fiyat = Convert.ToDouble(deger);
                toplam = adet * fiyat;
                TxtHesap.Text = toplam.ToString();
            }
            baglanti.Close();
        }
        private void BtnStok_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TxtHesap.Text))
            {
                MessageBox.Show("Lütfen Değer Hesaplaması Yapınız", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                double eklenenStok = Convert.ToDouble(NmrAdet.Value);
                double mevcutBakiye;
                if (double.TryParse(label10.Text, out mevcutBakiye))
                {
                    double cikarilacakBakiye = Convert.ToDouble(TxtHesap.Text);
                    double yeniBakiye = mevcutBakiye - cikarilacakBakiye;

                    if (yeniBakiye < 0)
                    {
                        MessageBox.Show("Bakiye Yetersiz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand("UPDATE TblParca SET Stok = Stok + @Stok WHERE Parca = @Parca", baglanti);
                        komut.Parameters.AddWithValue("@Parca", CmbParca.SelectedItem.ToString());
                        komut.Parameters.AddWithValue("@Stok", eklenenStok);
                        komut.ExecuteNonQuery();
                        SqlCommand komut1 = new SqlCommand("UPDATE TblBakiye SET Bakiye = Bakiye - @Bakiye", baglanti);
                        komut1.Parameters.AddWithValue("@Bakiye", cikarilacakBakiye);
                        komut1.ExecuteNonQuery();
                        SqlCommand komut2 = new SqlCommand("SELECT Bakiye FROM TblBakiye", baglanti);
                        SqlDataReader dr1 = komut2.ExecuteReader();
                        while (dr1.Read())
                        {
                            label10.Text = dr1["Bakiye"].ToString();
                        }
                        MessageBox.Show("Ürün Ekleme İşlemi Başarılı");
                        baglanti.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Geçersiz Bakiye", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void BtnÜrün_Click(object sender, EventArgs e)
        {
            if (TxtYeniAd.Text == "" || TxtYeniAlis.Text == "" || TxtYeniSatis.Text == "")
            {
                MessageBox.Show("Lütfen Bilgileri Eksiksiz Giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Eklemek İstediğinize Emin Misiniz?", "Bilgilendirme", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("INSERT INTO TblParca (Parca, Stok, AlısFiyat, SatisFiyat) VALUES (@p1,0, @p2, @p3)", baglanti);
                    komut.Parameters.AddWithValue("@p1", TxtYeniAd.Text);
                    komut.Parameters.AddWithValue("@p2", double.Parse(TxtYeniAlis.Text));
                    komut.Parameters.AddWithValue("@p3", double.Parse(TxtYeniSatis.Text));
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Ürün Ekleme İşlemi Başarılı", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.No)
                {

                }
            }
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

