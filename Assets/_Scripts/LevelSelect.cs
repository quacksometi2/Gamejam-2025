using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public string LevelToLoad;
    public TextMeshPro LevelText;

    void Start()
    {
        LevelText.text = LevelToLoad;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(LevelToLoad);
        }
    }
}
