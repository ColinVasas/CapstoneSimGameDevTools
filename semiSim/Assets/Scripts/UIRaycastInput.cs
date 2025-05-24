using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class UIRaycastInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private float maxRaycastDistance = 10f;
    [SerializeField] private LayerMask uiLayerMask;
    
    [Header("Cursor Settings")]
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private float cursorDistanceFromSurface = 0.01f;
    [SerializeField] private float cursorScale = 0.02f;
    
    private Camera mainCamera;
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;
    private GraphicRaycaster[] graphicRaycasters;
    
    // Tracking hover state
    private GameObject currentHoveredObject;
    private GameObject previousHoveredObject;
    private GameObject currentPressedObject;
    
    // Cursor object
    private GameObject cursor;
    private bool isCursorActive = false;
    
    private void Awake()
    {
        // Get required components
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
            
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene! Please add one.");
            enabled = false;
            return;
        }
        
        // Find all GraphicRaycasters (one per canvas)
        graphicRaycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
        
        raycastResults = new List<RaycastResult>();
        
        // Create cursor if prefab is assigned
        if (cursorPrefab != null)
        {
            cursor = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
            cursor.transform.localScale = Vector3.one * cursorScale;
            cursor.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No cursor prefab assigned. Cursor will not be shown.");
        }
    }
    
    private void OnEnable()
    {
        if (clickAction == null || clickAction.action == null) return;
        
        clickAction.action.performed += OnClickPerformed;
        clickAction.action.canceled += OnClickCanceled;
        clickAction.action.Enable();
    }
    
    private void OnDisable()
    {
        if (clickAction == null || clickAction.action == null) return;
        
        clickAction.action.performed -= OnClickPerformed;
        clickAction.action.canceled -= OnClickCanceled;
        clickAction.action.Disable();
        
        // Clean up any active pointer states
        ResetHoverState();
    }
    
    private void Update()
    {
        // Update raycasting every frame for hover detection
        PerformRaycast();
    }
    
    private void PerformRaycast()
    {
        // Use the center of the screen as interaction point for first-person view
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
        // Create PointerEventData for the screen center
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = screenCenterPoint
        };

        // Clear previous results
        raycastResults.Clear();
        
        var ray = mainCamera.ScreenPointToRay(screenCenterPoint);
        
        // Reset hover state tracking for this frame
        previousHoveredObject = currentHoveredObject;
        currentHoveredObject = null;
        
        // Perform 3D raycast for world space UI
        var physicsRaycastHit = Physics.Raycast(ray, out var hit, maxRaycastDistance, uiLayerMask);
        
        if (physicsRaycastHit)
        {
            // Check if hit object has any UI component
            var button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                currentHoveredObject = button.gameObject;
                UpdateCursorPosition(hit.point, hit.normal);
                
                // We found a UI element, no need to check canvases
                UpdateHoverState();
                return;
            }
        }
        
        foreach (var raycaster in graphicRaycasters)
        {
            // Check canvas render mode
            var canvas = raycaster.GetComponent<Canvas>();
            if (canvas.renderMode != RenderMode.WorldSpace)
            {
                continue; // Skip non-world space canvases
            }
            
            // Raycast through this specific canvas
            raycastResults.Clear();
            raycaster.Raycast(pointerEventData, raycastResults);

            if (raycastResults.Count <= 0) continue;
            
            // We hit something in this canvas
            currentHoveredObject = raycastResults[0].gameObject;
            
            // Position cursor at hit position
            if (cursor != null)
            {
                // Get the world position and normal from raycast result
                Vector3 worldPos = raycastResults[0].worldPosition;
                Vector3 worldNormal = raycastResults[0].worldNormal;
                UpdateCursorPosition(worldPos, worldNormal);
            }
            
            // We found a UI element, no need to check more canvases
            break;
        }
        
        // Update hover state based on what we found (or didn't find)
        UpdateHoverState();
    }
    
    private void UpdateCursorPosition(Vector3 hitPoint, Vector3 normal)
    {
        if (cursor == null) return;
        
        // Position slightly in front of the hit surface
        cursor.transform.position = hitPoint + normal * cursorDistanceFromSurface;
        
        // Orient cursor to face the surface normal
        cursor.transform.forward = normal;
        
        // Show cursor if it's not already active
        if (!isCursorActive)
        {
            cursor.SetActive(true);
            isCursorActive = true;
        }
    }
    
    private void HideCursor()
    {
        if (cursor != null && isCursorActive)
        {
            cursor.SetActive(false);
            isCursorActive = false;
        }
    }
    
    private void UpdateHoverState()
    {
        // If we're not hovering over anything, hide the cursor
        if (currentHoveredObject == null)
        {
            HideCursor();
            
            // If we were previously hovering over something, exit it
            if (previousHoveredObject != null)
            {
                ExecuteEvents.Execute(previousHoveredObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                
                // If we also had it pressed, release it
                if (currentPressedObject == previousHoveredObject)
                {
                    ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                    currentPressedObject = null;
                }
            }
            return;
        }
        
        // If we're hovering over a new object
        if (currentHoveredObject != previousHoveredObject)
        {
            // Exit previous object if there was one
            if (previousHoveredObject != null)
            {
                ExecuteEvents.Execute(previousHoveredObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                
                // If we also had it pressed, release it
                if (currentPressedObject == previousHoveredObject)
                {
                    ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                    currentPressedObject = null;
                }
            }
            
            // Enter new object
            ExecuteEvents.Execute(currentHoveredObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
        }
    }
    
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        // If we're hovering over a UI element, handle pointer down
        if (currentHoveredObject != null)
        {
            // Handle pointer down
            ExecuteEvents.Execute(currentHoveredObject, pointerEventData, ExecuteEvents.pointerDownHandler);
            currentPressedObject = currentHoveredObject;
        }
    }
    
    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        // If we had a pressed object and it's the same as the current hovered object, complete the click
        if (currentPressedObject != null && currentHoveredObject == currentPressedObject)
        {
            // Handle pointer up and click
            ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            
            // For Button components, also directly invoke the onClick event
            var button = currentPressedObject.GetComponent<Button>();
            if (button != null && button.interactable)
            {
                button.onClick.Invoke();
            }
            else
            {
                // Try to find a button in parent
                var parentButton = currentPressedObject.GetComponentInParent<Button>();
                if (parentButton != null && parentButton.interactable)
                {
                    parentButton.onClick.Invoke();
                }
            }
        }
        else if (currentPressedObject != null)
        {
            // If we're no longer hovering over the pressed object, just do pointer up without click
            ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
        }
        
        // Reset pressed state
        currentPressedObject = null;
    }
    
    private void ResetHoverState()
    {
        // Clean up any active pointer states when disabled
        if (previousHoveredObject != null)
        {
            ExecuteEvents.Execute(previousHoveredObject, pointerEventData, ExecuteEvents.pointerExitHandler);
            previousHoveredObject = null;
        }
        
        if (currentPressedObject != null)
        {
            ExecuteEvents.Execute(currentPressedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
            currentPressedObject = null;
        }
        
        currentHoveredObject = null;
        HideCursor();
    }
}