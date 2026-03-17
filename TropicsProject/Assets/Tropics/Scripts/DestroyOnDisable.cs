using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDisable : MonoBehaviour
{
    // When this object is disabled, destroy it.
    void OnDisable()
    {
        GameObject.Destroy(gameObject);
    }
}