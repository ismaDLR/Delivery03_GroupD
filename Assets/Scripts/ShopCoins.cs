using TMPro;
using UnityEngine;

public class ShopCoins : MonoBehaviour
{
    private TMP_Text coinText;

    private void Start()
    {
        coinText = gameObject.GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        InventorySlotUI.OnSell += RemoveCoins;
        InventorySlotUI.OnBuy += AddCoins;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnSell += RemoveCoins;
        InventorySlotUI.OnBuy += AddCoins;
    }

    public int GetCoins()
    {
        return int.Parse(coinText.text);
    }

    public void AddCoins(int coin)
    {
        int sum = int.Parse(coinText.text) + coin;
        coinText.text = sum.ToString();
    }

    public void RemoveCoins(int coin)
    {
        Debug.Log(coinText.text);
        Debug.Log(coin);
        int sum = int.Parse(coinText.text) - coin;
        coinText.text = sum.ToString();
    }
}
