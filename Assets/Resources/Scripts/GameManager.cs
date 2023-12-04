using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 playerPosition;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
