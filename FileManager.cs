using System.Reflection.PortableExecutable;
using System.Text;

namespace TMP_Laba2
{
    public class FileManager : IDisposable
    {
        private static string _path = @$"C:\Users\{Environment.UserName}\Downloads\";

        private ArrayHeader _arrayHeader;

        private FileStream _filestream;

        private const int PageSize = 526;
        private const int ElementsPerPage = 128;

        private FileManager(FileStream fileHeader, ArrayHeader arrayHeader)
        {
            _filestream = fileHeader;
            _arrayHeader = arrayHeader;
        }

        public static FileManager CreateIntArrayFiles(string filename, long arraySize = 10000)
        {
            int[] array = new int[arraySize];
            long arrayBytes = arraySize * 4;
            byte[] buffer = new byte[arrayBytes];
            int offset = 0;

            var filestream = new FileStream(_path + filename, FileMode.Create);
            
            var header = new IntArrayHeader(arraySize);

            buffer = header.ToBytes().Concat(buffer).ToArray();
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, Convert.ToInt32(arrayBytes));

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

        private void UpdateFile(byte[] bytes, int offset, int length)
        {
            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Write(bytes, offset, length);
        }

        public void Dispose()
        {
            _filestream?.Dispose();
        }


        public void AddValueToArray(int index, int value)
        {
            if (_arrayHeader.ArrayType != ArrayType.Int) throw new NotImplementedException();

            byte[] buffer = new byte[PageSize];

            int pageIndex = index / ElementsPerPage;

            _filestream.Read(buffer, (pageIndex - 1) * PageSize, PageSize);

            var page = new IntArrayPage(buffer, ref pageIndex);

            var elements = page.Elements;

            elements[index] = value;
        }

        public void AddValueToArray(int index, string value)
        {
            if (_arrayHeader.ArrayType != ArrayType.String) throw new NotImplementedException();


        }

        public void AddValueToArray(int index, char value)
        {

        }


    }
}
