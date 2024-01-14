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
    public partial class Personel_Islemleri : Form
    {
        public Personel_Islemleri()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnListele_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("SELECT * FROM TblPersonel", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (TxtAd.Text == "" || TxtEposta.Text == "" || TxtSifre.Text == "")
            {
                MessageBox.Show("Lütfen Bilgileri Eksiksiz Giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO TblPersonel (K_Adi, Sifre, Eposta) VALUES (@K_Adi,@Sifre,@Eposta)", baglanti);
                komut.Parameters.AddWithValue("@K_Adi", TxtAd.Text);
                komut.Parameters.AddWithValue("@Sifre", TxtSifre.Text);
                komut.Parameters.AddWithValue("@Eposta", TxtEposta.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Personel Ekleme İşlemi Başarılı");
            }
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (TxtID.Text == "")
            {
                MessageBox.Show("Lütfen Silinecek Personeli Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE * FROM TblPersonel WHERE id = @id");
                komut.Parameters.AddWithValue("@id", TxtID);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Personel Silme İşlemi Başarılı");
            }
        }
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (TxtAd.Text == "" || TxtEposta.Text == "" || TxtSifre.Text == "")
            {
                MessageBox.Show("Lütfen Bilgileri Eksiksiz Giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("UPDATE TblPersonel SET K_Adi = @K_Adi , Sifre = @Sifre, Eposta=@Eposta", baglanti);
                komut.Parameters.AddWithValue("@K_Adi", TxtAd.Text);
                komut.Parameters.AddWithValue("@Sifre", TxtSifre.Text);
                komut.Parameters.AddWithValue("@Eposta", TxtEposta.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Personel Güncelleme İşlemi Başarılı", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                TxtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                TxtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                TxtSifre.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                TxtEposta.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM TblPersonel WHERE K_Adi = @K_Adi", baglanti);
            komut.Parameters.AddWithValue("@K_Adi", TxtArama.Text);
            komut.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
    }
}