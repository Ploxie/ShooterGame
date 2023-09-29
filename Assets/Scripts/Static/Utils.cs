using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static double GetUnixMillis()
    {
        //Hate how long this name is //Hampus
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
