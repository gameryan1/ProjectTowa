using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shoptower
{
    public string name;
    public int cout;
    public GameObject prefab;

    // Start is called before the first frame update
   public Shoptower (string newname, int newcout, GameObject newprefab)
    {
        name = newname;
        cout = newcout;
        prefab = newprefab;
    }
}
