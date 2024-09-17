using UnityEngine;

public class SeamlessHorizontalScroll : MonoBehaviour
{
    public RectTransform imageRectTransform; // Drag the RectTransform of the image here
    public float speed = 300f; // Speed at which the image moves

    private RectTransform duplicateImageRectTransform;
    private float imageWidth;
    private Camera mainCamera;
    private bool movingRight = true;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Calculate image width
        imageWidth = imageRectTransform.rect.width;

        // Create a duplicate image for seamless scrolling
        CreateDuplicateImage();
    }

    void Update()
    {
        // Move both the original and duplicate images horizontally
        MoveImages();
    }

    private void CreateDuplicateImage()
    {
        // Create a new GameObject for the duplicate image
        GameObject duplicateImage = new GameObject("DuplicateImage");
        duplicateImage.transform.SetParent(imageRectTransform.transform.parent);
        duplicateImage.transform.SetSiblingIndex(imageRectTransform.GetSiblingIndex() + 1);

        // Copy the RectTransform component
        duplicateImageRectTransform = duplicateImage.AddComponent<RectTransform>();
        duplicateImageRectTransform.sizeDelta = imageRectTransform.sizeDelta;
        duplicateImageRectTransform.anchorMin = imageRectTransform.anchorMin;
        duplicateImageRectTransform.anchorMax = imageRectTransform.anchorMax;
        duplicateImageRectTransform.pivot = imageRectTransform.pivot;
        duplicateImageRectTransform.anchoredPosition = new Vector2(imageRectTransform.anchoredPosition.x + imageWidth, imageRectTransform.anchoredPosition.y);

        // Copy the image component
        var imageComponent = imageRectTransform.GetComponent<UnityEngine.UI.Image>();
        if (imageComponent != null)
        {
            var duplicateImageComponent = duplicateImage.AddComponent<UnityEngine.UI.Image>();
            duplicateImageComponent.sprite = imageComponent.sprite;
            duplicateImageComponent.color = imageComponent.color;
        }
    }

    private void MoveImages()
    {
        float moveAmount = speed * Time.deltaTime;
        
        if (!movingRight)
        {
            moveAmount = -moveAmount;
        }

        // Move both images horizontally
        imageRectTransform.anchoredPosition += new Vector2(moveAmount, 0);
        duplicateImageRectTransform.anchoredPosition += new Vector2(moveAmount, 0);

        // Get the camera's viewport boundaries in world coordinates
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
        Vector3 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

        float leftEdge = topLeft.x;
        float rightEdge = bottomRight.x;

        // Seamless transition: flip and move direction if off-screen
        if (imageRectTransform.anchoredPosition.x > rightEdge + imageWidth)
        {
            imageRectTransform.anchoredPosition = new Vector2(duplicateImageRectTransform.anchoredPosition.x - imageWidth, imageRectTransform.anchoredPosition.y);
            FlipImage(imageRectTransform);
            FlipImage(duplicateImageRectTransform);
            movingRight = false;
        }

        if (duplicateImageRectTransform.anchoredPosition.x > rightEdge + imageWidth)
        {
            duplicateImageRectTransform.anchoredPosition = new Vector2(imageRectTransform.anchoredPosition.x - imageWidth, duplicateImageRectTransform.anchoredPosition.y);
            FlipImage(imageRectTransform);
            FlipImage(duplicateImageRectTransform);
            movingRight = false;
        }

        if (imageRectTransform.anchoredPosition.x < leftEdge - imageWidth)
        {
            imageRectTransform.anchoredPosition = new Vector2(duplicateImageRectTransform.anchoredPosition.x + imageWidth, imageRectTransform.anchoredPosition.y);
            FlipImage(imageRectTransform);
            FlipImage(duplicateImageRectTransform);
            movingRight = true;
        }

        if (duplicateImageRectTransform.anchoredPosition.x < leftEdge - imageWidth)
        {
            duplicateImageRectTransform.anchoredPosition = new Vector2(imageRectTransform.anchoredPosition.x + imageWidth, duplicateImageRectTransform.anchoredPosition.y);
            FlipImage(imageRectTransform);
            FlipImage(duplicateImageRectTransform);
            movingRight = true;
        }
    }

    private void FlipImage(RectTransform rectTransform)
    {
        // Flip the image horizontally by scaling it by -1 on the X axis
        Vector3 scale = rectTransform.localScale;
        scale.x *= -1;
        rectTransform.localScale = scale;
    }
}
