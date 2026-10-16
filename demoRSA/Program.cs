using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;

namespace demoRSA
{
    internal class Program
    {
        static Random r = new Random();
        static char random()
        {
            char randomNum =(char) (r.Next(32, 128)); // Đã bao gồm cả b
            return randomNum;
        }
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Tạo cặp khóa RSA (ví dụ 2048-bit)
            using (RSA rsa = RSA.Create(2048))
            {
                // 2. Mã hóa & Giải mã 
                string planText = "anh yêu em từ chân đến Cổ @@@@@xyz";//ko quá 190 ki tự
                byte[] data = Encoding.UTF8.GetBytes(planText);
               
                byte[] encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
                byte[] decrypted = rsa.Decrypt(encrypted, RSAEncryptionPadding.OaepSHA256);

                Console.WriteLine($"plan text       : {planText}");
                Console.WriteLine($"data length     : {data.Length}");
                Console.WriteLine($"encrypted length: {encrypted.Length}");
                Console.WriteLine($"decrypted length: {decrypted.Length}");

                string giai_ma = Encoding.UTF8.GetString(decrypted);
                Console.WriteLine($"decrypted       : {giai_ma}");
               
                // 3. Tạo chữ ký số
                byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                bool isValid = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                Console.WriteLine($"signature length: {signature.Length}");
                Console.WriteLine($"           Valid: {isValid}");


                
            }
        }
    }
}
