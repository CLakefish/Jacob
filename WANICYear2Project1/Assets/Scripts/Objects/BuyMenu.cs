using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject Menu;
    [SerializeField] private List<Button> Buttons = new();
    internal PlayerHealth playerHealth;
    internal PlayerAttackController playerAttack;
    internal MovementController playerMovement;
    internal ScoreAndTimer playerPoints;

    static internal BuyMenu Instance;

    [Header("Variables")]
    private bool Open = false;

    void Start() {
        playerPoints = FindObjectOfType<ScoreAndTimer>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerAttack = FindObjectOfType<PlayerAttackController>();
        playerMovement = FindObjectOfType<MovementController>();

        Instance = this;
    }

    void Update()
    {
        if (Open)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Open = false;
                Menu.SetActive(false);
            }
        }
    }

    public void Pay(int quantity)
    {
        playerPoints.currentScore -= quantity;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Open = true;
        Menu.SetActive(true);
        ButtonInstantiation();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Open = false;
        Menu.SetActive(false);
    }

    public void ButtonInstantiation()
    {
        if (playerPoints.currentScore - (100 * (int)playerHealth.HealthBar.maxValue * 12) < 0) {
            Buttons[1].GetComponent<Image>().color = Color.red;
            Buttons[1].GetComponent<Button>().enabled = false;
        }
        else {
            Buttons[1].GetComponent<Image>().color = Color.white;
            Buttons[1].GetComponent<Button>().enabled = true;
        }

        if (playerPoints.currentScore - (100 * playerAttack.MaxStamina) < 0)  {
            Buttons[2].GetComponent<Image>().color = Color.red;
            Buttons[2].GetComponent<Button>().enabled = false;
        }
        else {
            Buttons[2].GetComponent<Image>().color = Color.white;
            Buttons[2].GetComponent<Button>().enabled = true;
        }
    }

    public void AddHP()
    {
        playerHealth.HealthBar.maxValue++;
        playerHealth.HealthBar.value++;

        Pay(100 * (int)playerHealth.HealthBar.maxValue * 12);
    }

    public void AddStamina()
    {
        playerAttack.MaxStamina += playerAttack.MaxStamina / 6;
        playerAttack.AttackSlider.maxValue = playerAttack.MaxStamina;

        Pay(100 * playerAttack.MaxStamina);
    }
}