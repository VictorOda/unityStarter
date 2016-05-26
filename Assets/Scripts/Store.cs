using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	public static Store instance;

	[Header ("Items")]
	public GameObject[] items; 				// Items of the store
	public int[] prices;				// Price of each item
	public string[] itemsIds;			// Ids of each item, used by the ShopController.cs

	[Header ("UI Elements")]
	public Button buyButton;				// Button to buy the items
	public Button leftButton, rightButton;	// Buttons to go to the previous and next items

	// Script private variables
	private int selectedItem;
	private Text txtBuyButton;

	void Start () {
		// Set singleton
		if(!instance)
			instance = this;

		// Get elements
		txtBuyButton = buyButton.gameObject.transform.GetChild(0).GetComponent<Text>();

		// Set values
		selectedItem = 0;

		UpdateUI();
	}

	public void ChangeItem(bool next) {
		if(next) {
			// Go to next item
			selectedItem++;
			items[selectedItem].SetActive(true);		// Activate selected item
			items[selectedItem - 1].SetActive(false);	// Deactivate previous item
		} else {
			// Go to previous item
			selectedItem--;
			items[selectedItem].SetActive(true);		// Activate selected item
			items[selectedItem + 1].SetActive(false);	// Deactivate previous item
		}

		UpdateUI();	// Update UI according to selected item
	}

	public void UpdateUI() {
		if(selectedItem == 0) {
			// If it's the first item...
			leftButton.interactable = false; // Deactivate left button
			rightButton.interactable = true; // Activate right button
		} else if(selectedItem >= items.Length - 1) {
			// If it's the last item...
			leftButton.interactable = true; // Activate left button
			rightButton.interactable = false; // Deactivate right button
		} else {
			leftButton.interactable = true; // Activate left button
			rightButton.interactable = true; // Activate right button
		}

		// Update buy button's price
		txtBuyButton.text = "$" + prices[selectedItem].ToString();
	}

	public void BuyItem() {
		ShopController.instance.BuyProductID(itemsIds[selectedItem]);
	}

	public void Back() {
		SceneController.instance.ChangeScene("Menu");
	}
}
