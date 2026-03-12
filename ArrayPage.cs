using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace TMP_Laba2
{
    public abstract class ArrayPage : IPageSerializable
    {
        private const int _modificationFlagSize = 1;
        private const int _openToWriteFlagSize = 1;
        private const int _pageUsingCountSize = 4;
        private const int _pageInMemoryTimeSize = 4;
        private const int _pageIndexSize = 4;
        private const int _bitmapSize = TotalElementsCount / 8;

        protected const int AdditionalFieldsSize = _modificationFlagSize + _openToWriteFlagSize
           + _pageUsingCountSize + _pageInMemoryTimeSize + _pageIndexSize + TotalElementsCount;

        public const int TotalElementsCount = 128;

        public bool ModificationFlag { get; set; } = false;
        public bool OpenToWriteFlag { get; set; } = true;
        public int PageUsingCount { get; set; } = 0;
        public int PageInMemoryTime { get; set; } = 0;
        public int PageIndex { get; set; }

        public BitArray Bitmap { get; private set; }

        public ArrayPage()
        {
            Bitmap = new BitArray(TotalElementsCount);
        }

        public virtual byte[] ToBytes(int elementSize = 0)
        {
            byte[] bytes = new byte[AdditionalFieldsSize];
            int offset = 0;

            BitConverter.GetBytes(ModificationFlag).CopyTo(bytes, offset);
            offset += _modificationFlagSize;

            BitConverter.GetBytes(OpenToWriteFlag).CopyTo(bytes, offset);
            offset += _openToWriteFlagSize;

            BitConverter.GetBytes(PageUsingCount).CopyTo(bytes, offset);
            offset += _pageUsingCountSize;

            BitConverter.GetBytes(PageInMemoryTime).CopyTo(bytes, offset);
            offset += _pageInMemoryTimeSize;

            Array.Copy(BitConverter.GetBytes(PageIndex), 0, bytes, offset, _pageIndexSize);
            offset += _pageIndexSize;

            byte[] bitmapBytes = new byte[_bitmapSize];
            Bitmap.CopyTo(bitmapBytes, 0);
            bitmapBytes.CopyTo(bytes, offset);
            offset += _bitmapSize;

            return bytes;
        }

        public virtual void FromBytes(byte[] bytes, ref int offset, int elementSize = 0)
        {
            ModificationFlag = BitConverter.ToBoolean(bytes, offset);
            offset += _modificationFlagSize;

            OpenToWriteFlag = BitConverter.ToBoolean(bytes, offset);
            offset += _openToWriteFlagSize;

            PageUsingCount = BitConverter.ToInt32(bytes, offset);
            offset += _pageUsingCountSize;

            PageInMemoryTime = BitConverter.ToInt32(bytes, offset);
            offset += _pageInMemoryTimeSize;

            PageIndex = BitConverter.ToInt32(bytes, offset);
            offset += _pageIndexSize;

            byte[] bitmapBytes = new byte[_bitmapSize];
            Array.Copy(bytes, offset, bitmapBytes, 0, _bitmapSize);
            Bitmap = new BitArray(bitmapBytes);
            offset += _bitmapSize;
        }
    }

    public class IntArrayPage : ArrayPage
    {
        public int[] Elements { get; }

        public IntArrayPage()
        {
            Elements = new int[TotalElementsCount];
        }

        public override byte[] ToBytes(int elementSize)
        {
            byte[] bytes = new byte[TotalElementsCount * elementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(BitConverter.GetBytes(element), 0, bytes, offset, elementSize);
                offset += elementSize;
            }

            bytes = base.ToBytes(elementSize).Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset, int elementSize)
        {
            base.FromBytes(bytes, ref offset, elementSize);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToInt32(bytes, offset);
                offset += elementSize;
            }
        }
    }

    public class CharArrayPage : ArrayPage
    {
        public char[] Elements { get; }

        public CharArrayPage()
        {
            Elements = new char[TotalElementsCount];
        }

        public override byte[] ToBytes(int elementSize)
        {
            byte[] bytes = new byte[TotalElementsCount * elementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(BitConverter.GetBytes(element), 0, bytes, offset, elementSize);
                offset += elementSize;
            }

            bytes = base.ToBytes(elementSize).Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset, int elementSize)
        {
            base.FromBytes(bytes, ref offset, elementSize);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToChar(bytes, offset);
                offset += elementSize;
            }
        }
    }

    public class StringArrayPage : ArrayPage
    {
        public string[] Elements { get; }

        public StringArrayPage()
        {
            Elements = new string[TotalElementsCount];
        }

        public override byte[] ToBytes(int elementSize)
        {
            byte[] bytes = new byte[TotalElementsCount * elementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(Encoding.UTF8.GetBytes(element), 0, bytes, offset, elementSize);
                offset += elementSize;
            }

            bytes = base.ToBytes(elementSize).Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset, int elementSize)
        {
            base.FromBytes(bytes, ref offset, elementSize);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = Encoding.UTF8.GetString(bytes, offset, elementSize);
                offset += elementSize;
            }
        }
    }
}
