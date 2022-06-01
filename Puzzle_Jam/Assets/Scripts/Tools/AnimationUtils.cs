using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AnimationUtils
{
    // Helpers
    public static IEnumerator FadeImgIn(Image img, float speed)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        while (img.color.a < 1.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime / speed));
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
    }

    public static IEnumerator FadeImgOut(Image img, float speed)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        while (img.color.a > 0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime / speed));
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }

    public static IEnumerator FadeTextIn(Text txt, float speed)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / speed));
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1.0f);
    }

    public static IEnumerator FadeTextOut(Text txt, float speed)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        while (txt.color.a > 0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime / speed));
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
    }
    // Helpers
}
