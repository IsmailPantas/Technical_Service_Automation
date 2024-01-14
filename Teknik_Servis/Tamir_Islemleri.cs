using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Teknik_Servis
{
    public partial class Tamir_Islemleri : Form
    {
        public Tamir_Islemleri()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        private void Tamir_Islemleri_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT MusteriID,Marka,Sorun ,MusteriAd + ' ' + MusteriSoyad AS MusteriADSOYAD FROM TblMusteris INNER JOIN TblMarka ON TblMusteris.Marka = TblMarka.id";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                CmbMusteri.Items.Add(dr["MusteriADSOYAD"]);
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
        private void BtnBilgi_Click(object sender, EventArgs e)
        {
            if (CmbMusteri.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Kişi Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT MarkaAd FROM TblMusteris INNER JOIN TblMarka ON TblMusteris.Marka = TblMarka.id WHERE MusteriAd + ' ' + MusteriSoyad = @p1", baglanti);
                komut.Parameters.AddWithValue("@p1", CmbMusteri.SelectedItem.ToString());
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    string marka = Convert.ToString(reader.GetValue(0));
                    TxtID.Visible = true;
                    TxtMarka.Text = marka;
                }
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut1 = new SqlCommand("SELECT Parca FROM TblMusteris INNER JOIN TblParca ON TblMusteris.Sorun = TblParca.ParcaID WHERE MusteriAd + ' ' + MusteriSoyad = @p2", baglanti);
                komut1.Parameters.AddWithValue("@p2", CmbMusteri.SelectedItem.ToString());
                SqlDataReader reader1 = komut1.ExecuteReader();
                while (reader1.Read())
                {
                    string marka = Convert.ToString(reader1.GetValue(0));

                    TxtSorun.Text = marka;
                }
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("SELECT MusteriID FROM TblMusteris INNER JOIN TblParca ON TblMusteris.Sorun = TblParca.ParcaID WHERE MusteriAd + ' ' + MusteriSoyad = @p3", baglanti);
                komut2.Parameters.AddWithValue("@p3", CmbMusteri.SelectedItem.ToString());
                SqlDataReader reader2 = komut2.ExecuteReader();
                while (reader2.Read())
                {
                    int id = Convert.ToInt32(reader2.GetValue(0));
                    TxtID.Text = id.ToString();
                }
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut3 = new SqlCommand("SELECT SatisFiyat FROM TblParca WHERE Parca = @parca", baglanti);
                komut3.Parameters.AddWithValue("@parca", TxtSorun.Text);
                SqlDataReader reader3 = komut3.ExecuteReader();
                while (reader3.Read())
                {
                    int id = Convert.ToInt32(reader3.GetValue(0));
                    TxtFiyat.Text = id.ToString();
                }
                baglanti.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (CmbMusteri.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Bir Müşteri Seçiniz");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(TxtID.Text) || string.IsNullOrWhiteSpace(TxtMarka.Text) || string.IsNullOrWhiteSpace(TxtSorun.Text))
                {
                    MessageBox.Show("Lütfen Önce Bilgileri Görüntüleyiniz");
                }
                else
                {
                    double mevcutBakiye;
                    if (double.TryParse(label10.Text, out mevcutBakiye))
                    {
                        double eklenecekBakiye = Convert.ToDouble(TxtFiyat.Text);
                        double yeniBakiye = mevcutBakiye + eklenecekBakiye;

                        SqlCommand stoksayisi = new SqlCommand("SELECT SUM(Stok) FROM TblParca WHERE Parca = @stok", baglanti);
                        stoksayisi.Parameters.AddWithValue("@stok", TxtSorun.Text);

                        object sonuc = stoksayisi.ExecuteScalar();
                        int stokSayisi = Convert.ToInt32(sonuc);

                        if (stokSayisi <= 0)
                        {
                            MessageBox.Show("Yetersiz Stok", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            SqlCommand komut2 = new SqlCommand("INSERT INTO TblEskiMusteri (EskiID, EskiAdSoyad, EskiMarka, EskiSorun,EskiTarih, EskiFiyat) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
                            komut2.Parameters.AddWithValue("@p1", TxtID.Text);
                            komut2.Parameters.AddWithValue("@p2", CmbMusteri.SelectedItem);
                            komut2.Parameters.AddWithValue("@p3", TxtMarka.Text);
                            komut2.Parameters.AddWithValue("@p4", TxtSorun.Text);
                            DateTime selectedDate = monthCalendar1.SelectionStart;
                            string formattedDate = selectedDate.ToString("yyyy-MM-dd");
                            komut2.Parameters.AddWithValue("@p5", formattedDate);
                            komut2.Parameters.AddWithValue("@p6", TxtFiyat.Text);
                            komut2.ExecuteNonQuery();
                            SqlCommand komut = new SqlCommand("DELETE FROM TblMusteris WHERE MusteriID = @MusteriID", baglanti);
                            komut.Parameters.AddWithValue("@MusteriID", int.Parse(TxtID.Text));
                            komut.ExecuteNonQuery();
                            SqlCommand stokazalt = new SqlCommand("UPDATE TblParca SET Stok = Stok - 1 WHERE Parca = @Parca", baglanti);
                            stokazalt.Parameters.AddWithValue("@Parca", TxtSorun.Text);
                            stokazalt.ExecuteNonQuery();
                            SqlCommand bakiyearttır = new SqlCommand("UPDATE TblBakiye SET Bakiye = Bakiye + @Bakiye", baglanti);
                            bakiyearttır.Parameters.AddWithValue("@Bakiye", eklenecekBakiye);
                            bakiyearttır.ExecuteNonQuery();
                            SqlCommand bakiyegoster = new SqlCommand("SELECT Bakiye FROM TblBakiye", baglanti);
                            SqlDataReader dr1 = bakiyegoster.ExecuteReader();
                            while (dr1.Read())
                            {
                                label10.Text = dr1["Bakiye"].ToString();
                                MessageBox.Show("Tamir İşlemi Başarılı");
                            }
                        }
                    }
                }
            }
            baglanti.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            CmbMusteri.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT MusteriID,Marka,Sorun ,MusteriAd + ' ' + MusteriSoyad AS MusteriADSOYAD FROM TblMusteris INNER JOIN TblMarka ON TblMusteris.Marka = TblMarka.id";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr1;
            dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {
                CmbMusteri.Items.Add(dr1["MusteriADSOYAD"]);
            }
            baglanti.Close();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}