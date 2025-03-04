using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSits : MonoBehaviour
{

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isSitting", true);
    }
}
