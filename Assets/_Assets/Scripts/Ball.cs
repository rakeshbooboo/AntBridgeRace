using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshRenderer meshRenderer;
    public Animator animator;
    private void Start()
    {
    }
    private void OnBecameVisible()
    {
        if (meshRenderer != null) {
            meshRenderer.enabled = true;
        }
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.enabled = true;
        }
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
    private void OnBecameInvisible()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.enabled = false;
        }
        if (animator != null)
        {
            animator.enabled = false;
        }
    }
}
