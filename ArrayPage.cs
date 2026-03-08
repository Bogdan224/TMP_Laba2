using System.Runtime.InteropServices;

namespace TMP_Laba2
{
    public class ArrayPage<T>
    {
        private int _typeSize;
        private int _totalSize;

        public int TotalElementsSize
        {
            get => (_totalSize - _modificationFlagSize - _openToWriteFlagSize
                - _pageUsingCountSize - _pageInMemoryTimeSize);
        }

        private const int _modificationFlagSize = 1;
        private const int _openToWriteFlagSize = 1;
        private const int _pageUsingCountSize = 4;
        private const int _pageInMemoryTimeSize = 4;

        public bool ModificationFlag { get; set; } = false;
        public bool OpenToWriteFlag { get; set; } = true;
        public int PageUsingCount { get; set; } = 0;
        public int PageInMemoryTime { get; set; } = 0;

        public T[] Elements { get; }

        public ArrayPage(int totalSize = 512)
        {
            _totalSize = totalSize;
            _typeSize = Marshal.SizeOf<T>();
            Elements = new T[TotalElementsSize];
        }
    }
}
