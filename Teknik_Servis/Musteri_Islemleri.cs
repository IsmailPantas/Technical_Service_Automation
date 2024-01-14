using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Teknik_Servis
{
    public partial class Musteri_Islemleri : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        DataSet1TableAdapters.TblMusteriTableAdapter tb = new DataSet1TableAdapters.TblMusteriTableAdapter();
        public Musteri_Islemleri()
        {
            InitializeComponent();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = tb.MusteriListele();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                TxtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                TxtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                TxtSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                CmbMarka.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                CmbSorun.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (TxtAd.Text == "" || TxtSoyad.Text == "")
            {
                MessageBox.Show("Eksik Bilgi Girdiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                tb.MusteriEkle(TxtAd.Text, TxtSoyad.Text, int.Parse(CmbMarka.SelectedValue.ToString()), int.Parse(CmbSorun.SelectedValue.ToString()));
                MessageBox.Show("Ekleme İşlemi Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Musteri_Islemleri_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM TblMarka", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            SqlCommand komut1 = new SqlCommand("SELECT * FROM TblParca", baglanti);
            SqlDataAdapter da1 = new SqlDataAdapter(komut1);
            DataTable dt2 = new DataTable();
            da1.Fill(dt2);
            da.Fill(dt);
            CmbMarka.DisplayMember = "MarkaAd";
            CmbMarka.ValueMember = "id";
            CmbMarka.DataSource = dt;
            CmbSorun.DisplayMember = "Parca";
            CmbSorun.ValueMember = "ParcaID";
            CmbSorun.DataSource = dt2;
            baglanti.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tb.MusteriSil(int.Parse(TxtID.Text));
            MessageBox.Show("Müşteri Silme İşlemi Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            tb.MusteriGuncelle(TxtAd.Text, TxtSoyad.Text, int.Parse(CmbMarka.SelectedValue.ToString()), int.Parse(CmbSorun.SelectedValue.ToString()), int.Parse(TxtID.Text));
            MessageBox.Show("Müşteri Güncelleme İşlemi Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            if (RdbAd.Checked == true)
            {
                dataGridView1.DataSource = tb.AdaGoreAra(TxtArama.Text);
            }
            if (RdbSoyad.Checked == true)
            {
                dataGridView1.DataSource = tb.SoyadaGoreAra(TxtArama.Text);
            }
            if (RdbMarka.Checked == true)
            {
                dataGridView1.DataSource = tb.MarkayaGoreAra(TxtArama.Text);
            }
        }
    }
}