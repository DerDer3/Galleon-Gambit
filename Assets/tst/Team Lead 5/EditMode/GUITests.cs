using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GUITests
{
    private EnemyHUD _hud;
    private Enemy _enemy;

    [SetUp]
    public void Setup()
    {
        _enemy = new Enemy();
        GameObject hudObject = new GameObject();
        _hud = hudObject.AddComponent<EnemyHUD>();
        _hud.hpSlider = new GameObject("Slider").AddComponent<Slider>();
        _enemy.maxHealth = 10;
        _hud.SetHUD(_enemy);
    }

    [Test]
    public void HUDHealthBounds()
    {
        //HUD's hpSlider maxValue should always have value of enemy's max health, no matter what is done to the enemy's current health
        _enemy.currentHealth = 100;
        _hud.SetHUD(_enemy);
        Assert.AreEqual(_enemy.maxHealth, _hud.hpSlider.maxValue);

        //HUD's min value should never go below 0. This is to prevent visual issues 
        _enemy.currentHealth -= 11;
        _hud.SetHUD(_enemy);
        Assert.AreEqual(0f, _hud.hpSlider.value);

        //HUD's min value can be 0 
        _enemy.currentHealth -= 10;
        _hud.SetHUD(_enemy);
        Assert.AreEqual(0f, _hud.hpSlider.value);
    }
}

   