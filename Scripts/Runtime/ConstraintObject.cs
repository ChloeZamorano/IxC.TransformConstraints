using System.Collections.Generic;
using UnityEngine;

namespace IxC.TransformConstraints
{
    [ExecuteInEditMode]
    public class ConstraintObject : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ConstraintSolver> constraints;

        [SerializeField]
        public List<SerializedData> serializedConstraints;

        void Update()
        {
            for(int i = 0; i < constraints.Count; ++i)
                constraints[i].Solve(transform);
        }


        public void Awake()
        {
            if (constraints == null) constraints = new List<ConstraintSolver>();
            for (int i = 0; i < constraints.Count; ++i)
                constraints[i].Setup(transform);
        }

        public void OnEnable()
        {
            if (constraints == null) constraints = new List<ConstraintSolver>();
            for (int i = 0; i < constraints.Count; ++i)
                constraints[i].Setup(transform);
        }

        public void OnDisable()
        {
            for (int i = 0; i < constraints.Count; ++i)
                constraints[i].Cancel(transform);
        }

        public void OnBeforeSerialize()
        {
            if (constraints == null) return;
            serializedConstraints = new List<SerializedData>(constraints.Count);

            for (int i = 0; i < constraints.Count; ++i)
                serializedConstraints.Add(SerializedData.Serialize(constraints[i]));
        }
        public void OnAfterDeserialize()
        {
            if(serializedConstraints == null) return;
            constraints = new List<ConstraintSolver>(serializedConstraints.Count);

            for (int i = 0; i < serializedConstraints.Count; ++i)
                constraints.Add(SerializedData.Deserialize(serializedConstraints[i]) as ConstraintSolver);
        }
    }
}
