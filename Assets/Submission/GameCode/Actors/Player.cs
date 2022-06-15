using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Actor
{
    SpriteRenderer renderer;

    [SerializeField]
    Image hpSlider;

    [SerializeField]
    STRLevelData status;

    float maxHp;

    // Start is called before the first frame update
    void Start()
    {
        renderer = animator.GetComponent<SpriteRenderer>();
        GameObject o = GameObject.Find("GameSystem");
        // ask for player level
        status = o.GetComponent<GameSystem>().GetPlayerLevelData(out int levelPlayer);
        life = maxHp = status.GetStatusData().hp;
        GetComponent<Attack>().SetAtkDmg(status.GetStatusData().dmg);
        // sync with slider
        hpSlider.fillAmount = 1f;
    }

    public void ReThinkStatus() {
        life = maxHp = status.GetStatusData().hp;
        GetComponent<Attack>().SetAtkDmg(status.GetStatusData().dmg);
        // sync with slider
        hpSlider.fillAmount = 1f;
    }

    void HpSlider() {
        hpSlider.fillAmount = (life/maxHp)* 1f;
    }

    // Update is called once per frame
    void Update()
    {
        WallSlideOverride();
        HpSlider();
    }

    void WallSlideOverride() {
        renderer.flipX = animator.GetBool("IsWallSliding");
    }
}
