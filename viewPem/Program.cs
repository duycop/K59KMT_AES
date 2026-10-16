using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Đọc nội dung file public_key.pem
        string pemContent = File.ReadAllText("private_key.pem");

        // 2. Lọc bỏ dòng header, footer và khoảng trắng/xuống dòng
        string base64Body = pemContent
            .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            .Replace("-----END RSA PRIVATE KEY-----", "")
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\r", "")
            .Replace("\n", "")
            .Trim();

        // 3. Giải mã Base64 thành mảng byte
        byte[] bytes = Convert.FromBase64String(base64Body);

        // 4. Hiển thị mảng byte dưới dạng hex (hệ 16)
        Console.WriteLine($"Tổng số byte: {bytes.Length}\n");
        for (int i = 0; i < bytes.Length; i++)
        {
            Console.Write($"{bytes[i]:X2} ");
            if ((i + 1) % 16 == 0) Console.WriteLine(); // Xuống dòng mỗi 16 byte cho dễ soi
        }
    }
}