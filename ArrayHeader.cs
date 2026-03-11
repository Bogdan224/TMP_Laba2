namespace TMP_Laba2
{
    public interface ISerializable
    {
        public byte[] ToBytes();
        public void FromBytes(byte[] bytes, ref int offset);
    }

    public abstract class ArrayHeader(int elementSize) : ISerializable
    {
        public int ElementSize => elementSize;
        public int TotalElementsSize { get; protected set; }

        public abstract ArrayPage[] Pages { get; set; }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void FromBytes(byte[] bytes, ref int offset)
        {
            throw new NotImplementedException();
        }
    }

    public enum ArrayType
    {
        Int, Char, String
    }

    public abstract class IntArrayHeader : ArrayHeader
    {
        public IntArrayHeader(long arraySize) : base(4)
        {
            TotalElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            Pages = new IntArrayPage[pageCount];
        }
    }

    public abstract class CharArrayHeader : ArrayHeader
    {
        public CharArrayHeader(long arraySize) : base(4)
        {
            TotalElementsSize = 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            Pages = new IntArrayPage[pageCount];
        }
    }

    public abstract class StringArrayHeader : ArrayHeader
    {
        public StringArrayHeader(long arraySize, int charCount) : base(charCount * 2)
        {
            TotalElementsSize = (int)Math.Ceiling((double)(ArrayPage.TotalElementsCount * ElementSize) / 512) * 512;
            var pageCount = arraySize / ArrayPage.TotalElementsCount;

            Pages = new StringArrayPage[pageCount];
        }
    }

}
