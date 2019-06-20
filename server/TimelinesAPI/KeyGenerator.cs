using System;

namespace TimelinesAPI
{
    public class KeyGenerator
    {
        public static string NewKey()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
    }
}
