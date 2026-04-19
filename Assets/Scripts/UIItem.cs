using UnityEngine;

public class UIItem : MonoBehaviour
{
    public void Show()
    {
        if(!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if(this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}
