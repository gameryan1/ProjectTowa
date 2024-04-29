using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cashTxt;
    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] TextMeshProUGUI waveTxt;
    [SerializeField] TextMeshProUGUI ennemiRestantTxt;
    [SerializeField] TextMeshProUGUI currentXp;
    [SerializeField] Animator animator;

    bool menuOpen = true;
    // Start is called before the first frame update

    private void OnGUI()
    {
        cashTxt.text = Player.player.cash.ToString();
        hpTxt.text = ($"hp : { Player.hp}");
        waveTxt.text = ($" Wave: {GameManager.instance.currentWave}");
        ennemiRestantTxt.text = ($" Ennemy en vie: {GameManager.instance.ennemyEnVie}");
       currentXp.text = Player.exp.ToString();

    }
    public void InteractMenu()
    {
        menuOpen = !menuOpen;
        animator.SetBool("Isopen", menuOpen);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
