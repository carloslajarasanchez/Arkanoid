using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource audioSource;// Reproduce los sonidos de la escena

    public RectTransform screenTransform;// Transform del canvas para hacer animacion por codigo de efecto de apagar la TV
    public CanvasGroup canvasGroup;// Canvas group para poder controlar todos los elementos que hay dentro de este
    public float collapseDuration = 0.3f;// Tiempo de colapso de la animacion
    public float lineDuration = 0.2f;// Duracion de la linea de la animacion 
    public float fadeOutDuration = 0.2f;// Tiempo del Fade

    private Vector3 originalScale;// Escala original del canva

    void Start()
    {
        if (screenTransform == null) screenTransform = GetComponent<RectTransform>();// Cogemos referencia del screen Transform
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();// Cogemos referencias del canvasGroup
        originalScale = screenTransform.localScale;// Recogemos la escala original
    }

    // Reproduce la corrutina con el efecto de apagarTV
    public void PlayShutdownEffect()
    {
        StopAllCoroutines();// Paramos corrutinas para evitar errores
        StartCoroutine(ShutdownSequence());// Iniciamos la corrutina de efecto de apagarTV
    }

    // Corrutina con el efecto de apagar TV
    public IEnumerator ShutdownSequence()
    {
        // Collapse vertically (altura)
        float t = 0f;
        while (t < collapseDuration)
        {
            t += Time.deltaTime;// Aumentamos el tiempo
            float scaleY = Mathf.Lerp(originalScale.y, 0.01f, t / collapseDuration);// Cambiamos la escala 
            screenTransform.localScale = new Vector3(originalScale.x, scaleY, 1f);// Aplicamos el cambio de escala
            yield return null;
            yield return null;
        }

        // Collapse horizontally (anchura de la línea)
        t = 0f;
        while (t < lineDuration)
        {
            t += Time.deltaTime;// Aumentamos el tiempo
            float scaleX = Mathf.Lerp(originalScale.x, 0.01f, t / lineDuration);// Cambiamos la escala 
            screenTransform.localScale = new Vector3(scaleX, 0.01f, 1f);// Aplicamos el cambio de escala
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;// Aumentamos el tiempo
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);// Cambiamos el Alpha del canvasGroup
            yield return null;
        }
        SceneManager.LoadScene("TitleMenu");// Se carga la escena del titulo
        // Deactivate if needed
        gameObject.SetActive(false);
    }



    public void PlaySound()
    {
        audioSource.Play();
    }

    public void StartGame()
    {
        Invoke("PlayShutdownEffect", 3f);
    }
}
