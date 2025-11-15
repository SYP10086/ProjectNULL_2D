using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyDel();
public delegate void Myfloat(float a);
public delegate void MyStrFloat(string a,float b);
public static class Event
{
    public static MyDel CleanSpeed;
    public static MyDel SpeedRe;
    public static MyDel Init;
    public static Myfloat Hit;
    public static MyDel Death;
    public static MyDel LocalChange;
    public static MyStrFloat Attack;

}
