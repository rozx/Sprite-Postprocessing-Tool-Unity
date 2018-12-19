using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Utils/Sprite Postrocessing utility")]

public class SpritePostprocessing : MonoBehaviour {

    [Header("Source Settings")]
    public Image targetUIImage;
    public SpriteRenderer targetSpriteRenderer;

    [Header("Main Settings")]

    public List<Color> ignoredColors = new List<Color>();
    public float ignoreThreshold = 0.01f;
    public float colorMultiplier = 1;
    public PostProcessingMethod imagePostProcessingEffect = PostProcessingMethod.GreyScale;


    [Header("Tint Settings")]

    public Color tintColor = Color.white;

    private Sprite targetSprite;
    private Texture2D targetTexture;
    private Sprite tempSprite;
    private Texture2D tempTexture;

    public enum PostProcessingMethod { None ,GreyScale, Exposure, Tint, Inverse, Noise }

    private void OnEnable()
    {

        if (targetUIImage == null) targetUIImage = GetComponent<Image>();
        if (targetSpriteRenderer == null) targetSpriteRenderer = GetComponent<SpriteRenderer>();


        // assign sprites
        if (targetUIImage) targetSprite = targetUIImage.sprite;
        if (targetSpriteRenderer && !targetSprite) targetSprite = targetSpriteRenderer.sprite;


        if (targetSprite)
        {
            targetTexture = targetSprite.texture;
            if (targetTexture) ApplyTextureChanges();

        }
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Reset()
    {
        if(targetUIImage) targetUIImage.sprite = targetSprite;
        if(targetSpriteRenderer) targetSpriteRenderer.sprite = targetSprite;

        tempSprite = null;
        tempTexture = null;
    }

    public void ApplyTextureChanges()
    {


        if (tempTexture != null)
        {
            tempTexture.SetPixels(targetSprite.texture.GetPixels());
            ProcessingAllPixels(tempTexture);

        }
        else
        {
            CreateTempSpriteAndTexture();
        }


    }


    private void CreateTempSpriteAndTexture()
    {
        tempTexture = new Texture2D(targetTexture.width, targetTexture.height);
        tempTexture.SetPixels(targetTexture.GetPixels());

        ProcessingAllPixels(tempTexture);

        tempSprite = Sprite.Create(tempTexture, new Rect(0, 0, targetTexture.width, targetTexture.height), new Vector2(targetTexture.width / 2, targetTexture.height / 2));

        if(targetUIImage) targetUIImage.sprite = tempSprite;
        if(targetSpriteRenderer) targetSpriteRenderer.sprite = tempSprite;
    }

    private void ProcessingAllPixels(Texture2D texture)
    {

        switch (imagePostProcessingEffect)
        {
            default:

                Color[] colors = texture.GetPixels();

                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = ProcessPixel(colors[i]);
                }

                texture.SetPixels(colors);

                break;
        }



        texture.Apply();
    }

    private Color ProcessPixel(Color pixel)
    {

        bool isSimilarColor = false;

        isSimilarColor = !ignoredColors.TrueForAll(x => !IsColorSimilar(x, pixel, ignoreThreshold));

        if (!isSimilarColor)
        {

            switch (imagePostProcessingEffect)
            {

                case PostProcessingMethod.GreyScale:

                    float grey = (pixel.r + pixel.g + pixel.b) / 3 * colorMultiplier;
                    pixel = new Color(grey, grey, grey);

                    break;

                case PostProcessingMethod.Exposure:

                    pixel = new Color(pixel.r * colorMultiplier, pixel.g * colorMultiplier, pixel.b * colorMultiplier);

                    break;

                case PostProcessingMethod.Tint:

                    pixel = new Color((pixel.r * tintColor.r) * colorMultiplier, (pixel.g * tintColor.g) * colorMultiplier, (pixel.b * tintColor.b) * colorMultiplier);

                    break;

                case PostProcessingMethod.Inverse:

                    pixel = new Color((1 - pixel.r) * colorMultiplier, (1 - pixel.g) * colorMultiplier, (1 - pixel.b) * colorMultiplier);

                    break;

                case PostProcessingMethod.Noise:

                    pixel = new Color((pixel.r * Random.Range(0, colorMultiplier)), (pixel.g * Random.Range(0, colorMultiplier)), (pixel.b * Random.Range(0, colorMultiplier)));

                    break;
            }
        }

        return pixel;
    }

    /// <summary>
    /// Check the color is similar within the threshold.
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    private bool IsColorSimilar(Color color1, Color color2, float threshold = 0.1f)
    {
        float r = Mathf.Abs(color1.r - color2.r);
        float g = Mathf.Abs(color1.g - color2.g);
        float b = Mathf.Abs(color1.b - color2.b);

        return (r * r + g * g + b * b) <= threshold * threshold;

        //return Vector3.Distance(new Vector3(color1.r, color1.g, color1.b), new Vector3(color2.r, color2.g, color2.b)) <= threshold;
    }

}
