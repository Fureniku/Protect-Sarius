using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float maxShields = 10.0f;
    [SerializeField] private int maxLives = 3;

    private float shields;
    private int immunity;

    private Camera cam;

    //UI Controls
    private Slider throttle;
    private Joystick yawPitchStick;
    private Joystick rollStick;
    
    private GameObject respawnPoint;

    //Properties for shooting
    [SerializeField] private GameObject bulletObject = null;
    [SerializeField] private int bulletCooldown = 15;
    private bool bulletLoaded = true;
    private int bulletCooldownTimer;
    private GameObject gunLeft;
    private GameObject gunRight;
    private bool fireButtonDown;

    //Properties for respawning
    private bool respawning;
    private int respawnTimer;
    private const int respawnTime = 180;
    private Vector3 lockedRotate;
    private Quaternion lockedAngle;
    private float lockedThrottle;
    
    //Set up GameOver event for losing game
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    //Particles & Sounds
    private ParticleSystem engineLeft;
    private ParticleSystem engineMid;
    private ParticleSystem engineRight;

    private AudioSource engineSound;
    
    //Initialize everything
    void Start() {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        throttle = GameObject.Find("Throttle").GetComponent<Slider>();

        respawnPoint = GameObject.Find("RespawnPoint");
        
        yawPitchStick = GameObject.Find("YawPitchStick")?.GetComponent<Joystick>();
        rollStick = GameObject.Find("RollStick")?.GetComponent<Joystick>();

        gunLeft = GameObject.Find("GunLeft");
        gunRight = GameObject.Find("GunRight");
        
        //Make sure player can't shoot themselves
        Physics.IgnoreLayerCollision(8, 10);

        shields = maxShields;

        GameObject engineMidObj = GameObject.Find("Engine_mid");
        
        engineLeft = GameObject.Find("Engine_left").GetComponent<ParticleSystem>();
        engineMid = engineMidObj.GetComponent<ParticleSystem>();
        engineRight = GameObject.Find("Engine_right").GetComponent<ParticleSystem>();

        engineSound = engineMidObj.GetComponent<AudioSource>();

        //Do this after the fighter initializes so we can dynamically disable them without losing access to them
        GameObject.Find("SticksController").GetComponent<SticksController>().UpdateControlStatus();
    }

    //Button input can be a bit weird in fixedupdate due to frame variances. Update is much more reliable.
    //This is only for hardware input; on-screen fire/menu buttons have direct calls to their actions.
    void Update() {
        if (Input.GetButtonDown("Shoot") || Input.GetAxisRaw("Shoot") != 0) fireButtonDown = true;
        if (Input.GetButtonUp("Shoot") || Input.GetAxisRaw("Shoot") == 0) fireButtonDown = false;

        if (Input.GetButtonDown("ResetShip")) {
            StabilizePlayer();
        }
        
        if (Input.GetButtonDown("Menu")) {
            GameObject menuBtn = GameObject.Find("MenuButton");
            if (menuBtn != null) {
                Cursor.lockState = CursorLockMode.None;
                GameObject.Find("MenuButton").GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    //Updates both the engine flare particles, and the sound made by the engine
    private void engineSpeed(ParticleSystem engine) {
        float engineSpeed = throttle.value / throttle.maxValue;
        engineSound.pitch = 0.5f + (engineSpeed * 2f);
        ParticleSystem.MainModule main = engine.main;
        main.startSpeed = (engineSpeed * 3);
    }

    private void Awake() {
        GrabMouse();
    }

    public void GrabMouse() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Handle most of the input and physics controls in here
    private void FixedUpdate() {

        float throttleCurrent = throttle.value;
        int invert = (PlayerPrefs.GetInt("invertPitch", 0) != 0)  ? -1 : 1; //0 != 0 is false
        float pitch = Input.GetAxis("Pitch");
        float roll = Input.GetAxis("Roll");
        float yaw = Input.GetAxis("Yaw");
        float throttleControl = Input.GetAxis("Throttle"); //Only used for hardware control

        engineSpeed(engineLeft);
        engineSpeed(engineMid);
        engineSpeed(engineRight);

        float pitchSens = PlayerPrefs.GetFloat("pitchSens", 4);
        float yawSens = PlayerPrefs.GetFloat("yawSens", 2);
        float rollSens = PlayerPrefs.GetFloat("rollSens", 4);

        //Workaround for Unity's joysticks not working in 19.1.6f1
        //On-screen controls override hardware input controls.
        if (rollStick != null && rollStick.isEnabled() && rollStick.getXVal() != 0) {
            roll = rollStick.getXVal();
        }

        if (yawPitchStick != null && yawPitchStick.isEnabled()) {
            if (yawPitchStick.getXVal() != 0 && yaw == 0) {
                yaw = -yawPitchStick.getXVal();
            }
        
            if (yawPitchStick.getYVal() != 0 && pitch == 0) {
                pitch = yawPitchStick.getYVal();
            }
        }
        
        //Handle hardware input for throttle controls
        if (throttleControl > 0) {
            if (throttle.value + throttleControl < throttle.maxValue) {
                throttle.value += throttleControl;
            }
            else {
                throttle.value = throttle.maxValue;
            }
        } else if (throttleControl < 0) {
            if (throttle.value + throttleControl > throttle.minValue) {
                throttle.value += throttleControl;
            }
            else {
                throttle.value = throttle.minValue;
            }
        }

        if (respawning) { //Let the ship "spin out" for a few seconds after death
            if (respawnTimer == 0) { //On the first tick get the current controls, and lock them to give a consistent rotation as it spins out
                lockedRotate = new Vector3(pitch * pitchSens, yaw * yawSens, (roll * rollSens) * invert);
                lockedAngle = rb.transform.rotation;
                lockedThrottle = throttle.value;

                cam.transform.parent = null; //Let the camera watch the player's ship fly off into the distance
            }

            respawnTimer++;

            rb.velocity = (lockedAngle * Vector3.forward) * (baseSpeed + lockedThrottle);
            transform.Rotate(lockedRotate, Space.Self);

            if (respawnTimer >= respawnTime) { //Once the timer is reached, respawan.
                RespawnPlayer();
            }
        }
        else { //Normal movement
            Vector3 movement = rb.transform.rotation * Vector3.forward;
            rb.velocity = movement * (baseSpeed + throttleCurrent);

            transform.Rotate(pitch * pitchSens * invert, yaw * yawSens, (roll * rollSens), Space.Self);
            
            //Reload the gun, if needed.
            if (!bulletLoaded && bulletCooldownTimer < bulletCooldown) {
                bulletCooldownTimer++;
            }
            else {
                bulletLoaded = true;
                bulletCooldownTimer = 0;
            }
            
            
            if (fireButtonDown) FireBullet();
        }

        //Immunity period will tick down if it's above 0. Means we can just set immunity to whatever we like to give immunity.
        if (immunity > 0) {
            immunity--;
        }
    }

    //Called when the bullet button is down
    public void FireBullet() {
        if (bulletLoaded) {
            //Initially create with the parents as guns to quickly and easily ensure correct rotations etc
            //Positional adjustments are to account for creating the bullet at very high speeds - without them by the time the bullet appears the ship has moved and it spawns behind it.
            Vector3 posAdjustment = (rb.transform.rotation * Vector3.forward) * (throttle.value / 50f);
            GameObject bulletLeft = Instantiate(bulletObject, gunLeft.transform.position + posAdjustment, gunLeft.transform.rotation, gunLeft.transform);
            GameObject bulletRight = Instantiate(bulletObject, gunRight.transform.position  + posAdjustment, gunRight.transform.rotation, gunRight.transform);

            //But the prefab faces the wrong way by default, so fix that too :)
            bulletLeft.transform.Rotate(90f, 0f, 0f);
            bulletRight.transform.Rotate(90f, 0f, 0f);

            bulletLeft.transform.parent = null;
            bulletRight.transform.parent = null;
            
            //Fire the bullet. BulletController takes over from here.
            bulletLeft.GetComponent<BulletController>().AddBulletSpeed(throttle.value);
            bulletRight.GetComponent<BulletController>().AddBulletSpeed(throttle.value);

            AudioSource gunSoundLeft = gunLeft.GetComponent<AudioSource>();
            AudioSource gunSoundRight = gunRight.GetComponent<AudioSource>();
            
            //Play the pew pew sound
            gunSoundLeft.PlayOneShot(gunSoundLeft.clip);
            gunSoundRight.PlayOneShot(gunSoundRight.clip);
            
            //Prepare for reloading
            bulletLoaded = false;
        }
    }

    //Hurt the player. Called from asteroid/mothership collisions.
    public void DamagePlayer(float amt) {
        if (immunity == 0) {
            shields -= amt;

            if (shields <= 0f) {
                KillPlayer();
            }

            immunity = 60; //Give them a second of immunity to avoid double hits
        }
    }
    
    //Pretty self-explanitory.
    public void KillPlayer() {
        if (maxLives > 0) {
            maxLives--;
            respawning = true;
            if (maxLives == 2) {
                GameObject.Find("Life3").SetActive(false);
            }
            if (maxLives == 1) {
                GameObject.Find("Life2").SetActive(false);
            }
            if (maxLives == 0) {
                GameObject.Find("Life1").SetActive(false);
            }
        }
        else {
            //Call the Game Over event
            if (OnGameOver != null) {
                OnGameOver.Invoke();
            }
        }
    }

    //Set the player back to their spawn point (attached above the mothership)
    //Reset all their properties and movement
    public void RespawnPlayer() {
        respawning = false;
        respawnTimer = 0;
        transform.position = respawnPoint.transform.position;
        transform.rotation = respawnPoint.transform.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        cam.transform.parent = transform;
        cam.transform.localPosition = new Vector3(0, 0.035f, -0.1f);
        cam.transform.rotation = Quaternion.identity;
        throttle.value = 0f;

        shields = maxShields;
    }
    
    //Tap the ship itself to stabilize, in case a collision causes the player to spin out
    public void StabilizePlayer() {
        if (!respawning) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    //Used by UI
    public float GetHealth() {
        return shields;
    }

    public float GetMaxHealth() {
        return maxShields;
    }
}
