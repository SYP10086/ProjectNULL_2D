using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar1 : MonoBehaviour
{
    Image NowStaminaBar;
    // Start is called before the first frame update
    void Start()
    {
        NowStaminaBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        NowStaminaBar.fillAmount = PlayerMain.nowStamina / PlayerMain.stamina;
    }
}
