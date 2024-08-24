using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerDirection : MonoBehaviour
{
    public void TurnTowardsTarget(GameObject target)
    {
        StartCoroutine(SmoothLookAt(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 0.2f));
    }

    IEnumerator SmoothLookAt(Quaternion originalRotation, Quaternion
            finalRotation, float duration)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                transform.rotation = Quaternion.Slerp(originalRotation,
                finalRotation, progress);
                yield return null;
            }
        }
        transform.rotation = finalRotation;
    }
}
