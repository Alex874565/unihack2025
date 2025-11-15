using UnityEngine;

public class RandomAnimatorStart : MonoBehaviour
{
    [Range(0f, 1f)]
    public float startOffset = 0f;   // Set in Inspector (0 = start, 1 = full loop)

    public bool randomizeOnStart = true;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();

        float offset = randomizeOnStart ? Random.value : startOffset;

        animator.Play(0, 0, offset);
    }
}
