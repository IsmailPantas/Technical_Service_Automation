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
    public partial class Istatistikler : Form
    {
        public Istatistikler()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand ToplamStok = new SqlCommand("SELECT SUM(Stok) AS ToplamMiktar FROM TblParca", baglanti);
            object toplam = ToplamStok.ExecuteScalar();
            label4.Text = toplam.ToString();
            SqlCommand ToplamUrun = new SqlCommand("SELECT COUNT(Parca) AS 'Toplam Parça' FROM TblParca", baglanti);
            object toplamUrun = ToplamUrun.ExecuteScalar();
            label6.Text = toplamUrun.ToString();
            SqlCommand EnAzUrun = new SqlCommand("SELECT TOP 1 Parca FROM TblParca ORDER BY Stok ASC", baglanti);
            object enAzUrun = EnAzUrun.ExecuteScalar();
            label8.Text = enAzUrun.ToString();
            SqlCommand EnAzStok = new SqlCommand("SELECT TOP 1 Stok FROM TblParca ORDER BY Stok ASC", baglanti);
            object enAzStok = EnAzStok.ExecuteScalar();
            label10.Text = enAzStok.ToString();
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel5.Visible = true;
            panel6.Visible = true;
            pictureBox1.Image = Properties.Resources.product_white;
            pictureBox2.Image = Properties.Resources.stock;
            pictureBox3.Image = Properties.Resources.balance;
            pictureBox4.Image = Properties.Resources.low_stock;
            pictureBox5.Image = Properties.Resources.profit;
            label1.Text = "Toplam Stok:";
            label2.Text = "Toplam Ürün:";
            label7.Text = "Stok Önerisi:";
            label9.Text = "Kalan Stok:";
            comboBox1.Visible = true;
            comboBox2.Visible = true;
            label12.Text = "Seçiniz";
            label3.Text = "Seçiniz";
            baglanti.Close();
        }
        private void Istatistikler_Load(object sender, EventArgs e)
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
                comboBox1.Items.Add(dr["Parca"]);
            }
            baglanti.Close();
            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "SELECT * FROM TblParca";
            komut2.Connection = baglanti;
            komut2.CommandType = CommandType.Text;
            SqlDataReader dr2;
            baglanti.Open();
            dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox2.Items.Add(dr2["Parca"]);
            }
            baglanti.Close();
            baglanti.Open();
            SqlCommand musteriSayisi = new SqlCommand("SELECT COUNT(*) FROM TblMusteris", baglanti);
            SqlCommand eskiMusteriSayisi = new SqlCommand("SELECT COUNT(*) FROM TblEskiMusteri", baglanti);


          
            int toplamMusteriSayisi = Convert.ToInt32(musteriSayisi.ExecuteScalar());
            int EskiMusteriSayisi = Convert.ToInt32(eskiMusteriSayisi.ExecuteScalar());

            if (EskiMusteriSayisi != 0)
            {
                double oran = (double)EskiMusteriSayisi / (toplamMusteriSayisi + EskiMusteriSayisi);
                int yuzdeOran = (int)(oran * 100); // Yüzde olarak ifade etmek için 100 ile çarpıyoruz.

                //ProgressBar değerini yeniden ölçeklendirme
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                progressBar1.Value = Math.Min(Math.Max(yuzdeOran, progressBar1.Minimum), progressBar1.Maximum);
                label15.Text = yuzdeOran.ToString();
            }
            else
            {
                progressBar1.Value = 0;
            }
            baglanti.Close();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT Stok FROM TblParca WHERE Parca = @SelectedParcaID", baglanti);
            cmd.Parameters.AddWithValue("@SelectedParcaID", comboBox1.Text);
            SqlCommand cmd2 = new SqlCommand("SELECT SatisFiyat FROM TblParca WHERE Parca = @SecilenParca", baglanti);
            cmd2.Parameters.AddWithValue("@SecilenParca", comboBox1.Text);
            baglanti.Open();
            object result = cmd.ExecuteScalar();
            object result2 = cmd2.ExecuteScalar();
            if (result != null && result2 != null)
            {
                int stok = Convert.ToInt32(result);
                int satisFiyati = Convert.ToInt32(result2);
                int carpim = stok * satisFiyati;
                label3.Text = carpim.ToString();
            }
            baglanti.Close();
        }
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT AlısFiyat FROM TblParca WHERE Parca = @SelectedParcaID", baglanti);
            cmd.Parameters.AddWithValue("@SelectedParcaID", comboBox2.Text);
            SqlCommand cmd2 = new SqlCommand("SELECT SatisFiyat FROM TblParca WHERE Parca = @SecilenParca", baglanti);
            cmd2.Parameters.AddWithValue("@SecilenParca", comboBox2.Text);
            baglanti.Open();
            object result = cmd.ExecuteScalar();
            object result2 = cmd2.ExecuteScalar();
            if (result != null && result2 != null)
            {
                int alisFiyati = Convert.ToInt32(result);
                int satisFiyati = Convert.ToInt32(result2);
                int carpim = satisFiyati - alisFiyati;
                label12.Text = carpim.ToString();
            }
            baglanti.Close();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand ToplamSatis = new SqlCommand("SELECT SUM (EskiFiyat) AS ToplamSatis FROM TblEskiMusteri", baglanti);
            object toplamSatis = ToplamSatis.ExecuteScalar();
            label4.Text = toplamSatis.ToString();
            label1.Text = "Toplam Satış:";
            SqlCommand EnCokSatan = new SqlCommand("SELECT TOP 1 EskiSorun, COUNT(*) AS TekrarSayisi FROM TblEskiMusteri GROUP BY EskiSorun ORDER BY COUNT(*) DESC", baglanti);
            object enCokSatan = EnCokSatan.ExecuteScalar();
            label8.Text = enCokSatan.ToString();
            label7.Text = "Fazla Satılan:";
            SqlCommand EnCokAdet = new SqlCommand("SELECT TOP 1 EskiSorun, COUNT(*) AS TekrarSayisi FROM TblEskiMusteri GROUP BY EskiSorun ORDER BY COUNT(*) DESC", baglanti);
            SqlDataReader enCokAdet = EnCokAdet.ExecuteReader();
            enCokAdet.Read();
            string eskiSorun = enCokAdet["EskiSorun"].ToString();
            int tekrarSayisi = Convert.ToInt32(enCokAdet["TekrarSayisi"]);
            label10.Text = tekrarSayisi.ToString();
            label9.Text = "Satılan Adet:";
            enCokAdet.Close();
            SqlCommand ToplamSatisAdeti = new SqlCommand("SELECT COUNT(EskiID) AS StokCount FROM TblEskiMusteri;", baglanti);
            object toplamSatisAdeti = ToplamSatisAdeti.ExecuteScalar();
            label6.Text = toplamSatisAdeti.ToString();
            label2.Text = "Satış Adeti:";
            pictureBox1.Image = Properties.Resources.balance;
            pictureBox4.Image = Properties.Resources.best_product;
            pictureBox2.Image = Properties.Resources.total_sell;
            baglanti.Close();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand ToplamMarka = new SqlCommand("SELECT COUNT (MarkaAd) FROM TblMarka", baglanti);
            object toplamMarka = ToplamMarka.ExecuteScalar();
            label6.Text = toplamMarka.ToString();
            label2.Text = "Toplam Marka:";
            pictureBox2.Image = Properties.Resources.smartphone;
            SqlCommand EnPopulerMarka = new SqlCommand("SELECT TOP 1 EskiMarka, COUNT (*) AS TekrarSayisi FROM TblEskiMusteri GROUP BY EskiMarka ORDER BY COUNT(*) DESC", baglanti);
            object enPopulerMarka = EnPopulerMarka.ExecuteScalar();
            label8.Text = enPopulerMarka.ToString();
            label7.Text = "En Popüler:";
            pictureBox4.Image = Properties.Resources.best_product;
            SqlCommand EnCokAdet = new SqlCommand("SELECT TOP 1 EskiMarka, COUNT (*) AS TekrarSayisi FROM TblEskiMusteri GROUP BY EskiMarka ORDER BY COUNT(*) DESC", baglanti);
            SqlDataReader enCokAdet = EnCokAdet.ExecuteReader();
            enCokAdet.Read();
            string eskiSorun = enCokAdet["EskiMarka"].ToString();
            int tekrarSayisi = Convert.ToInt32(enCokAdet["TekrarSayisi"]);
            label10.Text = tekrarSayisi.ToString();
            label9.Text = "Tamir Adeti:";
            baglanti.Close();
        }
    }
}
