using System.Text;

namespace TMP_Laba2
{
    public class FileManager : IDisposable
    {
        private static string _path = @$"C:\Users\{Environment.UserName}\Downloads\";

        private ArrayHeader _arrayHeader;

        private FileStream _header;

        private FileManager(FileStream fileHeader, ArrayHeader arrayHeader)
        {
            _header = fileHeader;
            _arrayHeader = arrayHeader;
        }

        public static FileManager CreateIntArrayFiles(string filename, long arraySize = 10000)
        {
            int[] array = new int[arraySize];
            byte[] buffer = new byte[arraySize * 4];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);
            
            var header = new IntArrayHeader(arraySize);

            Array.Copy(header.ToBytes(), buffer, header.AdditionalFieldsSize);
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, buffer.Length);

            filestream.Write(buffer);

            return new FileManager(filestream, new IntArrayHeader(arraySize));
        }

        public static FileManager CreateCharArrayFiles(string filename, long arraySize = 10000)
        {
            char[] array = new char[arraySize];
            byte[] buffer = new byte[arraySize * 2];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);

            var header = new IntArrayHeader(arraySize);

            Array.Copy(header.ToBytes(), buffer, header.AdditionalFieldsSize);
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, buffer.Length);

            filestream.Write(buffer);

            return new FileManager(filestream, new CharArrayHeader(arraySize));
        }

        public static FileManager CreateStringArrayFiles(string filename, int charCount, long arraySize = 10000)
        {
            string[] array = new string[arraySize];
            
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < charCount; i++)
            {
                stringBuilder.Append('\0');
            }
            var str = stringBuilder.ToString();
            Array.Fill(array, str);

            byte[] buffer = new byte[arraySize * charCount];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);

            var header = new IntArrayHeader(arraySize);

            Array.Copy(header.ToBytes(), buffer, header.AdditionalFieldsSize);
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, buffer.Length);

            filestream.Write(buffer);

            return new FileManager(filestream, new StringArrayHeader(arraySize, charCount));
        }

        public static FileManager OpenFiles(string compFilename)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _header?.Dispose();
        }

        public void AddValueToArray(string index, string value)
        {

        }


    }
}
