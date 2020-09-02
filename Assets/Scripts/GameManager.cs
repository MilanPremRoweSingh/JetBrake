using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadNextScene()
    {
        int currSceneBuildIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneBuildIdx+1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextScene();
        }
    }
}
