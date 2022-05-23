using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReticle : MonoBehaviour
{
    [SerializeField]
    private Puzzle_Element aimerElement;

    private GridTile currentTile;

    private SpriteRenderer sr;

    [SerializeField]
    private Sprite pullReticle;
    [SerializeField]
    private Sprite releaseReticle;

    [SerializeField]
    private InteractionMode interactionMode;

    [SerializeField]
    private List<InteractionMode> availableInteractions = new List<InteractionMode>() { InteractionMode.None };

    [SerializeField]
    private int currentInteraction;

    public Puzzle_Element AimerElement
    {
        get { return aimerElement; }
    }

    public GridTile CurrentTile
    {
        get { return currentTile; }
    }

    public InteractionMode InteractionMode
    {
        get { return interactionMode; }
        set
        {
            if (interactionMode != value)
                SwitchReticleSprite(value);
            interactionMode = value;
        }
    }

    public List<InteractionMode> AvailableInteractions
    {
        get { return availableInteractions; }
        set { availableInteractions = value; }
    }

    public int CurrentInteraction
    {
        get { return currentInteraction; }
        set { currentInteraction = value; }
    }

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.enabled = true;

        SwitchReticleSprite(InteractionMode);
    }

    void Start()
    {
        GameManager.Instance.InteractionHandler.Reticles.Add(this);
        aimerElement.OnChangeDirection += ChangeReticleDirection;
        transform.position = aimerElement.transform.position + (Vector3) aimerElement.StartingDirection;

        UpdateCurrentTile();
    }

    private void ChangeReticleDirection(Vector2 newDirection)
    {
        // Will need to be changed if attached elements are not always grabbed elements
        if (aimerElement.AttachedElements.Count != 1)
            return;

        if (interactionMode != InteractionMode.None)
            StartCoroutine(MoveReticleOverTime(newDirection));
        else
        {
            transform.position = aimerElement.transform.position + (Vector3)newDirection;
            UpdateCurrentTile();
        }

    }

    private IEnumerator MoveReticleOverTime(Vector2 newDirection)
    {
        float timeToFinish = GameSettings.ChangeDirectionDelay;
        Vector3 targetPos = aimerElement.transform.position + (Vector3)newDirection;

        Vector3 diff = transform.position - targetPos;

        float tTaken = 0;

        while (tTaken < timeToFinish)
        {
            if (GameManager.Instance.State != GameState.PauseMenu)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, diff.magnitude / (timeToFinish / Time.deltaTime));
                tTaken += Time.deltaTime;
            }
            yield return null;
        }

        transform.position = aimerElement.transform.position + (Vector3)newDirection;

        UpdateCurrentTile();
    }

    public void UpdateCurrentTile()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.15f, GameLayers.Instance.TileLayer);
        if (colliders.Length > 0)
            currentTile = colliders[0].GetComponent<GridTile>();
        else
            currentTile = null;
    }

    public void SwitchReticleSprite(InteractionMode interactionType)
    {
        if (interactionType == InteractionMode.None)
            sr.sprite = null;
        if (interactionType == InteractionMode.Pull)
            sr.sprite = pullReticle;
        if (interactionType == InteractionMode.Release)
            sr.sprite = releaseReticle;
    }

    private void OnDestroy()
    {
        GameManager.Instance.InteractionHandler.Reticles.Remove(this);
    }
}
