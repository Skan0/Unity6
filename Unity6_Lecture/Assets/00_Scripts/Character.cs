using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
