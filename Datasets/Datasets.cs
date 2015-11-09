namespace ProceduralToolkit
{
    /// <summary>
    /// Various data
    /// </summary>
    public static partial class Datasets
    {
        /// <summary>
        /// Lowercase letters from a to z
        /// </summary>
        public const string lowercase = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Uppercase letters from A to Z
        /// </summary>
        public const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Digits from zero to nine
        /// </summary>
        public const string digits = "0123456789";

        /// <summary>
        /// The concatenation of the strings <see cref="lowercase"/> and <see cref="uppercase"/>
        /// </summary>
        public const string letters = lowercase + uppercase;

        /// <summary>
        /// The concatenation of the strings <see cref="letters"/> and <see cref="digits"/>
        /// </summary>
        public const string alphanumerics = letters + digits;
    }
}