using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public Image fadeScreenFade;
    private void Awake()
    {
        fadeScreenFade = GetComponent<Image>();
    }
    public IEnumerator Fade()
    {
        gameObject.GetComponent<Image>().enabled = true;
        do
        {
            float val = fadeScreenFade.color.a + 6*Time.deltaTime;
            val = Mathf.Clamp(val, 0f,1f);
            fadeScreenFade.color = new Color(0f, 0f, 0f, val);
            yield return null;
        }
        while (fadeScreenFade.color.a != 1f);
    }
    public IEnumerator DeFade()
    {
        do
        {
            float val = fadeScreenFade.color.a - 6*Time.deltaTime;
            val = Mathf.Clamp(val, 0f, 1f);
            fadeScreenFade.color = new Color(0f, 0f, 0f, val);
            yield return null;
        }
        while (fadeScreenFade.color.a != 0f);
        gameObject.GetComponent<Image>().enabled = false;

    }
}
