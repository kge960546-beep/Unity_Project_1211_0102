using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;

public class GoldService : MonoBehaviour, IGameManagementService
{
    //Maintain 16 bytes
    //반드시 16글자 유지
    private static readonly byte[] key = Encoding.UTF8.GetBytes("MySecretKey12345");
    private static readonly byte[] initializationVector = Encoding.UTF8.GetBytes("InitiVector12345");

    public static GoldService instance;

    private int savedGold;
    private int earnedGold;

    public int SavedGold => savedGold;
    public int EarnedGold => earnedGold;

    private event Action<int, int> onGoldAmountChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;       

        LoadGold();
    }
    public void SubscribeGoldAmountChanged(Action<int, int> action)
    {
        onGoldAmountChanged += action;
    }
    public void UnsubscribeGoldAmountChanged(Action<int, int> action)
    {
        onGoldAmountChanged -= action;
    }
    public void GetGold(GoldType goldType)
    {
        int amount = 0;
        switch(goldType)
        {
            case (GoldType.gold_1):
                amount = 1;
                break;
            case (GoldType.gold_5):
                amount = 5;
                break;
            case (GoldType.gold_10):
                amount = 10;
                break;                
        }
        earnedGold += amount;
        savedGold += amount;        

        onGoldAmountChanged?.Invoke(savedGold, earnedGold);
    }
    public bool UseGold(int amount)
    {
        if(savedGold < amount)
        {
            //골드 부족 UI SetActive(true)
            return false;
        }

        savedGold -= amount;
        SaveGold();
        //로비쪽에서는 earnedGoldText를 null로 두면 무시됨        
        onGoldAmountChanged?.Invoke(savedGold, earnedGold);

        return true;
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveGold();
        }
    }
    private void OnDisable()
    {
        SaveGold();
    }
    void SaveGold()
    {
        if (instance != this) return;
        SaveEncryptedData("MyGold", savedGold.ToString());
    }
    void LoadGold()
    {
        string data = LoadEncryptedData("MyGold");
        if(!string.IsNullOrEmpty(data))
        {
            int.TryParse(data, out savedGold);
        }

        onGoldAmountChanged?.Invoke(savedGold, earnedGold);
    }   
    public static void SaveEncryptedData(string keyName, string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = initializationVector;

            //Create an encryptor to perform encryption using the encryption key and initialization vector.
            //암호화 키와 초기화 벡터를 이용하여, 암호화를 진행할 encryptor 생성        
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encryptedData = null;

            //Encrypt plain data
            //일반 데이터를 암호화
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            //Convert encrypted data to a string and store it
            //암호화 데이터를 문자열로 변환하여 저장
            string encryptedString = Convert.ToBase64String(encryptedData);

            Debug.Log($"원본-> {data}, 암호화 -> {encryptedString}");

            PlayerPrefs.SetString(keyName, encryptedString);
            PlayerPrefs.Save();
        }
    }
    public static string LoadEncryptedData(string keyName)
    {
        string encryptedString = PlayerPrefs.GetString(keyName);
        if(!string.IsNullOrEmpty(encryptedString))
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = initializationVector;

                //Create a decryptor to perform decryption using the encryption key and initialization vector.
                //암호화 키와 초기화 벡터를 이용하여 복호화를 진행할 decryptor 생성
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                //data decryption
                //데이터 복호화
                byte[] encryptedData = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                //Return stored data using decrypted data
                //복호화된 데이터를 이용하여 저장된 데이터 반환
                return Encoding.UTF8.GetString(decryptedData);
            }           
        }
        else
        {
            return null;
        }
    }
}