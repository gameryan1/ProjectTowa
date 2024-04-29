using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouConstructeur : MonoBehaviour
{
    public static TouConstructeur instance;
    // Start is called before the first frame update
    [SerializeField] private Shoptower[] tourelles;

    public int tourSelec = 0;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Shoptower PrendreTourSelectionner()
    {
        return tourelles[tourSelec];
    }
    public void SetTour(int selection)
    {
        tourSelec = selection;
    }

    public bool Canbuild()
    {
        return tourelles[tourSelec].cout <= Player.player.cash;
    }
}
