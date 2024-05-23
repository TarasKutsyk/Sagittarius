using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    int currentHp;
    [SerializeField] TextMeshProUGUI hpText;

    Player player;

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            DisplayHp();
        }
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        currentHp = player.Hp;
        DisplayHp();
    }

    public void DisplayHp()
    {
        if (currentHp < 0)
             hpText.text = "0";
        else hpText.text = currentHp.ToString();
    }
}
