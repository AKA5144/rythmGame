 using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSong : MonoBehaviour
{
    public void LoadSceneWithSong()
    {
        KeepSong.instance.StopAudio();
        KeepSong.instance.SetAudioClip(GetComponent<AudioSource>().clip);
        KeepSong.instance.MusicPlayed = false ;
        string path = GetComponent<SongInfo>().path;
        if (path != null || path!= " ")
        {
            MapReader.mapPath = path;
        }
        else
        {
            path = MapReader.mapPath;
            MapReader.mapPath = path;
        }
        SceneManager.LoadScene("Scenes/Game");
    }

    public void ReturnToMenu()
    {
        KeepSong.instance.StopAudio();
        SceneManager.LoadScene("Scenes/SongSelection");
    }
}
