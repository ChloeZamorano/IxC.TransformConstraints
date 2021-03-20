using System;
using UnityEngine;

namespace IxC
{
    /// <summary>
    /// Simple class for serialization of types not supported by Unity,
    /// such as abstract classes, which would otherwise need to be an
    /// <see cref="ScriptableObject"/> to be serialized at all, something
    /// that can be undesirable.
    /// 
    /// Usage is as simple as you'd think, for every variable you want to
    /// serialize using this, create a <see cref="SerializedData"/> "SD"
    /// variable marked as a <see cref="SerializeField"/>, implement
    /// <see cref="ISerializationCallbackReceiver"/> in the
    /// <see cref="MonoBehaviour"/> or <see cref="ScriptableObject"/>
    /// you're holding your variable "SOBJ"; In
    /// <see cref="ISerializationCallbackReceiver.OnBeforeSerialize"/>,pass
    /// SOBJ to <see cref="Serialize(object)"/> and set SD to the
    /// <see cref="SerializedData"/> returned by this function, then in
    /// <see cref="ISerializationCallbackReceiver.OnAfterDeserialize"/>,
    /// pass SD to object <see cref="Deserialize(SerializedData)"/>
    /// and set SOBJ to the object returned by the function, SOBJ doesn't
    /// need to be directly a <see cref="object"/>, using casting is desired.
    /// 
    /// Example:
    /// 
    /// public class SerializationExample : MonoBehaviour, ISerializationCallbackReceiver
    /// {
    ///     public List<AbstractClass> abstractObjects;
    ///     
    ///     [SerializeField]
    ///     public List<SerializedData> serializedObjects;
    /// 
    ///     public void OnBeforeSerialize()
    ///     {
    ///         if (abstractObjects == null) return;
    ///         serializedObjects = new List<SerializedData>(abstractObjects.Count);
    /// 
    ///         for (int i = 0; i < abstractObjects.Count; ++i)
    ///             serializedObjects.Add(SerializedData.Serialize(abstractObjects[i]));
    ///     }
    ///     
    ///     public void OnAfterDeserialize()
    ///     {
    ///         if (serializedObjects == null) return;
    ///         abstractObjects = new List<AbstractClass>(serializedObjects.Count);
    /// 
    ///         for (int i = 0; i < serializedConstraints.Count; ++i)
    ///             abstractObjects.Add(SerializedData.Deserialize(serializedObjects[i]) as AbstractClass);
    ///     }
    /// }
    /// 
    /// <see cref="JsonUtility"/> is used because it somewhat understands
    /// that it should serialize a reference to certain <see cref="UnityEngine.Object"/>s
    /// instead of the underlying data.
    /// </summary>

    [Serializable]
    public class SerializedData
    {
        public string type;
        public string data;

        /// <summary>
        /// Constructor, not meant to be used externally but do it
        /// if you have a reason to; otherwise refer to the static
        /// methods <see cref="Serialize(object)"/>
        /// and <see cref="Deserialize(SerializedData)"/>.
        /// </summary>
        /// <param name="type">Name of the <see cref="Type"/> that's being serialized with namespace.</param>
        /// <param name="data">The serialized data of the <see cref="object"/>.</param>
        public SerializedData(string type, string data)
        {
            this.type = type;
            this.data = data;
        }

        /// <summary>
        /// Serializes the given <see cref="object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be serialized.</param>
        /// <returns>A <see cref="SerializedData"/> containing the name of the <see cref="object"/> with namespace.</returns>
        public static SerializedData Serialize(object obj) =>
            new SerializedData(obj.GetType().FullName, JsonUtility.ToJson(obj));

        /// <summary>
        /// Deserializes the given <see cref="SerializedData"/>.
        /// </summary>
        /// <param name="sd">The <see cref="SerializedData"/> to be deserialized.</param>
        /// <returns>An <see cref="object"/> equivalent to the given <see cref="SerializedData"/>.</returns>
        public static object Deserialize(SerializedData sd) =>
            JsonUtility.FromJson(sd.data, Type.GetType(sd.type));
    }
}
