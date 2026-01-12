using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public delegate void MyDel();
public delegate void Myfloat(float a);
public delegate void MyStrFloat(string a, float b);
public delegate void MyInt(int a);
public delegate void MyStr(string a);
public static class Event
{
    public static bool Open=true;
    public static MyDel CleanSpeed;
    public static MyDel SpeedRe;
    public static MyDel Death;
    public static MyDel LocalChange;
    public static MyStrFloat Attack;
    public static Myfloat Hit;
    public static MyDel Start;
    public static MyInt Save;
    public static MyInt Load;
    public static MyStr ItemUse;
}
