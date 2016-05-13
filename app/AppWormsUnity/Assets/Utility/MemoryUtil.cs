using System;
using System.Collections.Generic;

using System.Text;

namespace Picodex
{
    public static class MemoryUtil
    {
        public static void MemSet(byte[] array, byte value) {
          if (array == null) {
            throw new ArgumentNullException("array");
          }
          const int blockSize = 4096; // bigger may be better to a certain extent
          int index = 0;
          int length = System.Math.Min(blockSize, array.Length);
          while (index < length) {
            array[index++] = value;
          }
          length = array.Length;
          while (index < length) {
            Buffer.BlockCopy(array, 0, array, index, System.Math.Min(blockSize, length-index));
            index += blockSize;
          }
        }
        // FASTEST
        public static void MemSetG<T>(T[] array, T elem)
        {
            int length = array.Length;
            if (length == 0) return;
            array[0] = elem;
            int count;
            for (count = 1; count <= length / 2; count *= 2)
                Array.Copy(array, 0, array, count, count);
            Array.Copy(array, 0, array, count, length - count);
        }


    //    static MemoryUtil()
    //    {
    //        var dynamicMethod = new DynamicMethod("Memset", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard,
    //            null, new[] { typeof(IntPtr), typeof(byte), typeof(int) }, typeof(Util), true);

    //        var generator = dynamicMethod.GetILGenerator();
    //        generator.Emit(OpCodes.Ldarg_0);
    //        generator.Emit(OpCodes.Ldarg_1);
    //        generator.Emit(OpCodes.Ldarg_2);
    //        generator.Emit(OpCodes.Initblk);
    //        generator.Emit(OpCodes.Ret);

    //        MemsetDelegate = (Action<IntPtr, byte, int>)dynamicMethod.CreateDelegate(typeof(Action<IntPtr, byte, int>));
    //    }

    //    public static void Memset(byte[] array, byte what, int length)
    //    {
    //        var gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
    //        MemsetDelegate(gcHandle.AddrOfPinnedObject(), what, length);
    //        gcHandle.Free();
    //    }

    //    public static void ForMemset(byte[] array, byte what, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //        {
    //            array[i] = what;
    //        }
    //    }

    //    private static Action<IntPtr, byte, int> MemsetDelegate;

    }

}
