namespace GoFigure.App.ViewModels.Interfaces
{
  public interface ISolutionViewModel
  {
    bool ControlsEnabled { get; set; }
    
    int CurrentSlotIndex { get; }
    
    string Slot1 { get; }
    
    string Slot2 { get; }
    
    string Slot3 { get; }
    
    string Slot4 { get; }
    
    string Slot5 { get; }
    
    string Slot6 { get; }
    
    string Slot7 { get; }
    
    string SlotBackground { get; set; }
    
    int SolutionResult { get; }

    void SetSlotIndex(int index);
  }
}