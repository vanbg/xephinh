using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataStream 
{
  public static void Save<T>(T serializedObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName +".dat",FileMode.Create);

        try
        {
            formatter.Serialize(fileStream, serializedObject);
        }
        catch(SerializationException e)
        {
            Debug.Log("Saver file. Error: " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }

    }

    public static bool Exits(string fileName)//kiem tra xem thu muc co ton tai hay k
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";
        return File.Exists(path + fullFileName);
    }

    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Open);
        T returnType = default;

        try
        {
            returnType = (T)formatter.Deserialize(fileStream);
        }
        catch(SerializationException e)
        {
            Debug.Log("Read filed. Error: " + e.Message);


        }
        finally
        {
            fileStream.Close();
        }
        return returnType;
    }
}
