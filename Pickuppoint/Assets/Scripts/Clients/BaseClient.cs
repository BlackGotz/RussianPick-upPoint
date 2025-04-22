using UnityEngine;
using TMPro;

public abstract class BaseClient : MonoBehaviour
{
    protected ClientSFX _sfxController;

    public int requiredBoxNumber;
    public float moveSpeed = 2f;
    public float maxPatience = 40f;
    public float currentPatience;
    Animator animator;
    protected enum ClientState { MovingToPickup, Waiting, Leaving }
    protected ClientState state = ClientState.MovingToPickup;

    protected Transform pickupPoint;
    protected Vector3 spawnPosition;
    protected bool isDelivered;

    protected PatienceBar patienceBar;
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("move", true);
        currentPatience = maxPatience;
        isDelivered = false;
        ToggleRecieveTrigger(false);

        _sfxController = GetComponentInChildren<ClientSFX>();

        if (patienceBar == null)
            patienceBar = GetComponentInChildren<PatienceBar>();

        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);

    }

    public void PlayHappySound() => _sfxController?.PlaySound(ClientSoundType.Happy);
    public void PlayAngrySound() => _sfxController?.PlaySound(ClientSoundType.Angry);
    public void PlayOrderSound() => _sfxController?.PlaySound(ClientSoundType.Order);

    protected virtual void Update()
    {
        switch (state)
        {
            case ClientState.MovingToPickup:
                MoveTo(pickupPoint.position);
                if (Vector3.Distance(transform.position, pickupPoint.position) < 0.1f)
                {
                    state = ClientState.Waiting;
                    animator.SetBool("move", false);
                    UIManager.Instance.ShowClientPhone(GetDisplayText());

                    patienceBar.gameObject.SetActive(true);
                    patienceBar.UpdateBar(currentPatience, maxPatience);

                    ToggleRecieveTrigger(true); // �������� �������

                    PlayOrderSound();
                }
                break;

            case ClientState.Waiting:
                currentPatience -= Time.deltaTime;

                if (currentPatience <= 0f)
                {
                    PrepareToLeave();
                    PlayAngrySound();
                }
                else 
                    patienceBar.UpdateBar(currentPatience, maxPatience);

                break;

            case ClientState.Leaving:
                MoveTo(spawnPosition);
                if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
                {
                    // Вызываем метод ClientLeft для создания отзыва
                    if (ClientManager.Instance != null)
                    {
                        ClientManager.Instance.ClientLeft(this, isDelivered);
                    }
                    
                    //      
                    SpawnManager.Instance.ClientFinished(requiredBoxNumber, isDelivered);
                    Destroy(gameObject);
                }
                break;
        }
    }

    protected void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    protected void TurnAround()
    {
        Vector3 direction = (spawnPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    ///      
    /// </summary>
    private void ToggleRecieveTrigger(bool isActive)
    {
        RecieveScript trigger = GetComponentInChildren<RecieveScript>();
        if (trigger != null)
        {
            Collider triggerCollider = trigger.GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = isActive;
                //Debug.Log($"RecieveTrigger {(isActive ? "" : "")}  {gameObject.name}");
            }
        }
    }


    /// <summary>
    ///     ,    Leaving.
    /// </summary>
    protected void PrepareToLeave()
    {
        UIManager.Instance.HidePhoneDisplay();
        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);

        ToggleRecieveTrigger(false); //  

        TurnAround();
        animator.SetBool("move", true);
        state = ClientState.Leaving;
    }

    /// <summary>
    ///     ,    .
    /// </summary>
    protected virtual string GetDisplayText()
    {
        return requiredBoxNumber.ToString();
    }

    public virtual void SetupClient(int boxNumber, Transform targetPoint)
    {
        requiredBoxNumber = boxNumber;
        pickupPoint = targetPoint;
        spawnPosition = transform.position;
    }

    public virtual void ReceiveBox(Box box)
    {
        if (box.boxNumber == requiredBoxNumber)
        {
            Debug.Log("  !");
            PlayHappySound();

            Destroy(box.gameObject);
            isDelivered = true;
            //       
            PrepareToLeave();
        }
        else
        {
            Debug.Log(" ");
            currentPatience -= maxPatience / 4f;
        }


        Debug.Log(box.boxNumber + "   " + requiredBoxNumber);
    }

    public virtual void InstantLeave()
    {
        if (state == ClientState.MovingToPickup)
        {
            state = ClientState.Waiting;
            currentPatience = 0f;
        }
        else
            currentPatience = 0f;
    }
}
