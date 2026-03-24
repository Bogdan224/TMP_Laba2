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

        private int _pageSize;
        private const int _pageAdditionalFieldsSize = 30;
        private const int _elementsPerPage = 128;

        private FileManager(FileStream fileHeader, ArrayHeader arrayHeader)
        {
            _filestream = fileHeader;
            _arrayHeader = arrayHeader;

            _pageSize = _arrayHeader.TotalPageElementsSize + _pageAdditionalFieldsSize;
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

            filestream.Seek(0, SeekOrigin.Begin);
            filestream.Write(buffer);

            return new FileManager(filestream, header);
        }

        public static FileManager CreateCharArrayFiles(string filename, long arraySize = 10000)
        {
            char[] array = new char[arraySize];
            long arrayBytes = arraySize * 2;
            byte[] buffer = new byte[arrayBytes];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);

            var header = new CharArrayHeader(arraySize);

            buffer = header.ToBytes().Concat(buffer).ToArray();
            offset += header.AdditionalFieldsSize;

            Buffer.BlockCopy(array, 0, buffer, offset, Convert.ToInt32(arrayBytes));

            filestream.Seek(0, SeekOrigin.Begin);
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
            var str1 = stringBuilder.ToString();
            Array.Fill(array, str1);

            long arrayBytes = arraySize * charCount * 2;
            byte[] buffer = new byte[arrayBytes];
            int offset = 0;

            var filestream = new FileStream(filename, FileMode.Create);

            var header = new StringArrayHeader(arraySize, charCount);

            buffer = header.ToBytes().Concat(buffer).ToArray();
            offset += header.AdditionalFieldsSize;

            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i] ?? string.Empty;
                char[] chars = str.PadRight(charCount).ToCharArray();
                Buffer.BlockCopy(chars, 0, buffer, offset + (i * charCount * 2), charCount * 2);
            }

            filestream.Seek(0, SeekOrigin.Begin);
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

            byte[] buff = new byte[_pageSize];

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Read(buff, 0, _pageSize);

            int pageOffset = 0;

            var page = new IntArrayPage(buff, ref pageOffset);

            int localIndex = index % _elementsPerPage;

            page.SetElementByIndex(localIndex, value); 

            byte[] pageBytes = page.ToBytes();

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Write(pageBytes, 0, pageBytes.Length);
        }

        private int GetPageIndex(int index)
        {
            int pageIndex = index / _elementsPerPage;
            int headerSize = _arrayHeader.AdditionalFieldsSize;
            int offset = headerSize + pageIndex * _pageSize;

            return offset;
        }

        public void AddValueToArray(int index, string value)
        {
            if (_arrayHeader.ArrayType != ArrayType.String) 
                throw new NotImplementedException();

            int offset = GetPageIndex(index);

            byte[] buff = new byte[_pageSize];

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Read(buff, 0, _pageSize);

            int pageOffset = 0;

            var page = new StringArrayPage(buff, ref pageOffset, _arrayHeader.ElementSize);

            int localIndex = index % _elementsPerPage;

            page.SetElementByIndex(localIndex, value);

            byte[] pageBytes = page.ToBytes(_arrayHeader.ElementSize);

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Write(pageBytes, 0, pageBytes.Length);
        }

        public void AddValueToArray(int index, char value)
        {
            if (_arrayHeader.ArrayType != ArrayType.Char)
                throw new Exception();

            int offset = GetPageIndex(index);

            byte[] buff = new byte[_pageSize];

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Read(buff, 0, _pageSize);

            int pageOffset = 0;

            var page = new CharArrayPage(buff, ref pageOffset);

            int localIndex = index % _elementsPerPage;

            page.SetElementByIndex(localIndex, value);

            byte[] pageBytes = page.ToBytes();

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Write(pageBytes, 0, pageBytes.Length);
        }

        private ArrayPage GetPage(int index)
        {
            int offset = GetPageIndex(index);

            byte[] buff = new byte[_pageSize];

            _filestream.Seek(offset, SeekOrigin.Begin);
            _filestream.Read(buff, 0, _pageSize);

            int pageOffset = 0;

            ArrayPage page;

            if (_arrayHeader.ArrayType == ArrayType.Int)
            {
                page = new IntArrayPage(buff, ref pageOffset);
            }
            //if (_arrayHeader.ArrayType == ArrayType.String)
            //{
            //     page = new StringArrayPage(buff, ref pageOffset);
            //}
            else
            {
                page = new CharArrayPage(buff, ref pageOffset);
            }

            return page;
        }

        public void Print(int index)
        {
           var page = GetPage(index);

            int localIndex = index % _elementsPerPage;

            Console.WriteLine(page.Elements.GetValue(localIndex));
        }


    }
}
