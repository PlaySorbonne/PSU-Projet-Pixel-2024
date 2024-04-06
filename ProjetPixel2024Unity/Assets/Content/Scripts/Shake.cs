using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float default_duration = 0.75f;
    public float default_magnitude = 0.5f;
    public float default_frequency = 20f;
    
    private float duration = -1f;
    private float magnitude = -1f;
    private float frequency = -1f;
    private float delayBetweenShakes = -1f;

    
    public void ScreenShake(float _duration = -1f, float _magnitude = -1f, float _frequency = -1f) {
        duration = _duration;
        if (duration < 0f)
            duration = default_duration;
        magnitude = _magnitude;
        if (magnitude < 0f)
            magnitude = default_magnitude;
        frequency = _frequency;
        if (frequency <= 0f)
            frequency = default_frequency;
        delayBetweenShakes = 1f / frequency;

        StartCoroutine(Shaking());
    }

    IEnumerator Shaking() {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0.0f;
        float lastShake = 0.0f;

        while (elapsedTime < duration) {
            if (lastShake <= 0f) {
                float x = Random.Range(-0.1f, 0.1f) * magnitude;
                float y = Random.Range(-0.1f, 0.1f) * magnitude;

                transform.position = originalPos + new Vector3(x, y, 0f);
                lastShake = delayBetweenShakes;
            }
            
            elapsedTime += Time.deltaTime;
            lastShake -= Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }
}
