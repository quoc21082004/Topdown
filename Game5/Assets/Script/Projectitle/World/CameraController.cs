using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public GameObject followTarget;
    private Vector3 targetPos;
    public float moveSpeed;

    private void FixedUpdate()
    {
        if (followTarget == null)
            followTarget = PartyController.player.gameObject.GetComponent<GameObject>();

        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
