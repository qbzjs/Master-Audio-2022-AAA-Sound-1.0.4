using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ITile CreateTile(System.Type t, Transform newTransform, Vector3 newPosition)
    {
        
        if (!t.IsSubclassOf(typeof(Wasteland)))
        {
            Debug.Log(t.Name + " is not a subclass of wasteland");
            return new Wasteland();
        }

        ConstructorInfo c = t.GetConstructors()[0];

        return (ITile) c.Invoke(new object[] {newTransform, newPosition});
    }
}
