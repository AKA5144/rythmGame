using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarManager : MonoBehaviour
{
    public Scrollbar scrollbar;
    public PlayerSongManager player;
    public SongInfoEditor info;
    public TextMeshProUGUI text; 

    private bool isDragging = false;

    void Update()
    {
        if (!isDragging)
        {
            if (player.isPlaying)
            {
                text.text = $"{info.percent:F1}%";
                scrollbar.value = info.percent * 0.01f;
            }
        }
    }

    public void StartDrag()
    {
        isDragging = true;
    }
    public void EndDrag()
    {
        isDragging = false;
        float newProgress = scrollbar.value * 100f;
        info.setProgress(newProgress);
        info.AudioSource.time = newProgress * info.clipLength * 0.01f;
    }

    public void OnScrollbarValueChanged()
    {
        if (isDragging)
        {
            float newProgress = scrollbar.value * 100f;
            text.text = $"{newProgress:F1}%";
        }
    }
}
