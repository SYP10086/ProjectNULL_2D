using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    GameObject[] dropObject = new GameObject[15];
    GameObject[] monster = new GameObject[15];
    bool[] bools = new bool[15];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            dropObject[i] = GameObject.Find($"AllDrop/{i}");
            dropObject[i].SetActive( false );
        }
        for (int i = 0; i < 15; i++)
        {
            monster[i] = GameObject.Find($"AllMonster/Monster{i}");
        }
        for (int i = 0; i < 15; i++)
        {
            bools[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int n=0;n<15;n++)
        {
            GameObject a=monster[n];
            if (a==null)continue;
            if(!a.activeSelf&&!bools[n])
            {
                bools[n]=true;
                GameObject b=null;
                for (int j = 0; j <= 10; j++)
                {
                    int i = (int)(Random.value * 100) % 20;
                    if(i>14)continue;
                    if (dropObject[i] != null&&!dropObject[i].activeSelf )
                    {
                        b=dropObject[i];
                        dropObject[i] = null;
                        break;
                    }
                }
                if ( b!= null)
                {
                    b.transform.position = a.transform.position;
                    b.SetActive(true);
                }
            }
        }
    }
}
