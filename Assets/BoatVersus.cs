using UnityEngine;

public class BoatVersus : MonoBehaviour
{
    private Rigidbody2D rb;
    public float constantAcceleration = 0.2f; // Constant acceleration when a key is pressed
    public float maxDeceleration = 1f; // Maximum deceleration when stamina is 0
    private float currentDeceleration;
    public float maxSpeed = 5f; // Theoretical maximum speed
    private float currentSpeed = 0f;
    private bool lastKeyPressedLeft = false;
    private bool canMove = false;
    public float passiveDeceleration = 0.1f;

    public bool isPlayerOne = true;

    // Stamina variables
    public float maxStamina = 100f;
    private float currentStamina;

    // Stamina depletion and refill rates
    public float staminaDepletionRate = 10f;
    public float baseRefillRate = 5f;

    public delegate void BoatStateHandler(float speed, float stamina);


    public static event BoatStateHandler OnBoatStateUpdatedPlayer1;
    public static event BoatStateHandler OnBoatStateUpdatedPlayer2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (canMove)
        {
            HandleInput();
            RefillStamina();

            // Calculate deceleration for logging
            float currentDeceleration = CalculateDeceleration(currentStamina);

            // Log current speed, stamina, and deceleration
            Debug.Log("Speed: " + currentSpeed + ", Stamina: " + currentStamina + ", Deceleration: " +
                      currentDeceleration);


            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

            if (isPlayerOne)
            {
                OnBoatStateUpdatedPlayer1?.Invoke(currentSpeed, currentStamina);
            }
            else
            {
                OnBoatStateUpdatedPlayer2?.Invoke(currentSpeed, currentStamina);
            }
        }
    }

    void FixedUpdate()
    {
        currentDeceleration = CalculateDeceleration(currentStamina);
        if (currentSpeed > 0 && !Input.anyKey)
        {
            currentSpeed -= currentDeceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        float currentDynamicDeceleration = CalculateDeceleration(currentStamina);

        if (!Input.anyKey)
        {
            ApplyPassiveDeceleration();
        }
        else
        {
            // Apply dynamic deceleration during player input
            currentSpeed -= currentDynamicDeceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Max(currentSpeed, 0); // Ensure speed doesn't go negative
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    void ApplyPassiveDeceleration()
    {
        // Apply passive deceleration when there is no player input
        currentSpeed -= passiveDeceleration * Time.deltaTime;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    void HandleInput()
    {
        float resistance = CalculateResistance(currentStamina);
        float effectiveAcceleration = constantAcceleration * (1 - resistance);

        // Differentiate input handling based on whether it's Player 1 or Player 2
        if (isPlayerOne)
        {
            HandlePlayerOneInput(effectiveAcceleration);
            OnBoatStateUpdatedPlayer1?.Invoke(currentSpeed, currentStamina);
        }
        else
        {
            HandlePlayerTwoInput(effectiveAcceleration);
            OnBoatStateUpdatedPlayer2?.Invoke(currentSpeed, currentStamina);
        }
    }

    void HandlePlayerOneInput(float effectiveAcceleration)
    {
        // Player 1 controls using Left and Right Arrows
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }

    void HandlePlayerTwoInput(float effectiveAcceleration)
    {
        // Player 2 controls using Z and C keys
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.C) && lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }

    float CalculateResistance(float currentStamina)
    {
        // Example: Inversely proportional to stamina
        return (1 - currentStamina / maxStamina) * maxDeceleration;
    }

    void IncreaseSpeed(float acceleration)
    {
        currentSpeed += acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void DecreaseStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    void RefillStamina()
    {
        float speedFactor = 1 - (currentSpeed / maxSpeed); // Inverse relation to speed
        float modifiedRefillRate = baseRefillRate * speedFactor;

        currentStamina += modifiedRefillRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    float CalculateDeceleration(float stamina)
    {
        float normalizedStamina = stamina / maxStamina;
        return maxDeceleration * (1 - Mathf.Pow(normalizedStamina, 1.5f));
    }
    
    void FinishRace()
    {
        UIManagerVersus uiManager = FindObjectOfType<UIManagerVersus>();
        if (uiManager != null && !uiManager.RaceFinished)
        {
            float raceTime = Time.time - uiManager.StartTime;
            uiManager.FinishTimer(isPlayerOne, raceTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("FinishLinePlayer1") && isPlayerOne) || 
            (other.gameObject.CompareTag("FinishLinePlayer2") && !isPlayerOne))
        {
            FinishRace();
        }
       
    }
}

        
    
    