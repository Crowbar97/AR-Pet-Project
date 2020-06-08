using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurtleStatControl : MonoBehaviour {
    // Health
    public float health_capacity = 100f;
    public float health;
    public float health_dec_speed = 10f;
    public float health_inc_amount = 30f;
    public bool is_dead = false;
    public Slider healthBar;

    // Energy
    public float energy_capacity = 100f;
    public float energy;
    public float energy_inc_speed = 20f;
    public float energy_cost = 30f;
    public bool is_rest = false;
    public Slider energyBar;
    public Image energyBarBg;

    // Animation controller
    TurtleAnimControll animController;

    // Sound controller
    public AudioSource eatSound;
    public AudioSource restSound;
    public AudioSource deathSound;

    // Button controller
    public GameObject RestartButton;
    

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start() {
       health = health_capacity;
       energy = energy_capacity;
       energyBarBg = energyBar.gameObject.transform.Find("Background").GetComponent<Image>();
       animController = gameObject.GetComponent<TurtleAnimControll>();

       GameObject.Find("LoadingCanvas").SetActive(false);
    }

    void UpdateHealthBar() {
        healthBar.value = health / health_capacity;
        Debug.Log("Health: " + health);
    }

    void Die() {
        animController.ButtonDead();
        eatSound.Stop();
        restSound.Stop();
        deathSound.Play();
        is_rest = false;
        is_dead = true;
        Debug.Log("Dead!");
        RestartButton.SetActive(true);
    }

    void DecreaseHealth() {
        if (!is_dead) {
            health -= health_dec_speed * Time.deltaTime;

            if (health < 0)
                health = 0;
            UpdateHealthBar();

            if (health == 0)
                Die();
        }
    }

    void UpdateEnergyBar() {
        energyBar.value = energy / energy_capacity;
        Debug.Log("Energy: " + energy);
    }

    void IncreaseEnergy() {
        if (is_rest) {
            energy += energy_inc_speed * Time.deltaTime;

            if (energy > energy_capacity)
                energy = energy_capacity;
        }
        UpdateEnergyBar();
    }

    void Update() {
        DecreaseHealth();
        IncreaseEnergy();
    }

    IEnumerator HgEnergyBar() {
        Debug.Log("One!");
        Color initColor = energyBarBg.color;
        energyBarBg.color = new Color(171f / 255f, 30f / 255f, 0f / 255f);
        yield return new WaitForSeconds(0.25f);
        energyBarBg.color = initColor;
        Debug.Log("Two!");
    }

    void IncreaseHealth() {
        health += health_inc_amount;
        if (health > health_capacity)
            health = health_capacity;
        UpdateHealthBar();
    }

    void DecreaseEnergy() {
        energy -= energy_cost;
        if (energy < 0)
            energy = 0;
        UpdateEnergyBar();
    }

    // FIXME: don't allow animation when no energy
    public void EatAction() {
        if (energy < energy_cost)
            StartCoroutine(HgEnergyBar());
        else {
            is_rest = false;
            IncreaseHealth();
            DecreaseEnergy();
            animController.ButtonEat();
            restSound.Stop();
            eatSound.Play();
        }
    }

    public void RestAction() {
        is_rest = true;
        animController.ButtonRest();
        eatSound.Stop();
        restSound.Play();
    }
}
