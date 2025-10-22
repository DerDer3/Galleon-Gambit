using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{

    private Enemy _enemy;


    [SetUp]
    public void Setup()
    {
        _enemy = new Enemy();
        _enemy.maxHealth = 10;
    }

    // A Test behaves as an ordinary method
    [Test]
    
    //test to see what happens when enemy health is at 0, below 0, or exactly 0
    public void TestHealthBounds()
    {
        // when the enemy takes damage equal to it's max health, it should be considered dead
        _enemy.TakeDamage(_enemy.maxHealth);
        Assert.IsTrue(_enemy.isDead);
        _enemy.currentHealth = _enemy.maxHealth;
        // when the enemy takes damage more than it's max health, it should be considered dead
        _enemy.TakeDamage(_enemy.maxHealth+1);
        Assert.IsTrue(_enemy.isDead);
        _enemy.currentHealth = _enemy.maxHealth;
        // when the enemy takes damage less than it's max health, it should be considered alive
        _enemy.TakeDamage(_enemy.maxHealth+1);
        Assert.IsFalse(_enemy.isDead);

    }

    
}
