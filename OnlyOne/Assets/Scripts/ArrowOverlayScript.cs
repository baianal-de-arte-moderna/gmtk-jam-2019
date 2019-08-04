using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOverlayScript : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Shot()
    {
        animator.SetTrigger("Shot");
    }
    public void Retrieve()
    {
        animator.SetTrigger("Retrieve");
    }
}
