using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Teleport : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public VisualEffect visualEffect;
    public float targetHeight = 3f;
    public float startHeight = 0f;
    public FadeDirection fadeDirection;
    private VisualEffect effect;

    private float lifetime = 2f;
    private float timer = 0f;
    private void Start()
    {
        float initialFadeValue = fadeDirection == FadeDirection.FADEIN ? 0f : 1f;
        meshRenderer.material.SetFloat("FadeOut01", initialFadeValue);

        Vector3 targetPosition = transform.position + Vector3.up * startHeight;
        effect = Instantiate(visualEffect.gameObject, targetPosition, Quaternion.identity).GetComponent<VisualEffect>();
        effect.SetFloat("Target Height", targetHeight);
        lifetime = effect.GetFloat("Rings Lifetime") * Mathf.Abs(targetHeight);
        effect.Play();
    }

    private void Update()
    {
        if(timer < lifetime)
        {
            timer += Time.deltaTime * effect.playRate;
            float fadeout01 = timer / lifetime;
            if (fadeDirection == FadeDirection.FADEIN)
                fadeout01 = 1f - fadeout01;

            fadeout01 = Mathf.Clamp01(fadeout01);
            Debug.Log(fadeout01);
            meshRenderer.material.SetFloat("_FadeOut01", fadeout01);
        }
        else
        {
            if(fadeDirection == FadeDirection.FADEOUT)
            {
                Destroy(gameObject);
            }
        }
    }
}

public enum FadeDirection
{
    FADEIN,
    FADEOUT
}
