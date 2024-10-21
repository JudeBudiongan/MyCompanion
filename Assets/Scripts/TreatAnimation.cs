using UnityEngine;

public class TreatAnimation : MonoBehaviour
{
    public RectTransform foodSprite; // The sprite of the treat (food) to animate
    public Transform companionTarget; // The companion's target position (destination)
    public float animationDuration = 1.0f; // Duration of the animation

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isAnimating = false;
    private float animationProgress = 0f;

    // Call this method to start the animation
    public void StartAnimation(Vector3 buttonPosition)
    {
        startPosition = buttonPosition; // Start at the button's position
        targetPosition = companionTarget.position; // End at the companion's position
        isAnimating = true;
        animationProgress = 0f; // Reset animation progress
    }

    void Update()
    {
        if (isAnimating)
        {
            // Move the food sprite in a straight line from button to companion
            animationProgress += Time.deltaTime / animationDuration;
            foodSprite.position = Vector3.Lerp(startPosition, targetPosition, animationProgress);

            // Stop animating when the progress reaches 1 (end of animation)
            if (animationProgress >= 1f)
            {
                isAnimating = false;
            }
        }
    }
}
