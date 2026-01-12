using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnd : MonoBehaviour
{
    Transform children;
    GameObject child,player;
    public bool yes = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        children = transform.GetChild(0);
        child = children.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (yes) player.transform.position = transform.position;
        child.SetActive(yes);
    }
}
