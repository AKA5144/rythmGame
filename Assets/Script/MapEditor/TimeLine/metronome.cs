using UnityEngine;

public class metronome : MonoBehaviour
{
    public float bpm = 120f; // Peut être modifié en direct
    public AudioClip metronomeTickSound;
    public AudioSource audioSource;

    private float beatInterval;
    private float nextTickTime;

    void Start()
    {
        UpdateInterval();
        nextTickTime = Time.time + beatInterval;
    }

    void Update()
    {
        UpdateInterval();

        if (Time.time >= nextTickTime)
        {
            PlayTick();

            // Avancer le prochain tick avec le nouvel intervalle
            nextTickTime += beatInterval;
        }
    }

    void UpdateInterval()
    {
        beatInterval = 60f / bpm;
    }

    public void PlayTick()
    {
        audioSource.PlayOneShot(metronomeTickSound);
    }
}
