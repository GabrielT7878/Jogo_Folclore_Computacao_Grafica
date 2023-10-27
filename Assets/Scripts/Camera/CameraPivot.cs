using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    public Transform player;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, 5f);
    }
}
