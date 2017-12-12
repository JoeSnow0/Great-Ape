using UnityEngine.UI;
using UnityEngine.EventSystems;

// Inherits from ScrollRect and overrides all methods handling drag so drag is essentially disabled
public class ScrollRectNoDrag : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
}
