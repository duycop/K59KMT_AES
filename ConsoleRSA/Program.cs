using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Hello, World!");

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


    rsa.ExportParameters(true);
    // 4. Xuất khóa ra chuỗi định dạng PEM
    string publicKey = rsa.ExportRSAPublicKeyPem();
    string privateKey = rsa.ExportRSAPrivateKeyPem();

    // 5. Lưu vào file text
    File.WriteAllText("public_key.pem", publicKey);
    File.WriteAllText("private_key.pem", privateKey);

    //đọc nội dung file public_key.pem
    //lấy phần body , giải mã base64 thành mảng bytes
    //hiển thị các byte lên màn hình (2 kí tự ở hệ 16)

}