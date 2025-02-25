using TMPro;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    private TMP_Text coinText;
    private void Start()
    {
        coinText = gameObject.GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        InventorySlotUI.OnSell += AddCoins;
        InventorySlotUI.OnBuy += RemoveCoins;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnSell -= AddCoins;
        InventorySlotUI.OnBuy -= RemoveCoins;
    }

    public int GetCoins()
    {
        return int.Parse(coinText.text);
    }

    public void AddCoins(int coin)
    {
        var sum = int.Parse(coinText.text) + coin;
        coinText.text = sum.ToString();
    }

    public void RemoveCoins(int coin)
    {
        var sum = int.Parse(coinText.text) - coin;
        coinText.text = sum.ToString();
    }
}
