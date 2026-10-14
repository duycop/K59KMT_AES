using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace libAES
{
    public class MyAES
    {
        // AES Key & IV
        private byte[] aesKey;  //key
        private byte[] aesIV;   //vector khởi tạo (Init Vector)

        public string genKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();// IV ngẫu nhiên 16 bytes

                aesKey = aes.Key;
                aesIV = aes.IV;

                return Convert.ToBase64String(aesKey);
            }
        }
        public string MaHoaAES(string txtInput)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(txtInput);
            byte[] resultBytes = MaHoaAES(plainBytes);
            return Convert.ToBase64String(resultBytes);
        }

        public byte[] MaHoaAES(byte[] plainBytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                //mã hoá
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Gộp IV (16 bytes) + cipherBytes thành 1 mảng duy nhất
                byte[] resultBytes = new byte[aes.IV.Length + cipherBytes.Length];
                Buffer.BlockCopy(aes.IV, 0, resultBytes, 0, aes.IV.Length);
                Buffer.BlockCopy(cipherBytes, 0, resultBytes, aes.IV.Length, cipherBytes.Length);

                return resultBytes;
            }
        }

        public string GiaiMaAES(string fullBase64Text, string keyBase64)
        {
            byte[] fullBytes = Convert.FromBase64String(fullBase64Text); // 16byte IV + Ciphertext
            byte[] decryptedBytes = GiaiMaAES(fullBytes, keyBase64);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public byte[] GiaiMaAES(byte[] fullBytes, string keyBase64)
        {
            byte[] key = Convert.FromBase64String(keyBase64);  //ok

            // Tách 16 bytes đầu làm IV, phần còn lại là Ciphertext
            byte[] iv = new byte[16];
            byte[] cipherBytes = new byte[fullBytes.Length - 16];

            //syntax: Buffer.BlockCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, count);
            Buffer.BlockCopy(fullBytes, 0, iv, 0, 16); //copy 16 byte => iv
            Buffer.BlockCopy(fullBytes, 16, cipherBytes, 0, cipherBytes.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return decryptedBytes;
            }
        }
        public void MaHoaAESFile(string inputFilePath, string outputFilePath, string keyBase64)
        {
            byte[] key = Convert.FromBase64String(keyBase64);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Tạo IV ngẫu nhiên mới cho mỗi lần mã hóa
                byte[] iv = aes.IV;

                using (var fsInput = new System.IO.FileStream(inputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                using (var fsOutput = new System.IO.FileStream(outputFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    // Ghi 16 bytes IV đầu tiên vào file đích
                    fsOutput.Write(iv, 0, iv.Length);

                    using (var encryptor = aes.CreateEncryptor())
                    using (var cs = new System.Security.Cryptography.CryptoStream(fsOutput, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        // Đọc và ghi theo luồng (buffer 80KB)
                        byte[] buffer = new byte[81920];
                        int read;
                        while ((read = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cs.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }
        public void GiaiMaAESFile(string inputFilePath, string outputFilePath, string keyBase64)
        {
            byte[] key = Convert.FromBase64String(keyBase64);

            using (var fsInput = new System.IO.FileStream(inputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                // Đọc 16 bytes đầu tiên làm IV
                byte[] iv = new byte[16];
                fsInput.Read(iv, 0, 16);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var fsOutput = new System.IO.FileStream(outputFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    using (var cs = new System.Security.Cryptography.CryptoStream(fsInput, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        // Đọc và ghi theo luồng, không tốn RAM
                        byte[] buffer = new byte[81920]; // Buffer 80KB
                        int read;
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fsOutput.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }
    }
}
