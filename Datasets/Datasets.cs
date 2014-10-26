namespace ProceduralToolkit
{
    /// <summary>
    /// Various data
    /// </summary>
    public static class Datasets
    {
        public static readonly string lowercase = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly string digits = "0123456789";
        public static readonly string letters = lowercase + uppercase;
        public static readonly string alphanumerics = letters + digits;
    }
}