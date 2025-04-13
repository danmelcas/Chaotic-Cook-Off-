// Interface for RayCastHit interactions 
interface IInteractable { 
    // Any interactable object should have their own OnInteraction methods, and OnHoverEnter and OnHoverExit methods 
    public void OnInteraction(); 
    public void OnHover(); 
    public void OnHoverExit(); 
}