using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    /// <summary>
    /// Generalized constructor for any subclass of Wasteland. (Ezra) This function is a little gross, sorry!
    /// Also, if we want to have tiles be able to add tiles around them we may need to make this more accessible.
    /// </summary>
    /// <param name="TypeToBeCreated">Subclass of Wasteland (e.g. Gargoyle)</param>
    /// <param name="transformParent">To be initialized underneath in the hierarchy</param>
    /// <param name="newPosition">3D position in world space to be initialized</param>
    /// <returns></returns>
    public static ITile CreateTile(System.Type TypeToBeCreated, Transform transformParent, Vector3 newPosition)
    {
        //Enforces that this must be a subclass of Wasteland i.e. an ITile
        if (!TypeToBeCreated.IsSubclassOf(typeof(Wasteland)))
        {
            Debug.Log(TypeToBeCreated.Name + " is not a subclass of wasteland");
            return new Wasteland();
        }

        //Note this gets the first constructor. As long as they have only 1 constructor 
        //  this is fine, but it may break if we add more.
        ConstructorInfo c = TypeToBeCreated.GetConstructors()[0];

        //This seems to be the only way to call it. Passing a list of "objects" which 
        //  are the parameters.
        return (ITile) c.Invoke(new object[] {transformParent, newPosition});
    }
}
