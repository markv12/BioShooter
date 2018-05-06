using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    private float health = 10;
    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            GameManager.instance.DisplayHealth(health);
            if(health <= 0) {
                GameManager.instance.RestartGame();
            }
        }
    }

    private float score =  0;
    public float Score {
        get {
            return score;
        }
        set {
            score = value;
            GameManager.instance.DisplayScore(score);
        }
    }

    public Transform bodyTransform;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	float horizontalInput;
	bool wallSliding;
	int wallDirX;

    public static Player p;

	void Awake() {
        p = this;
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() {
        if (GameManager.gameRunning) {
            CalculateVelocity();
            HandleWallSliding();

            controller.Move(velocity * Time.deltaTime, new Vector2(horizontalInput, 0));

            if (controller.collisions.above || controller.collisions.below) {
                if (controller.collisions.slidingDownMaxSlope) {
                    velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                } else {
                    velocity.y = 0;
                }
            }
        }
	}

    private DirectionState dState = DirectionState.initial;
    public void SetHorizontalInput (float input) {
		horizontalInput = input;
        if (input > 0) {
            if (dState != DirectionState.right) {
                bodyTransform.localScale = new Vector3(1, 1, 1);
                dState = DirectionState.right;
            }
        } else if(input < 0) {
            if (dState != DirectionState.left) {
                bodyTransform.localScale = new Vector3(-1, 1, 1);
                dState = DirectionState.left;
            }
        }
    }

	public void OnJumpInputDown() {
		if (wallSliding) {
			if (wallDirX == horizontalInput) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (horizontalInput == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (horizontalInput != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}
		

	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (horizontalInput != wallDirX && horizontalInput != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = horizontalInput * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
