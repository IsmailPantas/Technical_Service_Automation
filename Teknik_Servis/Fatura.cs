using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teknik_Servis
{
    public partial class Fatura : Form
    {
        public Fatura()
        {
            InitializeComponent();
        }
        public string IDNumarası;
        public string AdSoyad;
        public string Marka;
        public string Parca;
        public string Tarih;
        public string Fiyat;
        public string PersonelAdi;
        private void Fatura_Load(object sender, EventArgs e)
        {
            int index = Tarih.IndexOf(' ');
            string sadeceTarih = Tarih.Substring(0, index);
            label2.Text = sadeceTarih;
            LblID.Text = IDNumarası;
            LblAdsoyad.Text = AdSoyad;
            LblMarka.Text = Marka;
            LblParca.Text = Parca;
            LblFiyat.Text = Fiyat;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderName = "faturalar";
            string fullPath = Path.Combine(folderPath, folderName);
            string fileName = AdSoyad+".png";
            string filePath = Path.Combine(fullPath, fileName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                // Panel içeriğini bmp nesnesine aktar
                Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                // Panel içeriğini bmp nesnesine aktar
                Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            MessageBox.Show("Fatura Kesme İşlemi Başarılı", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
