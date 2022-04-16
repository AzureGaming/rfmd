using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    Imp impRef;

    public void SetImp(Imp imp)
    {
        impRef = imp;
    }

    public void CheckHit()
    {
        impRef.CheckHit();
    }

    public void CompleteAttack()
    {
        impRef.CompleteAttack();
        Destroy(gameObject);
    }
}
