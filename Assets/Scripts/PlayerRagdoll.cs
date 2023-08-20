using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] rb_Ragdoll;

    void Start()
    {
        rb_Ragdoll = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rb_Ragdoll)
        {
            rb.isKinematic = true;
        }
            
        
    }

    public void RagDools()
    {
        foreach(Rigidbody rb in rb_Ragdoll)
        rb.isKinematic = false;
        GetComponentInChildren<Animator>().enabled = false;
    }
}
