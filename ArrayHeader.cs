namespace TMP_Laba2
{
    public interface ISerializable<T>
    {
        public byte[] ToBytes();
        public void FromBytes(byte[] bytes, ref int offset);
    }

    public abstract class IntArrayHeader : ISerializable<IntArrayHeader>
    {
        public IntArrayPage[] Pages { get; set; }

        public IntArrayHeader(long arraySize, int elementSize)
        {
            var tmp = new IntArrayPage(elementSize, 0);
            var pageCount = arraySize / tmp.TotalElementsCount;

            Pages = new IntArrayPage[pageCount];
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void FromBytes(byte[] bytes, ref int offset)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CharArrayHeader : ISerializable<CharArrayHeader>
    {
        public CharArrayPage[] Pages { get; set; }

        public CharArrayHeader(long arraySize)
        {
            var tmp = new CharArrayPage(2, 0);
            var pageCount = arraySize / tmp.TotalElementsCount;

            Pages = new CharArrayPage[pageCount];
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void FromBytes(byte[] bytes, ref int offset)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class StringArrayHeader : ISerializable<StringArrayHeader>
    {
        public StringArrayPage[] Pages { get; set; }

        public StringArrayHeader(long arraySize, int elementSize)
        {
            var tmp = new StringArrayPage(elementSize, 0);
            var pageCount = arraySize / tmp.TotalElementsCount;

            Pages = new StringArrayPage[pageCount];
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void FromBytes(byte[] bytes, ref int offset)
        {
            throw new NotImplementedException();
        }
    }

}
