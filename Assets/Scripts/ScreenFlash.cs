using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public float FlashDuration = 0.33f;
    private Image flashImage;
    private Color imageColor;

    void Start()
    {
        flashImage = GetComponent<Image>();
        imageColor = flashImage.color;
    }

    public IEnumerator FlashRoutine()
    {
        float timer = 0f;
        float t = 0f;
        float alphaFrom = 1f;
        float alphaTo = 0;

        while (t < 1f)
        {
            timer += Time.deltaTime;
            t = Mathf.Clamp01(timer / FlashDuration);
            float alpha = Mathf.Lerp(alphaFrom, alphaTo, t);
            Color col = imageColor;
            col.a = alpha;
            flashImage.color = col;
            yield return new WaitForEndOfFrame();
        }

    }

    private void Update()
    {
        
    }

}
