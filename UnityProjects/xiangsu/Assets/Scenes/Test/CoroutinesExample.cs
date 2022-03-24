using UnityEngine;
using System.Collections;

public class CoroutinesExample : MonoBehaviour
{
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(transform.position, Vector2.right * 10, color);

        if (hit != false)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Rubbish"))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}