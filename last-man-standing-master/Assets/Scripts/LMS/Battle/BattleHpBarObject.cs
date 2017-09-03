using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHpBarObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer foreground;

    private void Start()
    {
        if (foreground == null)
            foreground = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SetValue(float percent)
    {
        percent = Mathf.Clamp(percent, 0, 1);
        if (foreground != null)
        {
            foreground.transform.localScale = new Vector2(percent, foreground.transform.localScale.y);
        }
    }
    public void ResetScale()
    {
        transform.localScale = new Vector3(2, 0.2f);
    }
}
