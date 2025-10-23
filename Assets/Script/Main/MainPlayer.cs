using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{

    public static float Healthy = 1;
    public static float Stamina = 1;
    public static new Transform transform;
    public static float x, y;
    Scene scene;
    //Save
    public bool InitLocation=false;
    public static bool initLocation=false;
    //
    static Rigidbody2D rb;
    public static float  speed = 10;
    Vector2 speedOfPlayer;
    
    // Start is called before the first frame update
    //
    void SpeedClear()
    {
        speedOfPlayer = rb.velocity;
        speed = 0;
    }
    void SpeedRe()
    {
        speed = 10;
        rb.velocity = speedOfPlayer;
    }
    void Start()
    {
        transform =GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        Event.CleanSpeed += new MyDel(SpeedClear);
        Event.SpeedRe += new MyDel(SpeedRe);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!InitLocation||!initLocation)
        {
            transform.position=new Vector2 (x,y);
            InitLocation = true;
            initLocation = true;
        }
        rb.velocity=new Vector2 (Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        ChangeSence();
    }
    void ChangeSence()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex+1);
        }
    }
    

}
