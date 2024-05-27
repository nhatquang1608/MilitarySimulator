using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public enum ObjectType
    {
        Concrete,
        Metal,
        Wood,
        Softbody
    }

    public ObjectType objectType;
}
