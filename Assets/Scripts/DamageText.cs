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
        text.DOKill();
        Destroy(gameObject);
    }

    public void SetText(Color colorIn, int damage, Vector3 worldspace, float typeMultiplier = 1, bool crit = false)
    {

        if (damage == 0)
        {
            AnimEnd();
            return;
        }    
        text.color = colorIn;
        text.text = damage.ToString();
        if (crit || typeMultiplier > 1) text.text += "!";

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldspace);
        GetComponent<RectTransform>().position = screenPos;


        float randomX = Random.Range(-7f, 7f);
        float moveUpY = 15f;

        Vector3 moveOffset = new Vector3(randomX, moveUpY, 0f);

        var seq = DOTween.Sequence();
        seq.SetUpdate(true);

        // Animate the movement


        //.Join(text.DOColor(Color.white, 0.2f));

        float killTime = 0.51f;
        float scalemod = 8;

        if (crit)
        {
            scalemod = 4;
            killTime = 0.751f;
            seq.Join(transform.DOShakePosition(0.4f, strength: 50f, vibrato: 10))
            .Join(text.DOColor(Color.blue, 0.1f).SetLoops(2, LoopType.Yoyo));
        }

        if (typeMultiplier > 1)
        {
            scalemod = 6;
            killTime = 0.751f;
            seq.Join(transform.DOShakePosition(0.4f, strength: 50f, vibrato: 10))
            .Join(text.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo));
        }

        if (typeMultiplier < 1)
        {
            scalemod = 10;
            killTime = 0.751f;
            seq.Join(transform.DOScale(0.4f, 0.2f).SetLoops(2, LoopType.Yoyo))
            .Join(text.DOColor(Color.gray, 0.3f));
        }
        

        seq.Join(transform.DOScale(1.3f + (damage / scalemod), 0.25f)
            .SetEase(Ease.OutQuad)
            
            .OnComplete(() =>
            {
                transform.DOScale(0.5f, 0.25f).SetEase(Ease.InQuad);
            })
        )
        .Join(transform.DOMove(transform.position + moveOffset, 0.5f))
        .Join(text.DOFade(0f, killTime)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                AnimEnd();
            }));

        

    }
}
