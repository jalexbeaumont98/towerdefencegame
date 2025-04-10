using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnchor : MonoBehaviour
{
    public Transform target; // The GameObject to follow
    public Vector3 offset;   // Optional offset (e.g. to hover above head)

    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetAnchor(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        rectTransform.position = screenPos;

        // Optionally, hide UI if behind the camera
        /*
        if (screenPos.z < 0)
        {
            rectTransform.gameObject.SetActive(false);
        }
        else
        {
            rectTransform.gameObject.SetActive(true);
            rectTransform.position = screenPos;
        }
        */
    }
}
