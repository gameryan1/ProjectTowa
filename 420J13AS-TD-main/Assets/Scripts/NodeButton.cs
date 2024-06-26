using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NodeState
{
    Obtained, // Vert
    Accessible, // Jaune
    Unaccessible // Rouge
}

public class NodeButton : MonoBehaviour
{
    [SerializeField] NodeButton parentNode;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text valueText;
    [SerializeField] int bonusHP = 1;

   // LineRenderer lineRenderer;
    NodeState currentState = NodeState.Unaccessible;
    List<NodeButton> children = new List<NodeButton>();

    private void OnGUI()
    {
        if (currentState == NodeState.Unaccessible)
        {
            valueText.color = Color.red;
        }
        if (currentState == NodeState.Accessible)
        {
            valueText.color = Color.yellow;
        }
        if (currentState == NodeState.Obtained)
        {
            valueText.color = Color.green;
        }
    }
    private void Awake()
    {
        valueText.text = $"+{bonusHP} HP";
        if (parentNode != null)
        {
            parentNode.children.Add(this);
          //  lineRenderer = GetComponent<LineRenderer>();
            //lineRenderer.SetPosition(0, transform.position);
           // lineRenderer.SetPosition(1, parentNode.transform.position);
        }
    }

    private void Start()
    {
        if (parentNode == null)
        {
            // On est � la racine
            SetState(NodeState.Accessible);
        }
    }

    private void SetState(NodeState nodeState)
    {
        currentState = nodeState;
        switch (currentState)
        {
            case NodeState.Obtained:
                Player.bonusHP++;
                Player.player.Extralife();

                //spriteRenderer.color = Color.green;
                foreach (var child in children)
                    child.SetState(NodeState.Accessible);
                break;
            case NodeState.Accessible:
                //spriteRenderer.color = new Color(1, 0.75f, 0);
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
            case NodeState.Unaccessible:
                //spriteRenderer.color = Color.red;
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
        }
    }

    public void ChangeTryget()
    {
        if (currentState == NodeState.Accessible)
        {
            if (Player.player.ExpCheck())
            {
                Player.player.SpendXp();

                SetState(NodeState.Obtained);
            }
        }
    }
}
