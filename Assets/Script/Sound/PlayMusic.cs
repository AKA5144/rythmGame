using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    static public AudioSource audioSource;
    private bool MusicPlayed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }
    public void StartSong()
    {
       /* if (!MusicPlayed)
        {
            audioSource.Play();
            MusicPlayed = true;
        }*/
    }
    // Update is called once per frame
    void Update()
    {

    }
}
