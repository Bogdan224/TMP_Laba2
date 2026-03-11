namespace TMP_Laba2
{
    public class FileManager : IDisposable
    {
        private static string _path = @$"C:\Users\{Environment.UserName}\Downloads\";

        private ArrayHeader arrayHeader;
        private ArrayType arrayType;

        private FileStream _header;

        private FileManager(FileStream fileHeader, ArrayHeader arrayHeader)
        {
            _header = fileHeader;
        }

        public static FileManager CreateFiles(string filename, ushort recordLength = 20)
        {

            throw new NotImplementedException();
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
