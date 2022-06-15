using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable {

	public float life = 10;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	[SerializeField]
	private Animator anim;
	CapsuleCollider2D capsule;
	PlayerSaveSystem playerSaveData;


	private bool facingRight = true;
	
	public float speed = 5f;

	public bool isInvincible = false;
	private bool isHitted = false;

	[SerializeField]
	AudioClip clipDefeated;
	[SerializeField]
	AudioSource speaker;

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		playerSaveData = GameObject.Find("GameSystem").GetComponent<PlayerSaveSystem>();
		rb = GetComponent<Rigidbody2D>();
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}
		capsule = GetComponent<CapsuleCollider2D>();
		speaker = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (life <= 0) {
			//transform.GetComponent<Animator>().SetBool("IsDead", true);
			anim.SetBool("IsDead", true);
			playerSaveData.IncrementCoins(1);
			speaker.PlayOneShot(clipDefeated);
			StartCoroutine(DestroyEnemy());
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}
			}
			else
			{
				Flip();
			}
		}
	}

	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage, Vector3 position)
	{
		// interafe overloading empty
	}

	public void ApplyDamage(float damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			//transform.GetComponent<Animator>().SetBool("Hit", true);
			anim.SetBool("Hit", true);
			life -= damage;// TODO: use demage formula here
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}

	IDamageable target;
	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			anim.SetTrigger("Atk");
			//collision.gameObject.GetComponent<IDamageable>().ApplyDamage(2f, transform.position);
			StartCoroutine(AttackDelay(collision.gameObject.GetComponent<IDamageable>()));
		}
	}

	IEnumerator AttackDelay(IDamageable collision) { 
		yield return new WaitForSeconds(0.1f);
		collision.ApplyDamage(2f, transform.position);

	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		//CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(3f);
		InGameData.nowKill++;
		Destroy(gameObject);
	}
}
