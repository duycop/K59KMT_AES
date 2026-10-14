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
    public partial class frmAES_Files : Form
    {

        private bool check_DLL()
        {
            try
            {
                var type = Type.GetType("libAES.MyAES, libAES");
                if (type != null)
                {
                    //ok
                    labelThongBao.Text = "libAES sẵn sàng";
                    return true;
                }
                else
                {
                    throw new System.IO.FileNotFoundException("Không tìm thấy libAES.dll");
                }
            }
            catch (Exception ex)
            {
                labelThongBao.Text = "ERROR: " + ex.Message;
                return false;
            }
        }
        public frmAES_Files()
        {
            InitializeComponent();

            check_DLL();

        }

        private void btnGenerateKey_Click(object sender, EventArgs e)
        {
            if (!check_DLL()) return;

            try
            {
                var type = Type.GetType("libAES.MyAES, libAES");
                if (type != null)
                {
                    var aesInstance = Activator.CreateInstance(type);
                    var method = type.GetMethod("genKey");
                    if (method != null)
                    {
                        txtKey.Text = method.Invoke(aesInstance, null)?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                labelThongBao.Text = "Error: " + ex.Message;
            }
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            // Code to browse and select input file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn file cần mã hoá AES";
            openFileDialog.CheckFileExists = true; // file phải tồn tại
            openFileDialog.Multiselect = false;//chỉ cho 1 file
            openFileDialog.Filter = "All file (*.*)|*.*|Chỉ chọn ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.tif;*.webp;*.ico;*.jfif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog.FileName;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Chọn file đích lưu kết quả mã hoá";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.OverwritePrompt = false; // Tắt cảnh báo mặc định đi để tự xử lý cho chắc chắn
            saveFileDialog.Filter = "All file (*.*)|*.*|Chỉ chọn ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.tif;*.webp;*.ico;*.jfif";

            while (true)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Kiểm tra xem file đã tồn tại thực tế chưa
                    if (System.IO.File.Exists(saveFileDialog.FileName))
                    {
                        MessageBox.Show("File này đã tồn tại! Vui lòng chọn một tên file mới để tránh ghi đè.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue; // Mở lại hộp thoại cho người dùng chọn lại
                    }

                    // Nếu file chưa tồn tại thì nhận kết quả và thoát vòng lặp
                    txtOutputFile.Text = saveFileDialog.FileName;
                    break;
                }
                else
                {
                    // Người dùng bấm Cancel
                    break;
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!check_DLL()) return;

            try
            {
                var type = Type.GetType("libAES.MyAES, libAES");
                if (type != null)
                {
                    var aesInstance = Activator.CreateInstance(type);
                    var method = type.GetMethod("MaHoaAESFile");

                    if (method != null)
                    {
                        method.Invoke(aesInstance, new object[] { txtInputFile.Text, txtOutputFile.Text, txtKey.Text });
                        labelThongBao.Text = "AES Encrypted OK";
                    }
                }
            }
            catch (Exception ex)
            {
                labelThongBao.Text = "Error: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        private void btnGiaiMa_Click(object sender, EventArgs e)
        {
            if (!check_DLL()) return;
            try
            {
                var type = Type.GetType("libAES.MyAES, libAES");
                if (type != null)
                {
                    var aesInstance = Activator.CreateInstance(type);
                    var method = type.GetMethod("GiaiMaAESFile");

                    if (method != null)
                    {
                        method.Invoke(aesInstance, new object[] { txtInputFile.Text, txtOutputFile.Text, txtKey.Text });
                        labelThongBao.Text = "AES DeCrypted OK";
                    }
                }
            }
            catch (Exception ex)
            {
                labelThongBao.Text += "Error: " + (ex.InnerException?.Message ?? ex.Message) + "\n";
            }
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            new frmAES_txt().Show();
        }
    }
}
