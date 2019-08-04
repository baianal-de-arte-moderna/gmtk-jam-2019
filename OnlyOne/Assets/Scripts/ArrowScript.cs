using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    new Rigidbody rigidbody;
    PlayerScript player;
    SphereCollider RetrievableCollider;
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        RetrievableCollider = GetComponentInChildren<SphereCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.AddForce(
            transform.forward * 50000f
        );
        Invoke("ActivateRetrieve", 1f);
    }

    void ActivateRetrieve()
    {
        RetrievableCollider.enabled = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                0.5f
            );
        }
    }

    public void ReturnTo(PlayerScript returner)
    {
        player = returner;
    }
}
