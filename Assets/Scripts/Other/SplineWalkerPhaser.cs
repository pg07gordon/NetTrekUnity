using UnityEngine;

public enum WALKERDIRECTION
{
    forward,
    backward
}

public class SplineWalkerPhaser : MonoBehaviour
{
    public BezierSpline spline;

    public float duration;
    public WALKERDIRECTION direction = WALKERDIRECTION.forward;

    [SerializeField]
    public bool lookForward;

    private float progress = 0;


    private void Start()
    {
        if (direction == WALKERDIRECTION.backward)
        {
            progress = 1f;
        }
    }

    private void Update()
    {
        if (direction == WALKERDIRECTION.forward)
        {
            progress += Time.deltaTime / duration;

            if (progress > 0.5f)
            {
                progress = 0.5f;
            }
        }
        else
        {
            progress -= Time.deltaTime / duration;

            if (progress < 0.5f)
            {
                progress = 0.5f;
            }
        }

        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}
