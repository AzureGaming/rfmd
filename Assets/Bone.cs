using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    private void Start()
    {
        GetComponent<GoToPosition>().endPos = GameObject.FindGameObjectWithTag("BoneTarget").transform.position;
    }

    void Update()
    {
        if (transform.position == GameObject.FindGameObjectWithTag("BoneTarget").transform.position)
        {
            GetComponentInParent<HotSkull>().CheckHit();
            GetComponentInParent<HotSkull>().CompleteAttack();
            Destroy(gameObject);
        }
    }
}
