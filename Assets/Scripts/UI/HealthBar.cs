using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthController playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private IEnumerator Start() {
        yield return new WaitForSeconds(0.5f);
        playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthController>();
        totalHealthBar.fillAmount = playerHealth.unlockedHeal / 10;
    }
    private void FixedUpdate() {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthController>();
        currentHealthBar.fillAmount = playerHealth.health / 10;
        totalHealthBar.fillAmount = playerHealth.unlockedHeal / 10;
    }
}
