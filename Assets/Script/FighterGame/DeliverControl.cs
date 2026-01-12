using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverControl : MonoBehaviour
{
    Transform children;
    GameObject child,player,text;
    bool yes=false;
    public bool NeedPush = true;
    public KeyCode key = KeyCode.E;
    public bool needGravity=true;
    // Start is called before the first frame update
    void Start()
    {
        children = transform.GetChild(0);
        child=children.gameObject;
        children = transform.GetChild(1);
        text = children.gameObject;
        text.SetActive(false);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null&&Vector3.Distance(player.transform.position,transform.position)<1)
        {
            text.SetActive(true);
            if(Input.GetKeyDown(key) ||!NeedPush)
            {
                yes = true;
            }
        }
        else
        {
            yes = false;
            text.SetActive(false);
        }
        child.SetActive(yes);
    }
}
