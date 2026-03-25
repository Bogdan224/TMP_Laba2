using System.IO;
using System.IO.Pipes;
using System.Reflection.PortableExecutable;
using System.Text;

namespace TMP_Laba2
{
    public class FileManager : IDisposable
    {
        private const int _pageAdditionalFieldsSize = 30;
        private const int _elementsPerPage = 128;

        private static string _path = @$"C:\Users\{Environment.UserName}\Downloads\";

        private ArrayHeader _arrayHeader;

        private FileStream _filestream;

        private int _pageSize;

        private FileManager(FileStream fileHeader, ArrayHeader arrayHeader)
        {
            _filestream = fileHeader;
            _arrayHeader = arrayHeader;

            _pageSize = _arrayHeader.TotalPageElementsSize + _pageAdditionalFieldsSize;
        }

        public static FileManager CreateIntArrayFiles(string filename, long arraySize = 10000)
        {
            var header = new IntArrayHeader(arraySize);

            byte[] buffer = new byte[_pageAdditionalFieldsSize];

            var filestream = new FileStream(_path + filename, FileMode.Create);

            buffer = header.ToBytes().Concat(buffer).ToArray();

            for (int i = 0; i < header.PageCount; i++)
            {
                var charArrayPage = new IntArrayPage(i + 1);

                buffer = buffer.Concat(charArrayPage.ToBytes(header.ElementSize)).ToArray();
            }

            filestream.Write(buffer);

            return new FileManager(filestream, header);
        }

        public static FileManager CreateCharArrayFiles(string filename, long arraySize = 10000)
        {
            var header = new CharArrayHeader(arraySize);

            byte[] buffer = new byte[_pageAdditionalFieldsSize];

            var filestream = new FileStream(_path + filename, FileMode.Create);

            buffer = header.ToBytes().Concat(buffer).ToArray();

            for (int i = 0; i < header.PageCount; i++)
            {
                var charArrayPage = new CharArrayPage(i + 1);

                buffer = buffer.Concat(charArrayPage.ToBytes()).ToArray();   
            }

            filestream.Write(buffer);

            return new FileManager(filestream, header);
        }

        public static FileManager CreateStringArrayFiles(string filename, int charCount, long arraySize = 10000)
        {
            var header = new StringArrayHeader(arraySize, charCount);

            byte[] buffer = new byte[_pageAdditionalFieldsSize];

            var filestream = new FileStream(_path + filename, FileMode.Create);

            buffer = header.ToBytes().Concat(buffer).ToArray();

            for (int i = 0; i < header.PageCount; i++)
            {
                var charArrayPage = new StringArrayPage(i + 1, header.ElementSize);

                buffer = buffer.Concat(charArrayPage.ToBytes(header.ElementSize)).ToArray();
            }

            filestream.Write(buffer);

            return new FileManager(filestream, header);
        }

        public static FileManager OpenFiles(string filename)
        {
            var filestream = new FileStream(_path + filename, FileMode.Open);

            var buffer = new byte[ArrayHeader.AdditionalFieldsSize];
            int offset = 0;

            filestream.Read(buffer, 0, buffer.Length);

            byte[] tmpBuffer;

            ArrayHeader header;
            if ((ArrayType)buffer.Last() == ArrayType.Int)
            {
                header = new IntArrayHeader(buffer, ref offset);

                tmpBuffer = new byte[(header.TotalPageElementsSize + _pageAdditionalFieldsSize) * 3];
            }
            else if ((ArrayType)buffer.Last() == ArrayType.Char)
            {
                header = new CharArrayHeader(buffer, ref offset);

                tmpBuffer = new byte[(header.TotalPageElementsSize + _pageAdditionalFieldsSize) * 3];   
            }
            else if ((ArrayType)buffer.Last() == ArrayType.String) 
            {
                header = new StringArrayHeader(buffer, ref offset);

                tmpBuffer = new byte[(header.TotalPageElementsSize + _pageAdditionalFieldsSize) * 3];
            }
            else
                throw new Exception("Запись типа массива была совершена неверно!");

            filestream.Read(tmpBuffer, 0, tmpBuffer.Length);

            offset = 0;
            header.RestorePagesFromBytes(tmpBuffer, ref offset);

            return new FileManager(filestream, header);
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
            int headerSize = ArrayHeader.AdditionalFieldsSize;
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
                page = new IntArrayPage(buff, ref pageOffset);
            else if (_arrayHeader.ArrayType == ArrayType.String)
                page = new StringArrayPage(buff, ref pageOffset, _arrayHeader.ElementSize);
            else
                page = new CharArrayPage(buff, ref pageOffset);

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
