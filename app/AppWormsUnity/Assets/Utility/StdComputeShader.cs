
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Picodex
{
    public enum StdComputeBufferMode
    {
        Read=1,
        Write = 2
     //   ReadWrite = 3
    }

    public class StdComputeBuffer<T>
    {
        public int count;
        public T[] buffer;
        public Texture2D texture;
        public RenderTexture renderTexture;
        public StdComputeBufferMode mode = StdComputeBufferMode.Read;

        public StdComputeBuffer(int count, StdComputeBufferMode mode)
        {
            this.count=count;
            this.mode = mode;
            int w = 0;
            int h =0;
            if (count < 1024)
            {
                w = (int)MathUtility.NextPowerOfTwo((uint)count);
                h = 1;
            }
            else
            {
                w = 1024;
                h = (int)((count / w) + 1);
            }
            if (mode == StdComputeBufferMode.Read)
            {
                TextureFormat format = TextureFormat.RFloat;
                if (typeof(T) == typeof(float[]))
                {
                    format = TextureFormat.RFloat;
                }
                else 
                {
                    format = TextureFormat.RGBAFloat;
                }

                texture = new Texture2D(w, h, format, false, false);
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.filterMode = FilterMode.Point;
            }
            else
            {
                RenderTextureFormat format = RenderTextureFormat.RFloat;
                if (typeof(T) == typeof(float[]))
                {
                    format = RenderTextureFormat.RFloat;
                }
                else 
                {
                    format = RenderTextureFormat.ARGBFloat;
                }


                renderTexture = RenderTexture.GetTemporary(w,h, 0, format);
                renderTexture.name = "VXCMBuffer";
                renderTexture.wrapMode = TextureWrapMode.Clamp;
                renderTexture.filterMode = FilterMode.Point;
            }
        }

     
        
    }
    public class FloatComputeBuffer : StdComputeBuffer<float>
    {
        public FloatComputeBuffer(int count, StdComputeBufferMode mode) : base(count,mode)
        {
        }

        public void Load(float[] values)
        {
            Debug.Assert(buffer.Length == values.Length);
            buffer = values;
            if (mode == StdComputeBufferMode.Read)
            {
                //   byte[] data = new byte[];
                //   Marshal.Copy(values, data,  0, imWidth * imHeight * 3);
                int len = values.Length * sizeof(float);
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(len);
                Marshal.Copy(values, 0, unmanagedPointer, len);
                // Call unmanaged code

                //texture.LoadRawTextureData(data);
                texture.LoadRawTextureData(unmanagedPointer, len);
                texture.Apply();

                Marshal.FreeHGlobal(unmanagedPointer);

            }
        }
    }

    // =============================================

    public class StdComputeShader
    {
        // esegue
      
        public void Dispatch()
        {
        }

        public void SetBuffer<T>( string name, StdComputeBuffer<T> buffer)
        {
        }
    }
}
