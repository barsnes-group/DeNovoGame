using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBox : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	public Transform parentToReturnTo = null;

	public void SetScale(float scale_x, float scale_y)
    {
        this.transform.localScale = new Vector3(scale_x, scale_y, 1);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{

		parentToReturnTo = this.transform.parent;
		this.transform.SetParent(this.transform.parent.parent);

		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//Debug.Log("OnEndDrag");
		//ref til parent, lag ny clone
		//clonen sin parent er boxcontainer

		this.transform.SetParent(parentToReturnTo);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}