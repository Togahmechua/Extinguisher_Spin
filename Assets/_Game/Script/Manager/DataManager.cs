using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<AchivementType, int> achivementData = new Dictionary<AchivementType, int>();

    [SerializeField] private int money;
    [SerializeField] private int record;
    [SerializeField] private int foamCount;

    [SerializeField] private int foamPower;
    [SerializeField] private int firstShot;

    private void OnEnable()
    {
        achivementData[AchivementType.Money] = PlayerPrefs.GetInt("Money", 500);
        achivementData[AchivementType.Record] = PlayerPrefs.GetInt("Record", 120);
        achivementData[AchivementType.FoamCount] = PlayerPrefs.GetInt("FoamCount", 10);
        achivementData[AchivementType.Power] = PlayerPrefs.GetInt("Power", 10);
        achivementData[AchivementType.FirstShotPower] = PlayerPrefs.GetInt("FirstShotPower", 10);

        money = Get<int>(AchivementType.Money);
        record = Get<int>(AchivementType.Record);
        foamCount = Get<int>(AchivementType.FoamCount);

        foamPower = Get<int>(AchivementType.Power);
        firstShot = Get<int>(AchivementType.FirstShotPower);
    }

    public T Get<T>(AchivementType type)
    {
        if (typeof(T) == typeof(int))
        {
            return (T)(object)achivementData[type];
        }

        Debug.LogWarning($"Unsupported type {typeof(T)} for Get");
        return default;
    }

    public void Set(AchivementType type, int value)
    {
        achivementData[type] = value;
        PlayerPrefs.SetInt(type.ToString(), value);
        PlayerPrefs.Save();

        switch (type)
        {
            case AchivementType.Money:
                money = value;
                break;
            case AchivementType.Record:
                record = value;
                break;
            case AchivementType.FoamCount:
                foamCount = value;
                break;
            case AchivementType.Power:
                foamPower = value;
                break;
            case AchivementType.FirstShotPower:
                firstShot = value;
                break;
        }
    }
}

public enum AchivementType
{
    Money,
    Record,
    FoamCount,
    Power,
    FirstShotPower
}
