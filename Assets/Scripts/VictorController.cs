using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VictorController : CharacterController
{
    private DestroyGameObjects destroyGameObjects;

    [Header("GUI")]
    [SerializeField] private SliderController sliderLife;
    [SerializeField] private SliderController sliderShield;
    [SerializeField] private SliderController sliderMovementSpeed;
    [SerializeField] private SliderController sliderAttackSpeed;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    protected override void Start()
    {
        base.Start();

        destroyGameObjects = FindObjectOfType<DestroyGameObjects>();

        sliderLife.InitializeBarStat(life, maxLife);
        sliderShield.InitializeBarStat(shield, maxShield);
        sliderMovementSpeed.InitializeBarStat(movementSpeed, maxMovementSpeed);
        sliderAttackSpeed.InitializeBarStat(attackSpeed, maxAttackSpeed);

        UpdateStats();
    }
    private void CheckVictory()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            destroyGameObjects.DestroyObjects();
            SceneManager.LoadScene("Victoria");
        }
    }
    private void GoToLoose()
    {
        destroyGameObjects.DestroyObjects();
        SceneManager.LoadScene("Derrota");
    }
    override protected void Die()
    {
        base.Die();
        Invoke("GoToLoose", 2f);

    }

    override public void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        UpdateStats();
    }
    private void UpdateStats()
    {
        sliderLife.ChangeActualValue(life);
        sliderShield.ChangeActualValue(shield);
        sliderMovementSpeed.ChangeActualValue(movementSpeed);
        sliderAttackSpeed.ChangeActualValue(attackSpeed);
    }

    internal void PickUpItem(Stats stats, float points)
    {
        if (!IsAlive)
            return;

        switch (stats)
        {
            case Stats.LifePoints:
                this.life = Mathf.Clamp(this.life + points, 0, maxLife);
                break;
            case Stats.Shield:
                this.shield = Mathf.Clamp(this.shield + points, 0, maxShield);
                break;

            case Stats.characterSpeed:
                this.movementSpeed = Mathf.Clamp(this.movementSpeed + points, 0, maxMovementSpeed);
                break;

            case Stats.attackSpeed:
                this.attackSpeed = Mathf.Clamp(this.attackSpeed + points, 0, maxAttackSpeed);
                break;
        }

        UpdateStats();
    }
    protected override bool IsAttacking()
    {
        return Input.GetMouseButton(0);
    }
    override protected void ExecuteAttack()
    {
        if(shootSound != null)
            AudioSource.PlayClipAtPoint(shootSound, transform.position);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        BulletController newBullet = Instantiate(bullet, bulletSpawner.position, Quaternion.identity);

        newBullet.Init(this, shootDirection);
    }

    override protected void ExecuteMove()
    {
        rb.velocity = Vector2.zero;
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity += movement * movementSpeed;
    }

    override protected void ExecuteUpdate()
    {
        CheckVictory();
    }
}
