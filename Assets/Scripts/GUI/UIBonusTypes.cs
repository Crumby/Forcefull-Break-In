using UnityEngine;
using System.Collections;

public class UIBonusTypes : MonoBehaviour {

    public ShopItems type;
    public UnityEngine.UI.Button buyButton;

    public void changeSelected() {
        MenusLogic.SelectedBonus = type;
        buyButton.interactable = gameData.canUpgrade(type);
    }

}
