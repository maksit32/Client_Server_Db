using APIServer.Domain.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;


namespace APIServer.Services
{
	public class MessageHasherService : IMessageHasherService
	{
		private readonly byte[] _key;
		private readonly byte[] _iv;

		public MessageHasherService(string key)
		{
			// Генерация ключа и вектора инициализации на основе заданной строки
			using (var sha256 = SHA256.Create())
			{
				_key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
			}

			// Используем фиксированный вектор инициализации (IV) для упрощения примера
			// В реальных приложениях лучше генерировать случайный IV для каждого шифрования
			_iv = new byte[16]; // 16 байт для AES
			Array.Copy(_key, _iv, 16); // Копируем первые 16 байт ключа в IV
		}

		public string Encrypt(string plainText)
		{
			using (var aes = Aes.Create())
			{
				aes.Key = _key;
				aes.IV = _iv;

				var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
				using (var ms = new MemoryStream())
				{
					using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						using (var sw = new StreamWriter(cs))
						{
							sw.Write(plainText);
						}
					}
					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public string Decrypt(string cipherText)
		{
			using (var aes = Aes.Create())
			{
				aes.Key = _key;
				aes.IV = _iv;

				var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
				using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
				{
					using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					{
						using (var sr = new StreamReader(cs))
						{
							return sr.ReadToEnd();
						}
					}
				}
			}
		}
	}
}
