using System;

namespace AngiesList.Redis
{
	public class CompressedValueSerializer : IValueSerializer
	{
		private readonly IValueSerializer _serializer;

		public CompressedValueSerializer(IValueSerializer serializer)
		{
			if (serializer == null)
				throw new ArgumentNullException("serializer");

			_serializer = serializer;
		}

		public byte[] Serialize(object value)
		{
			var bytes = _serializer.Serialize(value);
			var compressedBytes = Gzip.Compress(bytes);

			return compressedBytes;
		}

		public object Deserialize(byte[] bytes)
		{
			if (bytes == null)
				return null;

			var decompressedBytes = Gzip.Decompress(bytes);
			var value = _serializer.Deserialize(decompressedBytes);

			return value;
		}
	}
}
