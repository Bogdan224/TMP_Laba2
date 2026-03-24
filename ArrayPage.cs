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

        protected Array elements = null!;

        protected const int AdditionalFieldsSize = _modificationFlagSize + _openToWriteFlagSize
           + _pageUsingCountSize + _pageInMemoryTimeSize + _pageIndexSize;
        // AdditionalFieldsSize = 1 + 1 + 4 + 4 + 4 = 14

        public const int TotalElementsCount = 128;

        public bool ModificationFlag { get; set; } = false;
        public bool OpenToWriteFlag { get; set; } = true;
        public int PageUsingCount { get; set; } = 0;
        public int PageInMemoryTime { get; set; } = 0;
        public int PageIndex { get; set; }

        public Array Elements => elements;
        public BitArray Bitmap { get; private set; }

        public ArrayPage()
        {
            Bitmap = new BitArray(TotalElementsCount);
        }

        public ArrayPage(byte[] bytes, ref int offset) : this()
        {
            FromBytes(bytes, ref offset);
        }

        private void FromBytes(byte[] bytes, ref int offset)
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

        public void SetElementByIndex(int index, object value)
        {
            if (!OpenToWriteFlag)
                throw new Exception("Страница закрыта для записи!");

            Elements.SetValue(value, index);
            Bitmap.Set(index, true);
            ModificationFlag = true;
            PageUsingCount++;
        }
    }

    public class IntArrayPage : ArrayPage
    {
        private const int elementSize = 4;

        public new int[] Elements => (int[])elements;

        public IntArrayPage()
        {
            elements = new int[TotalElementsCount];
        }

        public IntArrayPage(byte[] bytes, ref int offset, int elementSize = 4) : base(bytes, ref offset)
        {
            elements = new int[TotalElementsCount];

            FromBytes(bytes, ref offset, elementSize);
        }

        public override byte[] ToBytes(int elementSize = elementSize)
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

        private void FromBytes(byte[] bytes, ref int offset, int elementSize = elementSize)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToInt32(bytes, offset);
                offset += elementSize;
            }
        }
    }

    public class CharArrayPage : ArrayPage
    {
        private const int elementSize = 2;

        public new char[] Elements => (char[])elements;

        public CharArrayPage()
        {
            elements = new char[TotalElementsCount];
        }

        public CharArrayPage(byte[] bytes, ref int offset) : base(bytes, ref offset)
        {
            elements = new char[TotalElementsCount];

            FromBytes(bytes, ref offset, elementSize);
        }

        public override byte[] ToBytes(int elementSize = elementSize)
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

        private void FromBytes(byte[] bytes, ref int offset, int elementSize = elementSize)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToChar(bytes, offset);
                offset += elementSize;
            }
        }
    }

    public class StringArrayPage : ArrayPage
    {
        public new string[] Elements => (string[])elements;

        public StringArrayPage()
        {
            elements = new string[TotalElementsCount];
        }

        public StringArrayPage(byte[] bytes, ref int offset, int elementSize) : base(bytes, ref offset)
        {
            elements = new string[TotalElementsCount];

            FromBytes(bytes, ref offset, elementSize);
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

        private void FromBytes(byte[] bytes, ref int offset, int elementSize)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = Encoding.UTF8.GetString(bytes, offset, elementSize);
                offset += elementSize;
            }
        }
    }
}
