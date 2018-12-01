using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{

	[RequireComponent(typeof(RPGCharacterMovementController))]
	[RequireComponent(typeof(RPGCharacterInputController))]
	public class RPGCharacterControllerFREE : MonoBehaviour{

		//Components.
		[HideInInspector]	public RPGCharacterMovementController rpgCharacterMovementController;
		[HideInInspector]	public RPGCharacterInputController rpgCharacterInputController;
		[HideInInspector] public Animator animator;
		public GameObject target;

		//Strafing/action.
		[HideInInspector] public bool isDead = false;
		[HideInInspector] public bool canAction = true;
		[HideInInspector] public bool isStrafing = false;

		#region Initialization

		void Awake(){
			rpgCharacterMovementController = GetComponent<RPGCharacterMovementController>();
			rpgCharacterInputController = GetComponent<RPGCharacterInputController>();
			animator = GetComponentInChildren<Animator>();
			if(animator == null){
				Debug.LogError("ERROR: There is no animator for character.");
				Destroy(this);
			}
			if(target == null){
				Debug.LogError("ERROR: There is no target for character.");
				Destroy(this);
			}
		}

		#endregion

		#region Updates

		void Update(){
			if(rpgCharacterMovementController.MaintainingGround()){
				//Revive.
				if(rpgCharacterInputController.inputDeath){
					if(isDead){
						Revive();
					}
				}
				if(canAction){
					Strafing();
					Rolling();
					//Hit.
					if(rpgCharacterInputController.inputLightHit){
						GetHit();
					}
					//Death.
					if(rpgCharacterInputController.inputDeath){
						if(!isDead){
							Death();
						}
						else{
							Revive();
						}
					}
					//Attacks.
					if(rpgCharacterInputController.inputAttackL){
						Attack(1);
					}
					if(rpgCharacterInputController.inputAttackR){
						Attack(2);
					}
					if(rpgCharacterInputController.inputLightHit){
						GetHit();
					}
					//Shooting / Navmesh.
					if(Input.GetMouseButtonDown(0)){
						if(rpgCharacterMovementController.useMeshNav){
							RaycastHit hit;
							if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
								rpgCharacterMovementController.navMeshAgent.destination = hit.point;
							}
						}
					}
				}
			}
			//Slow time toggle.
			if(Input.GetKeyDown(KeyCode.T)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0.05f;
				}
			}
			//Pause toggle.
			if(Input.GetKeyDown(KeyCode.P)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0f;
				}
			}
		}

		#endregion

		#region Turning

		//Turning.
		public IEnumerator _Turning(int direction){
			if(direction == 1){
				Lock(true, true, true, 0, 0.55f);
				animator.SetTrigger("TurnLeftTrigger");
			}
			if(direction == 2){
				Lock(true, true, true, 0, 0.55f);
				animator.SetTrigger("TurnRightTrigger");
			}
			yield return null;
		}

		#endregion

		#region Combat

		/// <summary>
		/// Dodge the specified direction.
		/// </summary>
		/// <param name="1">Left</param>
		/// <param name="2">Right</param>
		public IEnumerator _Dodge(int direction){
			animator.SetInteger("Action", direction);
			animator.SetTrigger("DodgeTrigger");
			Lock(true, true, true, 0, 0.55f);
			yield return null;
		}

		//0 = No side
		//1 = Left
		//2 = Right
		public void Attack(int attackSide){
			int attackNumber = 0;
			if(canAction){
				//Ground attacks.
				if(rpgCharacterMovementController.MaintainingGround()){
					//Stationary attack.
					if(!rpgCharacterMovementController.isMoving){
						//Armed or Unarmed.
						int maxAttacks = 3;
						//Left attacks.
						if(attackSide == 1){
							animator.SetInteger("AttackSide", 1);
							attackNumber = Random.Range(1, maxAttacks + 1);
						}
						//Right attacks.
						else if(attackSide == 2){
							animator.SetInteger("AttackSide", 2);
							attackNumber = Random.Range(4, maxAttacks + 4);
						}
						//Set the Locks.
						Lock(true, true, true, 0, 0.7f);
					}
				}
				//Trigger the animation.
				animator.SetInteger("Action", attackNumber);
				animator.SetTrigger("AttackTrigger");
			}
		}

		public void AttackKick(int kickSide){
			if(rpgCharacterMovementController.MaintainingGround()){
				animator.SetInteger("Action", kickSide);
				animator.SetTrigger("AttackKickTrigger");
				Lock(true, true, true, 0, 0.9f);
			}
		}

		void Strafing(){
			if(rpgCharacterInputController.inputStrafe || rpgCharacterInputController.inputTargetBlock > 0.8f){
				animator.SetBool("Strafing", true);
				isStrafing = true;
			}
			else{
				isStrafing = false;
				animator.SetBool("Strafing", false);
			}
		}

		void Rolling(){
			if(!rpgCharacterMovementController.isRolling){
				if(rpgCharacterInputController.inputRoll){
					rpgCharacterMovementController.DirectionalRoll();
				}
			}
		}

		public void GetHit(){
			int hits = 5;
			int hitNumber = Random.Range(1, hits + 1);
			animator.SetInteger("Action", hitNumber);
			animator.SetTrigger("GetHitTrigger");
			Lock(true, true, true, 0.1f, 0.4f);
			//Apply directional knockback force.
			if(hitNumber <= 1){
				StartCoroutine(rpgCharacterMovementController._Knockback(-transform.forward, 8, 4));
			}
			else if(hitNumber == 2){
				StartCoroutine(rpgCharacterMovementController._Knockback(transform.forward, 8, 4));
			}
			else if(hitNumber == 3){
				StartCoroutine(rpgCharacterMovementController._Knockback(transform.right, 8, 4));
			}
			else if(hitNumber == 4){
				StartCoroutine(rpgCharacterMovementController._Knockback(-transform.right, 8, 4));
			}
		}

		public void Death(){
			animator.SetTrigger("Death1Trigger");
			Lock(true, true, false, 0.1f, 0f);
			isDead = true;
		}

		public void Revive(){
			animator.SetTrigger("Revive1Trigger");
			Lock(true, true, true, 0f, 1f);
			isDead = false;
		}

		#endregion

		#region LockUnlock

		/// <summary>
		/// Keep character from doing actions.
		/// </summary>
		void LockAction(){
			canAction = false;
		}

		/// <summary>
		/// Let character move and act again.
		/// </summary>
		void UnLock(bool movement, bool actions){
			if(movement){
				rpgCharacterMovementController.UnlockMovement();
			}
			if(actions){
				canAction = true;
			}
		}

		#endregion

		#region Misc

		//Placeholder functions for Animation events.
		public void Hit(){
		}

		public void Shoot(){
		}

		public void FootR(){
		}

		public void FootL(){
		}

		public void Jump(){
		}

		public void Land(){
		}

		IEnumerator _GetCurrentAnimationLength(){
			yield return new WaitForEndOfFrame();
			float f = (float)animator.GetCurrentAnimatorClipInfo(0).Length;
			Debug.Log(f);
		}

		/// <summary>
		/// Lock character movement and/or action, on a delay for a set time.
		/// </summary>
		/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
		/// <param name="lockAction">If set to <c>true</c> lock action.</param>
		/// <param name="timed">If set to <c>true</c> timed.</param>
		/// <param name="delayTime">Delay time.</param>
		/// <param name="lockTime">Lock time.</param>
		public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			StopCoroutine("_Lock");
			StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
		}

		//Timed -1 = infinite, 0 = no, 1 = yes.
		public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			if(delayTime > 0){
				yield return new WaitForSeconds(delayTime);
			}
			if(lockMovement){
				rpgCharacterMovementController.LockMovement();
			}
			if(lockAction){
				LockAction();
			}
			if(timed){
				if(lockTime > 0){
					yield return new WaitForSeconds(lockTime);
				}
				UnLock(lockMovement, lockAction);
			}
		}

		/// <summary>
		/// Sets the animator state.
		/// </summary>
		/// <param name="weapon">Weapon.</param>
		/// <param name="weaponSwitch">Weapon switch.</param>
		/// <param name="Lweapon">Lweapon.</param>
		/// <param name="Rweapon">Rweapon.</param>
		/// <param name="weaponSide">Weapon side.</param>
		void SetAnimator(int weapon, int weaponSwitch, int Lweapon, int Rweapon, int weaponSide){
			Debug.Log("SETANIMATOR: Weapon:" + weapon + " Weaponswitch:" + weaponSwitch + " Lweapon:" + Lweapon + " Rweapon:" + Rweapon + " Weaponside:" + weaponSide);
			//Set Weapon if applicable.
			if(weapon != -2){
				animator.SetInteger("Weapon", weapon);
			}
			//Set WeaponSwitch if applicable.
			if(weaponSwitch != -2){
				animator.SetInteger("WeaponSwitch", weaponSwitch);
			}
		}

		public void AnimatorDebug(){
			Debug.Log("ANIMATOR SETTINGS---------------------------");
			Debug.Log("Moving: " + animator.GetBool("Moving"));
			Debug.Log("Strafing: " + animator.GetBool("Strafing"));
			Debug.Log("Aiming: " + animator.GetBool("Aiming"));
			Debug.Log("Stunned: " + animator.GetBool("Stunned"));
			Debug.Log("Swimming: " + animator.GetBool("Swimming"));
			Debug.Log("Blocking: " + animator.GetBool("Blocking"));
			Debug.Log("Injured: " + animator.GetBool("Injured"));
			Debug.Log("LeftRight: " + animator.GetInteger("LeftRight"));
			Debug.Log("AttackSide: " + animator.GetInteger("AttackSide"));
			Debug.Log("Jumping: " + animator.GetInteger("Jumping"));
			Debug.Log("Action: " + animator.GetInteger("Action"));
			Debug.Log("Talking: " + animator.GetInteger("Talking"));
			Debug.Log("Velocity X: " + animator.GetFloat("Velocity X"));
			Debug.Log("Velocity Z: " + animator.GetFloat("Velocity Z"));
			Debug.Log("Charge: " + animator.GetFloat("Charge"));
		}

		#endregion

	}
}