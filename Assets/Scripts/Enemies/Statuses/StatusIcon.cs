using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public Sprite sprite;

    public TMP_Text amountText;

    [SerializeField] public UnityEngine.UI.Image image;

    void Start()
    {
        //image = GetComponent<UnityEngine.UI.Image>();
    }

    public void SetIcon(Sprite sprite)
    {
        
        print("image should be set!");
        Debug.Log($"Setting sprite: {sprite?.name ?? "NULL"}");
        image.sprite = sprite;

    }

    public void SetText(int amount)
    {
        if (amount > 1)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.gameObject.SetActive(false);
        }
    }
}
