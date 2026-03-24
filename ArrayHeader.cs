using System.Collections;

namespace TMP_Laba2
{
    public interface IPageSerializable
    {
        public byte[] ToBytes(int elementSize);
    }

    public interface IHeaderSerializable
    {
        public byte[] ToBytes();
    }

    public abstract class ArrayHeader : IHeaderSerializable
    {
        private const int _elementSize = 4;
        private const int _totalPageElementsSize = 4;
        private const int _arraySize = 8;
        private const int _arrayTypeSize = 1;

        protected IList pages = null!;

        public ArrayType ArrayType { get; private set; }

        public int AdditionalFieldsSize => _elementSize
           + _totalPageElementsSize + _arraySize + _arrayTypeSize; // AdditionalFieldsSize = 19

        public long ArraySize { get; private set; }
        public long PageCount { get; private set; }
        public int TotalPageElementsSize { get; protected set; }
        public int ElementSize { get; private set; }

        public IList Pages => pages;

        public ArrayHeader(ArrayType arrayType, long arraySize, int elementSize)
        {
            ArrayType = arrayType;
            ArraySize = arraySize;
            ElementSize = elementSize;

            PageCount = ArraySize / ArrayPage.TotalElementsCount;
        }

        public ArrayHeader(byte[] bytes, long arraySize, ref int offset)
        {
            ArraySize = arraySize;
            PageCount = ArraySize / ArrayPage.TotalElementsCount;

            FromBytes(bytes, ref offset);
        }

        public virtual byte[] ToBytes()
        {
            byte[] bytes = new byte[AdditionalFieldsSize];
            int offset = 0;

            BitConverter.GetBytes(ElementSize).CopyTo(bytes, offset);
            offset += _elementSize;

            BitConverter.GetBytes(TotalPageElementsSize).CopyTo(bytes, offset);
            offset += _totalPageElementsSize;

            BitConverter.GetBytes(ArraySize).CopyTo(bytes, offset);
            offset += _arraySize;

            bytes[offset] = (byte)ArrayType;
            offset += _arrayTypeSize;

            return bytes;
        }

        private void FromBytes(byte[] bytes, ref int offset)
        {
            ElementSize = BitConverter.ToInt32(bytes, offset);
            offset += _elementSize;

            TotalPageElementsSize = BitConverter.ToInt32(bytes, offset);
            offset += _totalPageElementsSize;

            ArraySize = BitConverter.ToInt64(bytes, offset);
            offset += _arraySize;

            ArrayType = (ArrayType)bytes[offset];
            offset += _arrayTypeSize;
        }
    }

    public enum ArrayType : byte
    {
        Int, Char, String
    }

    public class IntArrayHeader : ArrayHeader
    {
        public new List<IntArrayPage> Pages => (List<IntArrayPage>)pages;

        public IntArrayHeader(long arraySize) : base(ArrayType.Int, arraySize, 4)
        {
            TotalPageElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            pages = new List<IntArrayPage>();
        }

        public IntArrayHeader(long arraySize, byte[] bytes, ref int offset, int pageCount = 3) : base(bytes, arraySize, ref offset)
        {
            TotalPageElementsSize = 512;

            pages = new List<IntArrayPage>();

            FromBytes(bytes, ref offset, pageCount);
        }

        public override byte[] ToBytes()
        {
            var bytes = new byte[Pages.Count * TotalPageElementsSize];
            int offset = 0;

            foreach (var item in Pages)
            {
                item.ToBytes().CopyTo(bytes, offset);
                offset += TotalPageElementsSize;
            }

            return [.. base.ToBytes(), .. bytes];
        }

        private void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            for(int i = 0; i < pageCount; i++)
            {
                var page = new IntArrayPage(bytes, ref offset, ElementSize);
                Pages.Add(page);
            }
        }
    }

    public class CharArrayHeader : ArrayHeader
    {
        public new List<CharArrayPage> Pages => (List<CharArrayPage>)pages;

        public CharArrayHeader(long arraySize) : base(ArrayType.Char, arraySize, 2)
        {
            TotalPageElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            pages = new List<CharArrayPage>();
        }

        public CharArrayHeader(long arraySize, byte[] bytes, ref int offset, int pageCount = 3) : base(bytes, arraySize, ref offset)
        {
            TotalPageElementsSize = 512;

            pages = new List<CharArrayPage>();

            FromBytes(bytes, ref offset, pageCount);
        }

        public override byte[] ToBytes()
        {
            var bytes = new byte[Pages.Count * TotalPageElementsSize];
            int offset = 0;

            foreach (var item in Pages)
            {
                item.ToBytes().CopyTo(bytes, offset);
                offset += TotalPageElementsSize;
            }

            return [.. base.ToBytes(), .. bytes];
        }

        private void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            for (int i = 0; i < pageCount; i++)
            {
                var page = new CharArrayPage(bytes, ref offset);
                Pages.Add(page);
            }
        }
    }

    public class StringArrayHeader : ArrayHeader
    {
        public new List<StringArrayPage> Pages => (List<StringArrayPage>)pages;

        public StringArrayHeader(long arraySize, int charCount) : base(ArrayType.String, arraySize, charCount)
        {
            TotalPageElementsSize = (int)Math.Ceiling((double)(ArrayPage.TotalElementsCount * ElementSize) / 512) * 512;

            pages = new List<StringArrayPage>();
        }

        public StringArrayHeader(long arraySize, byte[] bytes, ref int offset, int pageCount = 3) : base(bytes, arraySize, ref offset)
        {
            TotalPageElementsSize = (int)Math.Ceiling((double)(ArrayPage.TotalElementsCount * ElementSize) / 512) * 512;

            pages = new List<StringArrayPage>();

            FromBytes(bytes, ref offset, pageCount);
        }

        public override byte[] ToBytes()
        {
            var bytes = new byte[Pages.Count * TotalPageElementsSize];
            int offset = 0;

            foreach (var item in Pages)
            {
                item.ToBytes(ElementSize).CopyTo(bytes, offset);
                offset += TotalPageElementsSize;
            }

            return [.. base.ToBytes(), .. bytes];
        }

        private void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            for (int i = 0; i < pageCount; i++)
            {
                var page = new StringArrayPage(bytes, ref offset, ElementSize);
                Pages.Add(page);
            }
        }
    }
}
