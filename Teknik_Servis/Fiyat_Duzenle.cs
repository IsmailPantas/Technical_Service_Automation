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
    public partial class Fiyat_Duzenle : Form
    {
        public Fiyat_Duzenle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        private void Fiyat_Duzenle_Load(object sender, EventArgs e)
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
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT AlısFiyat FROM TblParca WHERE Parca = @SelectedParcaAlis", baglanti);
            komut.Parameters.AddWithValue("@SelectedParcaAlis", comboBox1.Text);
            object alisFiyat = komut.ExecuteScalar();
            textBox1.Text = alisFiyat.ToString();
            SqlCommand komut2 = new SqlCommand("SELECT SatisFiyat FROM TblParca WHERE Parca = @SelectedParcaSatis", baglanti);
            komut2.Parameters.AddWithValue("@SelectedParcaSatis", comboBox1.Text);
            object satisFiyat = komut2.ExecuteScalar();
            textBox2.Text = satisFiyat.ToString();
            baglanti.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Lütfen Bir Parça Seçiniz");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Lütfen Alış ve Satış Fiyat Bilgilerini Giriniz");
                }
                else
                {
                    SqlCommand komut = new SqlCommand("UPDATE TblParca SET AlısFiyat = @Alıs WHERE Parca = @Parca", baglanti);
                    komut.Parameters.AddWithValue("@Alıs", decimal.Parse(textBox4.Text));
                    komut.Parameters.AddWithValue("@Parca", comboBox1.Text);
                    komut.ExecuteNonQuery();
                    SqlCommand komut2 = new SqlCommand("UPDATE TblParca SET SatisFiyat = @Satis WHERE Parca = @Parca2", baglanti);
                    komut2.Parameters.AddWithValue("@Satis", decimal.Parse(textBox3.Text));
                    komut2.Parameters.AddWithValue("@Parca2", comboBox1.Text);
                    komut2.ExecuteNonQuery();
                    MessageBox.Show("Fiyat Güncelleme İşlemi Başarılı");
                }
            }
            baglanti.Close();
        }
    }
}
