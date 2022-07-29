using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Slider _healthBar;
    [SerializeField] Button _restartButton;
    [SerializeField] Button _quitButton;

    [SerializeField] int _slainGoal = 10;
    private int _slainMonsters = 0;
    [SerializeField] TMP_Text _enemyText;
    [SerializeField] TMP_Text _winText;
    [SerializeField] TMP_Text _deadText;

    public void UpdateHealth(float health)
    {
        float percentageHealth = health / 100;

        _healthBar.value = percentageHealth;
    }

    public void UpdateDeadMonsters()
    {
        _slainMonsters++;
        _enemyText.text = "Monsters slain: " + _slainMonsters;

        if (_slainMonsters >= _slainGoal)
        {
            _winText.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
            _quitButton.gameObject.SetActive(true);
        }
    }

    public void DisplayDeadUI()
    {
        _deadText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
