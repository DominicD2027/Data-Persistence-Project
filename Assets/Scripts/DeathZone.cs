using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Changed this from MainManager to LevelManager
    public LevelManager Manager;

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);

        if (Manager != null)
        {
            Manager.GameOver();
        }
        else
        {
            LevelManager foundManager = FindObjectOfType<LevelManager>();
            if (foundManager != null)
            {
                foundManager.GameOver();
            }
        }
    }
}
