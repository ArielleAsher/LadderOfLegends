using UnityEngine;
using System.Collections;
public class ButtonAnimation : MonoBehaviour
{
    public Vector3 initialScale = new Vector3(1f, 1f, 1f);
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    public float scaleChangeSpeed = 1f;

    private void Start()
    {
        // Start the animation coroutine when the game starts
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        while (true)
        {
            // Scale up
            while (transform.localScale.x < maxScale.x)
            {
                transform.localScale += (maxScale - initialScale) * Time.deltaTime * scaleChangeSpeed;
                yield return null;
            }

            // Scale down
            while (transform.localScale.x > initialScale.x)
            {
                transform.localScale -= (maxScale - initialScale) * Time.deltaTime * scaleChangeSpeed;
                yield return null;
            }
        }
    }
}