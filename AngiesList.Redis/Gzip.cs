using System.IO;
using System.IO.Compression;

namespace AngiesList.Redis
{
	public static class Gzip
	{
		public static bool IsCompressed(byte[] bytes)
		{
			return bytes.Length >= 2 && bytes[0] == 31 && bytes[1] == 139;
		}

		public static byte[] Compress(byte[] bytes)
		{
			byte[] compressedBytes;

			using (var stream = new MemoryStream())
			{
				using (var gzipStream = new GZipStream(stream, CompressionMode.Compress, true))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}

				compressedBytes = stream.ToArray();
			}

			return compressedBytes;
		}

		public static byte[] Decompress(byte[] bytes)
		{
			using (var gzipStream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
			{
				const int size = 4096;
				var buffer = new byte[size];

				using (var stream = new MemoryStream())
				{
					int count;

					do
					{
						count = gzipStream.Read(buffer, 0, size);
						if (count > 0)
						{
							stream.Write(buffer, 0, count);
						}
					}
					while (count > 0);

					return stream.ToArray();
				}
			}
		}
	}
}
