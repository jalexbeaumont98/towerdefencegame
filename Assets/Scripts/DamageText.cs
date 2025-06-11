using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void AnimEnd()
    {
        transform.DOKill();
        Destroy(gameObject);
    }

    public void SetText(Color colorIn, int damage, Vector3 worldspace)
    {
        text.color = colorIn;
        text.text = damage.ToString();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldspace);
        GetComponent<RectTransform>().position = screenPos;

        float randomX = Random.Range(-7f, 7f);
        float moveUpY = 15f;

        Vector3 moveOffset = new Vector3(randomX, moveUpY, 0f);

        // Animate the movement
        transform.DOMove(transform.position + moveOffset, 0.5f);


        //transform.DOMoveY(transform.position.y + 15f, 0.5f);

        transform.DOScale(1.3f+(damage/8), 0.25f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(0.5f, 0.25f).SetEase(Ease.InQuad);
            });

        text.DOFade(0f, 0.51f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                AnimEnd();
            });

    }
}
