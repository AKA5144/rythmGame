using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSong : MonoBehaviour
{
    public void LoadSceneWithSong(string path)
    {
        KeepSong.instance.SetAudioClip(GetComponent<AudioSource>().clip);
        KeepSong.instance.MusicPlayed = false ;
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
        SceneManager.LoadScene("Scenes/SongSelection");
    }
}
