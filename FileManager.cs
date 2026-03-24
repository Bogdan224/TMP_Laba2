using System.IO;
using System.IO.Pipes;
using System.Reflection.PortableExecutable;
using System.Text;

namespace TMP_Laba2
{
    public class FileManager : IDisposable
    {
        private static string _path = @$"C:\Users\{Environment.UserName}\Downloads\";

        private ArrayHeader _arrayHeader;

        private FileStream _filestream;

        private byte[] _currentPageBuffer;

        private const int PageSize = 526 + 16;
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
            filestream.Seek(0, SeekOrigin.Begin);

            return new FileManager(filestream, header);
        }

        public static FileManager CreateCharArrayFiles(string filename, long arraySize = 10000)
        {
            char[] array = new char[arraySize];
            byte[] buffer = new byte[arraySize * 2];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);

            var header = new CharArrayHeader(arraySize);

            Array.Copy(header.ToBytes(), buffer, header.AdditionalFieldsSize);
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, buffer.Length);

            filestream.Write(buffer);

            return new FileManager(filestream, header);
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

            var header = new StringArrayHeader(arraySize, charCount);

            Array.Copy(header.ToBytes(), buffer, header.AdditionalFieldsSize);
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, buffer.Length);

            filestream.Write(buffer);

            return new FileManager(filestream, header);
        }

        public static FileManager OpenFiles(string compFilename)
        {
            throw new NotImplementedException();
        }

        private void UpdateFile(byte[] bytes, int offset, int length)
        {
            _filestream.Seek(0, SeekOrigin.Begin);
            _filestream.Write(bytes, offset, length);
        }

        public void Dispose()
        {
            _filestream?.Dispose();
        }

        public void AddValueToArray(int index, int value)
        {
            if (_arrayHeader.ArrayType != ArrayType.Int)
                throw new Exception();

            int offset = GetPageIndex(index);

            byte[] buff = new byte[PageSize];

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Read(buff, 0, PageSize);

            int pageOffset = 0;

            var page = new IntArrayPage(buff, ref pageOffset);

            int localIndex = index % ElementsPerPage;

            page.Elements[localIndex] = value;
            page.Bitmap.Set(localIndex, true);
            page.ModificationFlag = true;

            byte[] pageBytes = page.ToBytes();

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Write(pageBytes, 0, pageBytes.Length);
        }

        public int GetPageIndex(int index)
        {
            int pageIndex = index / ElementsPerPage;
            // index = 20 ElementsPerPage = 128 // pageIndex = 0
            // index = 128 ElementsPerPage = 128 // pageIndex = 1

            int headerSize = _arrayHeader.AdditionalFieldsSize;
            // headerSize = 17

            int offset = headerSize + pageIndex * PageSize;
            // 17 + 0 * 526 = 17

            return offset;
        }

        public void AddValueToArray(int index, string value)
        {
            if (_arrayHeader.ArrayType != ArrayType.String) throw new NotImplementedException();

            GetPageIndex(index);
            //var page = new StringArrayPage(_currentPageBuffer, ref index);
        }

        public void AddValueToArray(int index, char value)
        {

        }


    }
}
