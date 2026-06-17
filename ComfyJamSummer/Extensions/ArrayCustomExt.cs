using System;

namespace ComfyJamSummer.Extensions
{
    public static class ArrayCustomExt
    {
        public static string[] CopyArray(this string[] original, string[] target)
        {
            if (original == null)
            {
                return null;
            }

            target = new string[original.Length];
            original.CopyTo(target, 0);

            return target;
        }
    }
}