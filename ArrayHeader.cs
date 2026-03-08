namespace TMP_Laba2
{
    public class ArrayHeader<T>
    {
        public ArrayPage<T>[] Pages { get; set; }
        
        public ArrayHeader(long arraySize) 
        {
            Pages = new ArrayPage<T>[arraySize];
        }
    }

    public class StringArrayHeader : ArrayHeader<string>
    {
        private int _stringSize;
        public StringArrayHeader(long arraySize, int stringSize) : base(arraySize)
        {
            _stringSize = stringSize;
        }
    }
}
