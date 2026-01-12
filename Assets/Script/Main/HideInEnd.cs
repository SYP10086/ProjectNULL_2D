using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HideInEnd : MonoBehaviour
{
    Scene scene;
    private void Awake()
    {
        scene = SceneManager.GetActiveScene(); 
    }
    // Update is called once per frame
    void Update()
    {
        if(scene.name=="FoxEnd"|| scene.name == "TreeEnd")
            this.gameObject.SetActive(false);
    }
}
