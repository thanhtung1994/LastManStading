using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFacingAtTarget : MonoBehaviour
{
    //[SerializeField]
    public Transform Source;

    private SpriteRenderer spriteRenderer
    {
        get { return GetComponent<SpriteRenderer>(); }
    }
    private SkeletonAnimator skeletonAnimator
    {
        get { return GetComponent<SkeletonAnimator>(); }
    }

    public void SetFacingTarGet(Vector3 pos)
    {
        bool flipX = pos.x < Source.position.x;
        Vector3 relativePos = pos - Source.position;
        //float angle = Mathf.Atan2(relativePos.y, flipX ? -1 : 1 * relativePos.x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(relativePos.y, flipX ? -1 : 1 * relativePos.x) * Mathf.Rad2Deg;
        //Source.localRotation = Quaternion.Euler(new Vector3(0, flipX ? 180 : 0, angle));
        Source.localRotation = Quaternion.Euler(new Vector3(0, flipX ? 180 : 0, flipX ? 180 - angle : angle));
    }

}
