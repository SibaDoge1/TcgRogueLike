using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace InGameSave
{
    [System.Serializable]
    public class SaveData
    {
        public bool isSaved = false;

        public int floor;
        public int hp;
        public List<int> deckCards;
        public List<int> attainCards;
        public bool pablus;
        public bool xynus;
        public int seed;
    }


    public static class SaveManager
    {

        static string dataPath=Application.persistentDataPath + "/SaveData.dat";

        static SaveData saveData;
        static SaveData SaveData
        {
            get
            {
                if(saveData== null)
                {
                    saveData = Read(dataPath);
                }

                return saveData;
            }
        }

        #region tool
        public static int Floor
        {
            get
            {
                return SaveData.floor;
            }
        }
        public static int Hp
        {
            get
            {
                return SaveData.hp;
            }
        }
        public static List<int> DeckCards
        {
            get { return SaveData.deckCards; }
        }
        public static List<int> AttainCards
        {
            get { return SaveData.attainCards; }
        }
        public static bool Pablus
        {
            get { return SaveData.pablus; }
        }
        public static bool Xynus
        {
            get{return SaveData.xynus;}
        }
        public static int Seed
        {
            get { return SaveData.seed; }
        }

        private static SaveData Read(string path)
        {
            SaveData data;
            using (FileStream SaveFile = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                try
                {
                    data =  binFormatter.Deserialize(SaveFile) as SaveData;
                    Debug.Log("인게임 세이브 로드 성공");
                }
                catch
                {
                    Debug.Log("로드 실패 , 새로운 데이터를 만듭니다.");
                    data = new SaveData();
                    binFormatter.Serialize(SaveFile, data);                    
                }
                finally
                {
                    SaveFile.Close();                    
                }
            }
            return data;
        }

        private static void Save(string path)
        {
            using (FileStream clear = new FileStream(path, FileMode.Create))
            {
                clear.Close();
            }

            using (FileStream WriteFile = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(WriteFile, SaveData);
                
                WriteFile.Close();

                Debug.Log("세이브 완료");
            }
        }
        #endregion



        public static void WriteAndSave(int _floor, int _hp, List<int> _deckCards,
            List<int> _attainCards,int _seed, bool _Pablus, bool _Xynus)
        {
            SaveData.floor = _floor;
            SaveData.hp = _hp;
            SaveData.deckCards = _deckCards;
            SaveData.attainCards = _attainCards;
            SaveData.pablus = _Pablus;
            SaveData.xynus = _Xynus;
            SaveData.isSaved = true;
            saveData.seed = _seed;
            Save(dataPath);
        }

        public static void ClearSaveData()
        {
            saveData = new SaveData();
            Save(dataPath);
        }

        public static bool CheckSaveData()
        {
            if(!SaveData.isSaved)
            {
                return false;
            }
            if (SaveData.floor < 1 || SaveData.floor > 5)
            {
                return false;
            }
            if(SaveData.deckCards == null || SaveData.deckCards.Count != 15)
            {
                return false;
            }
            if(SaveData.attainCards == null)
            {
                return false;
            }

            return true;

        }
    }
}

