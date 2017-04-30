using System;

namespace BLL
{
    public static class Guard
    {
        public static void ArgumentNotNull(object obj, string message = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void ArgumentNotWhiteSpaceOrNull(string strArg, string message = "")
        {
            if (string.IsNullOrWhiteSpace(strArg))
            {
                throw new ArgumentException(message);
            }
        }
    }
}