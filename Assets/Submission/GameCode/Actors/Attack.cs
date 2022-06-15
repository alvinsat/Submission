using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	[SerializeField]
	private float dmgValue = 4;
	[SerializeField]
	private GameObject throwableObject;
	[SerializeField]
	private Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	[SerializeField]
	private Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	[SerializeField]
	private GameObject cam;
	CameraFollow camSys;

	// temporary
	GameObject tempThrowableWeapon;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
		camSys = cam.GetComponent<CameraFollow>();  
		
    }

	public void SetAtkDmg(float atkVal) {
		dmgValue = atkVal;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.X) && canAttack)
		{
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackDelay());
			StartCoroutine(AttackCooldown());
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			tempThrowableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			tempThrowableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			tempThrowableWeapon.name = "ThrowableWeapon";
		}
	}

	/// <summary>
	/// Should not be larger than attack cooldown
	/// </summary>
	/// <returns></returns>
	IEnumerator AttackDelay() {
		yield return new WaitForSeconds(.1f);
		DoDashDamage();
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.CompareTag("Enemy"))
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				//collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				collidersEnemies[i].gameObject.GetComponent<IDamageable>().ApplyDamage(dmgValue);
				//cam.GetComponent<CameraFollow>().ShakeCamera();
				camSys.ShakeCamera();
			} 
			else if (collidersEnemies[i].gameObject.CompareTag("Grass"))
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				//collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				collidersEnemies[i].gameObject.GetComponent<IDamageable>().ApplyDamage(dmgValue);
				//cam.GetComponent<CameraFollow>().ShakeCamera();
				camSys.ShakeCamera(.1f, .07f, 1f);
			}
		}
	}
}
