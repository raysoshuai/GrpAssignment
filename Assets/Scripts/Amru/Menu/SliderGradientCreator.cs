using UnityEngine;
using UnityEngine.UI;

public class SliderGradientCreator : MonoBehaviour
{
    public Image targetImage; // The background or the image to apply the gradient to

    void Start()
    {
        ApplyGradient();
    }

    void ApplyGradient()
    {
        // Create a horizontal gradient
        Texture2D gradientTexture = new Texture2D(360, 1); // Width set to 360 for one pixel per degree
        gradientTexture.wrapMode = TextureWrapMode.Clamp;

        // Set the pixels from hue = 0 to 1
        for (int i = 0; i < gradientTexture.width; i++)
        {
            float hue = i / 360f; // Normalizing the hue value to 0-1
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            gradientTexture.SetPixel(i, 0, color);
        }

        gradientTexture.Apply();

        // Create a sprite from the texture and apply it to the target image
        if (targetImage != null)
        {
            targetImage.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, gradientTexture.width, gradientTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
