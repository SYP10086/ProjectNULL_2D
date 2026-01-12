using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDistory : MonoBehaviour
{
    Scene scene;
    new public string name= "Start";
    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == name)
            Destroy(this.gameObject);
    }
}
