using UnityEngine;
using System.Collections;

public class UIShopMenus : MonoBehaviour
{

    public UnityEngine.UI.Button[] buttonsToSlide;
    public UnityEngine.RectTransform panel;
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
        panel.gameObject.SetActive(true);
        float toMove = -panel.rect.height;
        foreach (var button in buttonsToSlide)
        {
            button.transform.position = new Vector3(button.transform.position.x,
                button.transform.position.y + toMove + button.GetComponent<UnityEngine.RectTransform>().rect.height * 2.5f,
                button.transform.position.z);
        }
        openedTab = this;
    }

    private void SlideUp()
    {
        panel.gameObject.SetActive(false);
        float toMove = panel.rect.height;
        foreach (var button in buttonsToSlide)
        {
            button.transform.position = new Vector3(button.transform.position.x,
                button.transform.position.y + toMove - button.GetComponent<UnityEngine.RectTransform>().rect.height * 2.5f,
                button.transform.position.z);
        }
        openedTab = null;
    }
}
