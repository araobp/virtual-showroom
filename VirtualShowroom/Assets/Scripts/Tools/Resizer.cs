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
}