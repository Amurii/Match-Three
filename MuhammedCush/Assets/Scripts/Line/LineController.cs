using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Texture[] textures;
    int animationStep;
   float fps = 50f;
    float fpsCounter;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //target = this.transform;
    }
    private void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter > 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;
            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0;
        }
    }
    public void AssignTarget(Vector3 startPos,Transform newTarget)
    {
        gameObject.SetActive(true);
        lineRenderer.SetPosition(1, newTarget.position);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        target = newTarget;
    }

}
