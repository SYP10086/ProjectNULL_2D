using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlace : MonoBehaviour
{
    GameObject player,SaveUI;
    // Start is called before the first frame update
    void Start()
    {
        SaveUI = GameObject.Find("UI/Menu/Save");
        player = GameObject.Find("Player/Foot");
    }

    // Update is called once per frame
    void Update()
    {
        double dis=0;
        if(player!=null)
        dis = Vector3.Distance(player.transform.position, transform.position);
        if (SaveUI != null)
            if(dis<=1) SaveUI.SetActive(true);
            else SaveUI.SetActive(false);
    }
}
