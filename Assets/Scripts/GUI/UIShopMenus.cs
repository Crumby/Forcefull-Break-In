using UnityEngine;
using System.Collections;

public class UIShopMenus : MonoBehaviour
{

    public UnityEngine.UI.Button[] buttonsToSlide;
    //public GameObject panel;
    public GameObject panel;
    private static UIShopMenus openedTab = null;

    public void SlideDown()
    {
        if (openedTab == this)
        {
            SlideUp();
            return;
        }
        if (openedTab != null)
            openedTab.SlideUp();
        panel.SetActive(true);
        float toMove = -panel.GetComponent<RectTransform>().rect.height * panel.GetComponent<RectTransform>().localScale.y;
        Debug.Log("O " + toMove);
        foreach (var button in buttonsToSlide)
        {
            button.transform.position = new Vector3(button.transform.position.x,
                button.transform.position.y + toMove,
                button.transform.position.z);
        }
        openedTab = this;
    }

    private void SlideUp()
    {
        panel.SetActive(false);
        float toMove = panel.GetComponent<RectTransform>().rect.height * panel.GetComponent<RectTransform>().localScale.y;
        Debug.Log(toMove);
        foreach (var button in buttonsToSlide)
        {
            button.transform.position = new Vector3(button.transform.position.x,
                button.transform.position.y + toMove,
                button.transform.position.z);
        }
        openedTab = null;
    }
}
