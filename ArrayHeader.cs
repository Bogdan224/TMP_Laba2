namespace TMP_Laba2
{
    public interface IPageSerializable
    {
        public byte[] ToBytes(int elementSize);
        public void FromBytes(byte[] bytes, ref int offset, int elementSize);
    }

    public interface IHeaderSerializable
    {
        public byte[] ToBytes();
        public void FromBytes(byte[] bytes, ref int offset, int pageCount = 3);
    }

    public abstract class ArrayHeader(long arraySize, int elementSize = 0) : IHeaderSerializable
    {
        private const int _elementSize = 4;
        private const int _totalPageElementsSize = 4;
        private const int _arraySize = 8;

        private const int AdditionalFieldsSize = _elementSize + _totalPageElementsSize;

        public long ArraySize { get; private set; } = arraySize;
        public int ElementSize { get; private set; } = elementSize;
        public int TotalPageElementsSize { get; protected set; }

        public virtual byte[] ToBytes()
        {
            byte[] bytes = new byte[AdditionalFieldsSize];
            int offset = 0;

            BitConverter.GetBytes(ElementSize).CopyTo(bytes, offset);
            offset += _elementSize;

            BitConverter.GetBytes(TotalPageElementsSize).CopyTo(bytes, offset);
            offset += _totalPageElementsSize;

            return bytes;
        }

        public virtual void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            ElementSize = BitConverter.ToInt32(bytes, offset);
            offset += _elementSize;

            TotalPageElementsSize = BitConverter.ToInt32(bytes, offset);
            offset += _totalPageElementsSize;
        }
    }

    public enum ArrayType
    {
        Int, Char, String
    }

    public abstract class IntArrayHeader : ArrayHeader
    {
        public List<IntArrayPage> Pages { get; set; }

        public IntArrayHeader(long arraySize) : base(4)
        {
            TotalPageElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            Pages = new();
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

        public override void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            base.FromBytes(bytes, ref offset, pageCount);

            for(int i = 0; i < pageCount; i++)
            {
                var page = new IntArrayPage();
                page.FromBytes(bytes, ref offset, ElementSize);
                Pages.Add(page);
            }
        }
    }

    public abstract class CharArrayHeader : ArrayHeader
    {
        public List<CharArrayPage> Pages { get; set; }

        public CharArrayHeader(long arraySize) : base(2)
        {
            TotalPageElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            Pages = new();
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

        public override void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            base.FromBytes(bytes, ref offset, pageCount);

            for (int i = 0; i < pageCount; i++)
            {
                var page = new CharArrayPage();
                page.FromBytes(bytes, ref offset, ElementSize);
                Pages.Add(page);
            }
        }
    }

    public abstract class StringArrayHeader : ArrayHeader
    {
        public List<StringArrayPage> Pages { get; set; }

        public StringArrayHeader(long arraySize, int charCount) : base(charCount * 2)
        {
            TotalPageElementsSize = (int)Math.Ceiling((double)(ArrayPage.TotalElementsCount * ElementSize) / 512) * 512;

            Pages = new();
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

        public override void FromBytes(byte[] bytes, ref int offset, int pageCount = 3)
        {
            base.FromBytes(bytes, ref offset, pageCount);

            for (int i = 0; i < pageCount; i++)
            {
                var page = new StringArrayPage();
                page.FromBytes(bytes, ref offset, ElementSize);
                Pages.Add(page);
            }
        }
    }
}
