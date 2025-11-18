using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource audioSource;

    public RectTransform screenTransform;
    public CanvasGroup canvasGroup;
    public float collapseDuration = 0.3f;
    public float lineDuration = 0.2f;
    public float fadeOutDuration = 0.2f;

    public AudioSource turnPageAudio;
    public AudioSource openBagAudio;

    private Vector3 originalScale;

    void Start()
    {
        if (screenTransform == null) screenTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        originalScale = screenTransform.localScale;
    }

    public void PlayShutdownEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ShutdownSequence());
    }

    public IEnumerator ShutdownSequence()
    {
        // Collapse vertically (altura)
        float t = 0f;
        while (t < collapseDuration)
        {
            t += Time.deltaTime;
            float scaleY = Mathf.Lerp(originalScale.y, 0.01f, t / collapseDuration);
            screenTransform.localScale = new Vector3(originalScale.x, scaleY, 1f);
            yield return null;
        }

        // Collapse horizontally (anchura de la línea)
        t = 0f;
        while (t < lineDuration)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Lerp(originalScale.x, 0.01f, t / lineDuration);
            screenTransform.localScale = new Vector3(scaleX, 0.01f, 1f);
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }
        SceneManager.LoadScene(1);
        // Deactivate if needed
        gameObject.SetActive(false);
    }



    public void PlaySound()
    {
        audioSource.Play();
    }

    public void TurnPageAudio()
    {
        turnPageAudio.Play();
    }

    public void OpenBagAudio()
    {
        openBagAudio.Play();
    }

    public void StartGame()
    {
        Invoke("PlayShutdownEffect", 3f);
    }
}
