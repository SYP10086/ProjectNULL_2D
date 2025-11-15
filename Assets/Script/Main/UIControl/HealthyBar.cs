using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthyBar : MonoBehaviour
{
    Image NowHealthyBar;
    // Start is called before the first frame update
    void Start()
    {
        NowHealthyBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        NowHealthyBar.fillAmount = PlayerMain.nowHealthy / PlayerMain.healthy;
    }
}
