using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthHUD : MonoBehaviour
{
    public GameObject entity;
    private PlayerMovement player;
    private int currentHealth;
    private int dmgTaken;
    public Image hud;
    [SerializeField] private Sprite[] healthStates;

    private void Start() {
        player = entity.GetComponent<PlayerMovement>();
        hud = this.GetComponent<Image>();
        currentHealth = player.health;
        dmgTaken = 0;
    }

    private void Update() {
        if (currentHealth > player.health) {
            currentHealth--;
            dmgTaken++;
            UpdateHUD();
        }
    }

    private void UpdateHUD() {
        switch(dmgTaken) {
            case 1:
                hud.sprite = healthStates[0];
                break;
            case 2:
                hud.sprite = healthStates[1];
                break;
            case 3:
                hud.sprite = healthStates[2];
                break;
        }
    }
}