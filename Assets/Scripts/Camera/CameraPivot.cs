using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, 60f * Time.deltaTime);
    }
}
