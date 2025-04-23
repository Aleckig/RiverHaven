using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform homeTarget;
    [SerializeField] private Transform newTarget;
    [SerializeField] private Transform newTarget2;
    [SerializeField] private Transform newTarget3;
    [SerializeField] private Transform newTarget4;
    [SerializeField] private Transform newTarget5;
    private Vector3 targetRotation;

    void Start()
    {
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }
    void Update()
    {
        transform.LookAt(targetRotation);
    }

    public void MakeHomeTarget()
    {
        target = homeTarget;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }

    public void MakeNewTarget()
    {
        target = newTarget;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }

    public void MakeNewTarget2()
    {
        target = newTarget2;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }

    public void MakeNewTarget3()
    {
        target = newTarget3;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }

    public void MakeNewTarget4()
    {
        target = newTarget4;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }

    public void MakeNewTarget5()
    {
        target = newTarget5;
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }
}
