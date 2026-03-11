using System.Runtime.InteropServices;
using System.Text;

namespace TMP_Laba2
{
    public abstract class ArrayPage : ISerializable
    {
        protected const int _modificationFlagSize = 1;
        protected const int _openToWriteFlagSize = 1;
        protected const int _pageUsingCountSize = 4;
        protected const int _pageInMemoryTimeSize = 4;
        protected const int _pageIndexSize = 4;

        public static int TotalElementsCount => 128;

        public static int AdditionalFieldsSize => _modificationFlagSize + _openToWriteFlagSize
            + _pageUsingCountSize + _pageInMemoryTimeSize + TotalElementsCount;

        public bool ModificationFlag { get; set; } = false;
        public bool OpenToWriteFlag { get; set; } = true;
        public int PageUsingCount { get; set; } = 0;
        public int PageInMemoryTime { get; set; } = 0;
        //public int PageIndex { get; private set; }

        public bool[] BitMap { get; }

        public ArrayPage()
        {
            //PageIndex = pageIndex;

            BitMap = new bool[TotalElementsCount];
        }

        public virtual byte[] ToBytes()
        {
            byte[] bytes = new byte[AdditionalFieldsSize];
            int offset = 0;

            Array.Copy(BitConverter.GetBytes(ModificationFlag), 0, bytes, offset, _modificationFlagSize);
            offset += _modificationFlagSize;

            Array.Copy(BitConverter.GetBytes(OpenToWriteFlag), 0, bytes, offset, _openToWriteFlagSize);
            offset += _openToWriteFlagSize;

            Array.Copy(BitConverter.GetBytes(PageUsingCount), 0, bytes, offset, _pageUsingCountSize);
            offset += _pageUsingCountSize;

            Array.Copy(BitConverter.GetBytes(PageInMemoryTime), 0, bytes, offset, _pageInMemoryTimeSize);
            offset += _pageInMemoryTimeSize;

            //Array.Copy(BitConverter.GetBytes(PageIndex), 0, bytes, offset, _pageIndexSize);
            foreach (var item in BitMap)
            {
                Array.Copy(BitConverter.GetBytes(item), 0, bytes, offset, 1);
                offset += 1;
            }

            return bytes;
        }

        public virtual void FromBytes(byte[] bytes, ref int offset)
        {
            ModificationFlag = BitConverter.ToBoolean(bytes, offset);
            offset += _modificationFlagSize;

            OpenToWriteFlag = BitConverter.ToBoolean(bytes, offset);
            offset += _openToWriteFlagSize;

            PageUsingCount = BitConverter.ToInt32(bytes, offset);
            offset += _pageUsingCountSize;

            PageInMemoryTime = BitConverter.ToInt32(bytes, offset);
            offset += _pageInMemoryTimeSize;

            //PageIndex = BitConverter.ToInt32(bytes, offset);
            //offset += _pageIndexSize;
        }
    }

    public class IntArrayPage : ArrayPage
    {
        public static int ElementSize => 4;
        public int[] Elements { get; }

        public IntArrayPage()
        {
            Elements = new int[TotalElementsCount];
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[TotalElementsCount * ElementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(BitConverter.GetBytes(element), 0, bytes, offset, ElementSize);
                offset += ElementSize;
            }

            bytes = base.ToBytes().Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset)
        {
            base.FromBytes(bytes, ref offset);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToInt32(bytes, offset);
                offset += ElementSize;
            }
        }
    }

    public class CharArrayPage : ArrayPage
    {
        public static int ElementSize => 2;
        public char[] Elements { get; }

        public CharArrayPage()
        {
            Elements = new char[TotalElementsCount];
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[TotalElementsCount * ElementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(BitConverter.GetBytes(element), 0, bytes, offset, ElementSize);
                offset += ElementSize;
            }

            bytes = base.ToBytes().Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset)
        {
            base.FromBytes(bytes, ref offset);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = BitConverter.ToChar(bytes, offset);
                offset += ElementSize;
            }
        }
    }

    public class StringArrayPage : ArrayPage
    {
        public static int ElementSize { get; private set; }
        public string[] Elements { get; }

        public StringArrayPage()
        {
            Elements = new string[TotalElementsCount];
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[TotalElementsCount * ElementSize];
            int offset = 0;

            foreach (var element in Elements)
            {
                Array.Copy(Encoding.UTF8.GetBytes(element), 0, bytes, offset, ElementSize);
                offset += ElementSize;
            }

            bytes = base.ToBytes().Concat(bytes).ToArray();

            return bytes;
        }

        public override void FromBytes(byte[] bytes, ref int offset)
        {
            base.FromBytes(bytes, ref offset);
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = Encoding.UTF8.GetString(bytes, offset, ElementSize);
                offset += ElementSize;
            }
        }
    }
}
