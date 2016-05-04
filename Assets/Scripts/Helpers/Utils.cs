using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Random = UnityEngine.Random;

static class Utils
{
    public static T GetRandomEnum<T>()
    {
        var array = Enum.GetValues(typeof(T));
        var randomValue = (T)array.GetValue(Random.Range(0, array.Length));
        return randomValue;
    }


    public static object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }


    public static byte[] ObjectToByteArray(object obj)
    {
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}