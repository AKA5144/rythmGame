using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Scrollbar scrollbar;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI percentageText;
    private bool isDragging = false;
    private bool wasPlayingBeforeDrag = false;
    private bool isInitialized = false;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.Pause();
            isInitialized = true;
        }
    }

    private void Update()
    {
        if (audioSource.clip != null)
        {
            UpdateTimeText();
            UpdatePercentageText();

            if (!isDragging)
            {
                scrollbar.SetValueWithoutNotify(audioSource.time / audioSource.clip.length);
            }
        }
    }

    public void OnScrollbarValueChanged(float value)
    {
        if (audioSource.clip != null)
        {
            if (!isInitialized)
            {
                audioSource.Play();
                audioSource.Pause();
                isInitialized = true;
            }

            // 🔥 Sécurité : Empêcher les erreurs quand la barre est à 100%
            float newTime = value * audioSource.clip.length;
            audioSource.time = Mathf.Clamp(newTime, 0f, audioSource.clip.length - 0.01f);

            UpdateTimeText();
            UpdatePercentageText();
        }
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(audioSource.time / 60);
        int seconds = Mathf.FloorToInt(audioSource.time % 60);
        int milliseconds = Mathf.FloorToInt((audioSource.time * 100) % 100);

        int totalMinutes = Mathf.FloorToInt(audioSource.clip.length / 60);
        int totalSeconds = Mathf.FloorToInt(audioSource.clip.length % 60);
        int totalMilliseconds = Mathf.FloorToInt((audioSource.clip.length * 100) % 100);

        timeText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2} / {totalMinutes:D2}:{totalSeconds:D2}:{totalMilliseconds:D2}";
    }

    private void UpdatePercentageText()
    {
        float progress = (audioSource.time / audioSource.clip.length) * 100f;
        percentageText.text = $"{progress:F1}%";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        wasPlayingBeforeDrag = audioSource.isPlaying;
        audioSource.Pause();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (wasPlayingBeforeDrag)
        {
            audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
    }
}
