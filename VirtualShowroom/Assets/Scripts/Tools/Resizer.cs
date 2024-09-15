using System;
using UnityEngine;

// Reference: https://stackoverflow.com/questions/56949217/how-to-resize-a-texture2d-using-height-and-width
public class Resizer : MonoBehaviour
{
    public Texture2D Resize(Texture2D texture2D, int newWidth, int newHeight)
    {
        RenderTexture rt = new RenderTexture(newWidth, newHeight, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        return result;
    }

    // Reference: https://stackoverflow.com/questions/65023000/how-to-convert-16bit-byte-array-to-audio-clip-data-correctly
    public float[] Convert16BitByteArrayToAudioClipData(byte[] source)
    {
        int s = sizeof(short);  // 2 bytes
        int convertedSize = source.Length / s;
        float[] data = new float[convertedSize];  // 4 bytes
        short maxValue = short.MaxValue;  // 32767

        for (int i = 0; i < convertedSize; i++)
        {
            int offset = i * s;
            data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;

            ++i;
        }

        return data;
    }
}