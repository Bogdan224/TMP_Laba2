namespace TMP_Laba2
{
    public interface ISerializable<T>
    {
        public byte[] ToBytes();
        public void FromBytes(byte[] bytes, ref int offset);
    }

    public abstract class ArrayHeader : ISerializable<ArrayHeader>
    {
        public ArrayPage[] Pages { get; set; }
        
        //public ArrayHeader(long arraySize, int elementSize)
        //{
        //    var tmp = new ArrayPage<T>(elementSize: elementSize, 0);
        //    var pageCount = arraySize / tmp.TotalElementsCount;

        //    Pages = new ArrayPage<T>[pageCount];
        //}

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
