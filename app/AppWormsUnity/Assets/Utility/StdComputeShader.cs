
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Picodex
{
    //public enum StdComputeBufferMode
    //{
    //    Read=1,
    //    Write = 2
    // //   ReadWrite = 3
    //}

    public class InputComputeBuffer
    {
        public int count;
        public Texture2D texture;
        public TextureFormat format;
        public Color[] buffer;
        public InputComputeBuffer(int count, TextureFormat format)
        {
            this.count=count;
            this.format = format;
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
                h = (int)((count / w) );
            }
            texture = new Texture2D(w, h, format, false, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            buffer = new Color[w * h];
        }

        public void Load()
        {
            texture.SetPixels(buffer); // precisione giusta ??
            texture.Apply();
        }

        public virtual void Upload()
        {
            buffer = texture.GetPixels();
        }
    }

    public class OutputComputeBuffer
    {
        public int count;
        public RenderTexture renderTexture;
        InputComputeBuffer input;

        public OutputComputeBuffer(InputComputeBuffer input)
        {
            this.input = input;
            this.count = input.count;

            int w = input.texture.width;
            int h = input.texture.height;

            RenderTextureFormat format = RenderTextureFormat.RFloat;
            if (input.format == TextureFormat.RFloat)
            {
                format = RenderTextureFormat.RFloat;
            }
            else
            {
                format = RenderTextureFormat.ARGBFloat;
            }

            renderTexture = RenderTexture.GetTemporary(w, h, 0, format);
            renderTexture.name = "VXCMBuffer";
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.filterMode = FilterMode.Point;
            
        }

        public virtual void Sync()
        {
            //DepthCamera.depthTextureMode = DepthTextureMode.Depth;
            //DepthCamera.enabled = false;
            // mirror on input text
            RenderTexture old = RenderTexture.active;
            RenderTexture.active = renderTexture;

            // blit on input 
            input.texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            input.texture.Apply();
            RenderTexture.active = old;

            input.Upload();
        }
    }

    // ================================

    public class VectorInputComputeBuffer : InputComputeBuffer
    {
        public VectorInputComputeBuffer(int count) : base(count, TextureFormat.RGBAFloat)
        {
        }
    }

    public class VectorOutputComputeBuffer : OutputComputeBuffer
    {
        public VectorOutputComputeBuffer(InputComputeBuffer input) : base(input)
        {
        }

        public override void Sync()
        {
            base.Sync();
        }

        //public void Load(Vrc[] values)
        //{
        //    Debug.Assert(buffer.Length == values.Length);
        //    buffer = values;
        //    if (mode == StdComputeBufferMode.Read)
        //    {
        //        //   byte[] data = new byte[];
        //        //   Marshal.Copy(values, data,  0, imWidth * imHeight * 3);
        //        int len = values.Length * sizeof(float);
        //        IntPtr unmanagedPointer = Marshal.AllocHGlobal(len);
        //        Marshal.Copy(values, 0, unmanagedPointer, len);
        //        // Call unmanaged code

        //        //texture.LoadRawTextureData(data);
        //        texture.LoadRawTextureData(unmanagedPointer, len);
        //        texture.Apply();

        //        Marshal.FreeHGlobal(unmanagedPointer);

        //    }
        //}
    }

    // =============================================

    public class StdComputeShader
    {
        // esegue
        Material material;

        public Material Material
        {
            get
            {
                return material;
            }
        }

        public StdComputeShader(String shaderName)
        {
            material = new Material(Shader.Find(shaderName));
        }

        public void Execute(InputComputeBuffer input, OutputComputeBuffer output)
        {
            Graphics.Blit(input.texture, output.renderTexture, material);

            output.Sync();
        }

    }
}
