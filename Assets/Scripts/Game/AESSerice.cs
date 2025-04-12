using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AESSerice
{
    private const string encryptionKey = "Your32ByteLongSecretKeyHere!1234"; // 32 chars for AES256

    public static string EncryptAES(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = new byte[16];

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return System.Convert.ToBase64String(ms.ToArray());
    }
    public static string DecryptAES(string cipherText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = new byte[16];

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
