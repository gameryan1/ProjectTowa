using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    public static Player player;
    public static int bonusHP = 0;
    public static int basehp = 3;
    public static float multiplicateurCash = 1;
    public static int hp = bonusHP + basehp;
    public static int exp = 0;
    [SerializeField] public int cash = 20;
    private void Awake()
    {
        player = this;
        //hpText.text = $"HP: {hp + bonusHP}";
    }

    public void PerdreHp()
    {
        if (hp-- <= 0) {
            Application.LoadLevel(Application.loadedLevel);
            hp = basehp;
        }
    }

    public void Extralife()
    {
        hp++;
        bonusHP++;
    }
    public void SetHp()
    {
        hp = basehp + bonusHP;
    }
    public void GetXP()
    {
        exp++;
    }
    public void SpendXp()
    {
        exp--;
    }
    public bool ExpCheck()
    { return exp > 0; }

   public void MoneyMultiplicateur()
    {
        multiplicateurCash += 0.05f;
    }


    public void GagnerCash(int montantGagner)
    {
        cash += (int)Mathf.Round(montantGagner * multiplicateurCash);
    }

    public bool DepenseCash(int cout)
    {
        if (cash < cout) {return false;}
        else {
        cash -= cout;
        return true;}
    }
}
