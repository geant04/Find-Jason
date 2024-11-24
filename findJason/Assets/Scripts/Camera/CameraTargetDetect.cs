using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetDetect : MonoBehaviour
{
    public Camera cam;
    private GameObject Quad;
    private MeshRenderer QuadMeshRenderer;
    private Material DetectorMat;
    private bool IsJasonFound = false;

    private Vector2 viewportBounds = new Vector2(0.1f, 0.1f); // assumed centered at origin
    private float progressToComplete = 0.0f;
    private float decaySpeed = 2.4f;
    private float progressSpeed = 1.2f;
    private bool camReadyTriggered = false;
    private bool clicked = false;

    public bool IsTargetInViewPort(Vector3 target)
    {
        Vector3 targetToViewPort = cam.WorldToViewportPoint(target);

        float xRight = 0.5f + viewportBounds.x / 2.0f;
        float xLeft = 0.5f - viewportBounds.x / 2.0f;
        float yTop = 0.5f + viewportBounds.y / 2.0f;
        float yBottom = 0.5f - viewportBounds.y / 2.0f;

        return targetToViewPort.x > xLeft && targetToViewPort.x < xRight 
            && targetToViewPort.y > yBottom && targetToViewPort.y < yTop;
    }

    public bool IsJasonVisible(GameObject Jason)
    {
        GameObject JasonCapsule = Jason.transform.Find("Capsule").gameObject; // 2jank4me
        Renderer jasonRenderer = JasonCapsule.GetComponent<Renderer>();

        if (jasonRenderer == null || !jasonRenderer.isVisible) return false;

        if (!IsTargetInViewPort(Jason.transform.position)) return false;

        return GetVisiblityPercentage(jasonRenderer.bounds, JasonCapsule) > 0.10f;
    }

    float GetVisiblityPercentage(Bounds bounds, GameObject Target)
    {
        float hits = 0.0f;
        int samples = 5;

        Vector3[] points = new Vector3[samples * samples]; // fire off of a 2D grid of sample points

        for (int i = 0; i < samples; i++)
        {
            for (int j = 0; j < samples; j++)
            {
                float px = bounds.min.x + bounds.size.x * (i / (float)(samples - 1));
                float py = bounds.min.y + bounds.size.y * (j / (float)(samples - 1));

                points[i * samples + j] = new Vector3(px, py, bounds.center.z);
            }
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (!IsTargetInViewPort(points[i])) continue;

            Vector3 dir = points[i] - cam.transform.position;

            if (Physics.Raycast(cam.transform.position, dir, out RaycastHit hit))
            {
                if (hit.transform == Target.transform) hits += 1.0f;
            }
        }

        // Debug.Log(hits / points.Length); // uncomment for debugging

        return hits / points.Length;
    }

    public void AssignDetection(bool Active)
    {
        if (!camReadyTriggered && Active)
        {
            camReadyTriggered = true;
            AudioClass.Instance.sfxSource.PlayOneShot(AudioClips.Instance.cameraReady);
        }

        DetectorMat.SetInt("_Detected", Active ? 1 : 0);
    }

    public void SetClick(bool Active) // gets called externally from CameraController
    {
        DetectorMat.SetInt("_Clicked", Active ? 1 : 0);
        clicked = Active;
    }

    public void UpdateUI()
    {
        DetectorMat.SetFloat("_Progress", progressToComplete);
    }

    public void Start()
    {
        if (cam == null) return;

        Quad = cam.transform.Find("Quad").gameObject;
        QuadMeshRenderer = Quad.GetComponent<MeshRenderer>();
        DetectorMat = QuadMeshRenderer.material;

        if (DetectorMat == null) return;

        DetectorMat.SetVector("_Bounds", viewportBounds);
    }

    public bool GetIsJasonFullyFound()
    {
        return IsJasonFound && progressToComplete >= 1.0f && !clicked;
    }

    public void Reset()
    {
        IsJasonFound = false;
        SetClick(false);
        camReadyTriggered = false;
        progressToComplete = 0.0f;
    }

    public void Update()
    {
        GameObject Jason = GameManager.Jason;
        if (Jason == null) return;
        if (clicked) return;

        IsJasonFound = IsJasonVisible(Jason);
        Jason.GetComponent<NPC>().isFound = IsJasonFound;

        AssignDetection(IsJasonFound && progressToComplete >= 1.0f);

        if (!IsJasonFound)
        {
            camReadyTriggered = false;
            progressToComplete = Mathf.Max(0.0f, progressToComplete - decaySpeed * Time.deltaTime);
        }

        if (progressToComplete < 1.1f && IsJasonFound)
        {
            progressToComplete = Mathf.Min(1.1f, progressToComplete + progressSpeed * Time.deltaTime);
        }

        UpdateUI();
    }
}
