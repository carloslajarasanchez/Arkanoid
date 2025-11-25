using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NaveController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _winCanvasGroup;
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private AudioClip _winMusic;
    [SerializeField] private AudioClip _naveFX;
    [SerializeField] private AudioClip _naveButtonFX;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(_winMusic);
    }

    public IEnumerator Fade(CanvasGroup canvasGroup, float target, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, target, time / duration);
            yield return null;
        }

        canvasGroup.alpha = target;
        canvasGroup.blocksRaycasts = target > 0.99f;
        canvasGroup.interactable = target > 0.99f;
    }

    public void StartWinFade()
    {
        AudioManager.Instance.PlaySound(_naveFX);
        StartCoroutine(Fade(_winCanvasGroup,1,1));
    }

    public void StarButtonsFade()
    {
        AudioManager.Instance.PlaySound(_naveButtonFX);
        StartCoroutine(Fade(_buttonsCanvasGroup, 1, 1));
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }
}
