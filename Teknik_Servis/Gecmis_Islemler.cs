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
    public partial class Gecmis_Islemler : Form
    {
        public Gecmis_Islemler()
        {
            InitializeComponent();
        }
        public string PersonelAdi;
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DbTeknikServis;Integrated Security=True");
        DataSet1TableAdapters.TblEskiMusteriTableAdapter tb1 = new DataSet1TableAdapters.TblEskiMusteriTableAdapter();
        private void Gecmis_Islemler_Load(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = tb1.EskiMusteriListele();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                TxtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                TxtAdSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                TxtMarka.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                TxtParca.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                TxtTarih.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                TxtFiyat.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtID.Text))
            {
                MessageBox.Show("Lütfen Önce Bir Müşteri Seçiniz");
            }
            else
            {
                Fatura fatura = new Fatura();
                fatura.IDNumarası = TxtID.Text;
                fatura.AdSoyad = TxtAdSoyad.Text;
                fatura.Marka = TxtMarka.Text;
                fatura.Parca = TxtParca.Text;
                fatura.Tarih = TxtTarih.Text;
                fatura.Fiyat = TxtFiyat.Text;
                fatura.Show();
            }
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
