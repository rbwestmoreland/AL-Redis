using System;

namespace AngiesList.Redis
{
	public class CompressedValueSerializer : IValueSerializer
	{
		private readonly IValueSerializer _serializer;
		private readonly RedisSessionStateConfiguration _redisConfig;

		public CompressedValueSerializer(IValueSerializer serializer)
		{
			if (serializer == null)
				throw new ArgumentNullException("serializer");

			_serializer = serializer;
			_redisConfig = RedisSessionStateConfiguration.GetConfiguration();
		}

		public byte[] Serialize(object value)
		{
			var bytes = _serializer.Serialize(value);
			var compressedBytes = _redisConfig.CompressionEnabled ? Gzip.Compress(bytes) : bytes;

			return compressedBytes;
		}

		public object Deserialize(byte[] bytes)
		{
			if (bytes == null)
				return null;

			var decompressedBytes = Gzip.IsCompressed(bytes) ? Gzip.Decompress(bytes) : bytes;
			var value = _serializer.Deserialize(decompressedBytes);

			return value;
		}
	}
}
