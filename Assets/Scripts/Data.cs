using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChickenOut
{
    public static class Data
    {
        public static void SaveProfile(ProfileData _profile)
        {
            try
            {
                string path = Application.persistentDataPath + "/profile.dt"; //The path of the game in user's Computer //the profile.dt the folder we deside to saveprofile in.

                if (File.Exists(path)) File.Delete(path);

                FileStream file = File.Create(path);

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, _profile);
                file.Close();
            }
            catch
            {
                Debug.LogError("SOMETHING WENT WRONG!!");
            }
        }

        public static ProfileData LoadProfile()
        {
            ProfileData ret = new ProfileData();
            try
            {
                string path = Application.persistentDataPath + "/profile.dt";

                if (File.Exists(path))
                {
                    FileStream file = File.Open(path, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    ret = (ProfileData)bf.Deserialize(file);
                }
            }
            catch
            {
                Debug.LogError("FILE ERROR");
            }

            return ret;
        }
    }
}