using UnityEngine;

public static class TextureUtility
{
    public static Texture2D SetAlphaTexture(Texture2D texture, byte alpha = 0)
    {
        Texture2D invertedTexture = new Texture2D(texture.width, texture.height);

        Color32[] original = texture.GetPixels32();
        Color32[] inverted = new Color32[original.Length];

        for (int i = 0; i < original.Length; i++)
        {
            inverted[i] = original[i];
            inverted[i].a = alpha;
        }

        invertedTexture.SetPixels32(inverted);
        invertedTexture.Apply();

        return invertedTexture;
    }

    public static Texture2D InvertTextureAlpha(Texture2D texture)
    {
        Texture2D invertedTexture = new Texture2D(texture.width, texture.height);

        Color32[] original = texture.GetPixels32();
        Color32[] inverted = new Color32[original.Length];

        for (int i = 0; i < original.Length; i++)
        {
            inverted[i] = original[i];
            if (inverted[i].a == 255)
            {
                inverted[i].a = 0;
                continue;
            }
            inverted[i].a = 255;
        }
        invertedTexture.SetPixels32(inverted);
        invertedTexture.Apply();

        return invertedTexture;
    }

    public static Texture2D RotateTexture(Texture2D texture, bool clockwise)
    {
        Color32[] original = texture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];

        int rotadedIndex;
        int originalIndex;

        for (int j = 0; j < texture.height; ++j)
        {
            for (int i = 0; i < texture.width; ++i)
            {
                rotadedIndex = (i + 1) * texture.height - j - 1;
                originalIndex = clockwise ? original.Length - 1 - (j * texture.width + i) : j * texture.width + i;
                rotated[rotadedIndex] = original[originalIndex];
            }
        }

        Texture2D rotatedTexture = new Texture2D(texture.height, texture.width);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();

        return rotatedTexture;
    }

    public static Texture2D FlipTextureVertically(Texture2D texture)
    {
        Color32[] original = texture.GetPixels32();
        Color32[] flipped = new Color32[original.Length];

        for (var i = 0; i < texture.width; i++)
        {
            for (var j = 0; j < texture.height; j++)
            {
                flipped[i + j * texture.width] = original[i + (texture.height - j - 1) * texture.width];
            }
        }

        Texture2D flippedTexture = new Texture2D(texture.width, texture.height);
        flippedTexture.SetPixels32(flipped);
        flippedTexture.Apply();

        return flippedTexture;
    }

    public static Texture2D FlipTextureHorizontally(Texture2D texture)
    {
        Color32[] original = texture.GetPixels32();
        Color32[] flipped = new Color32[original.Length];

        for (var i = 0; i < texture.width; i++)
        {
            for (var j = 0; j < texture.height; j++)
            {
                flipped[i + j * texture.width] = original[(texture.width - i - 1) + j * texture.width];
            }
        }

        Texture2D flippedTexture = new Texture2D(texture.width, texture.height);
        flippedTexture.SetPixels32(flipped);
        flippedTexture.Apply();

        return flippedTexture;
    }
}
