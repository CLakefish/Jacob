using System.Collections;
using System.Collections.Generic;
using UnityEngine; using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
    public void ExitGame() => Application.Quit();
}
