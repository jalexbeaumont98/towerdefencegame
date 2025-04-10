using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public EnemyStatus status;
    public Sprite sprite;

    public TMP_Text amountText;

    public UnityEngine.UI.Image image;

    void Start()
    {
        //image = GetComponent<UnityEngine.UI.Image>();
    }

    public void SetIcon(EnemyStatus status)
    {

        
        this.status = status;
        image.sprite = status.icon;

        

    }

    public void SetText(int amount)
    {
        if (amount > 0)
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
