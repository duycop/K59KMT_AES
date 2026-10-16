using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K59KMT_AES
{
    public partial class frmAES_txt : Form
    {
        libAES.MyAES aes = null;
        public frmAES_txt() //hàm tạo: cùng tên class
        {
            InitializeComponent();  //code gen UI
            try
            {
                aes = new libAES.MyAES(); //tạo đối tượng từ class MyAES
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thiếu Thư viện libAES.dll: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void btnGenerateKey_Click(object sender, EventArgs e)
        {
            txtKey.Text = aes.genKey();

            byte[] key = Convert.FromBase64String(txtKey.Text);
            label4.Text = $"Key length: {key.Length} bytes";
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtOutput.Text = aes.MaHoaAES(txtInput.Text);
        }


        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtDecryptedOutput.Text = aes.GiaiMaAES(txtOutput.Text, txtKey.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giải mã thất bại! Chuỗi mã hóa hoặc khóa không hợp lệ.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
