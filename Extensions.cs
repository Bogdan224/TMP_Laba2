namespace TMP_Laba2
{
    public static class ArrayTypeExtensions
    {
        public static ArrayType ToArrayType(this string str)
        {
            return str.ToLower() switch
            {
                "int" => ArrayType.Int,
                "char" => ArrayType.Char,
                "string" => ArrayType.String,
                _ => throw new Exception($"Массив с типом \"{str}\" не может быть создан!"),
            };
        }
    }
}
