using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PickRandomSurfacePoint : MonoBehaviour
{
    public VisualEffect visualEffect;
    public float timerMin = 1f;
    public float timerMax = 2f;
    public float stopAfter = 10f;

    private float timer;
    private float internalTimer = 0f;
    private float stopAfterTimer = 0f;

    bool stop = false;
    List<VisualEffect> visualEffects = new List<VisualEffect>();

    private void Start()
    {
        timer = Random.Range(timerMin, timerMax);
    }

    // Update is called once per frame
    void Update()
    {
        stopAfterTimer += Time.deltaTime;
        if (stopAfterTimer >= stopAfter)
            stop = true;

        if (!stop)
            internalTimer += Time.deltaTime; 

        if (internalTimer >= timer)
        {
            bool test = false;
            float x1 = 0;
            float x2 = 0;
            float x1Sqrd = 0;
            float x2Sqrd = 0;
            while (!test)
            {
                x1 = Random.Range(-1f, 1f);
                x2 = Random.Range(-1f, 1f);
                x1Sqrd = x1 * x1;
                x2Sqrd = x2 * x2;
                if (x1Sqrd + x2Sqrd <= 1)
                    test = true;
            }

            float x = 2 * x1 * Mathf.Sqrt(1 - x1Sqrd - x2Sqrd);
            float y = 2 * x2 * Mathf.Sqrt(1 - x1Sqrd - x2Sqrd);
            float z = 1 - 2 * (x1Sqrd + x2Sqrd);
            y = Mathf.Abs(y);

            x *= (transform.localScale.x * 0.5f);
            y *= (transform.localScale.y * 0.5f);
            z *= (transform.localScale.z * 0.5f);

            x += transform.position.x;
            y += transform.position.y;
            z += transform.position.z;

            
            VisualEffect newVFX = Instantiate(visualEffect);
            newVFX.SetVector3("ObjectPosition", transform.position);
            newVFX.SetVector3("ObjectScale", transform.localScale);
            newVFX.SetVector3("ImpactPosition", new Vector3(x, y, z));

            internalTimer = 0;
            timer = Random.Range(timerMin, timerMax);

            for (int i = 0; i < visualEffects.Count; i++)
            {
                if(visualEffects[i].aliveParticleCount <= 0)
                {
                    Destroy(visualEffects[i].gameObject);
                    visualEffects.Remove(visualEffects[i]);
                }
            }

            visualEffects.Add(newVFX);
        }
    }
}
