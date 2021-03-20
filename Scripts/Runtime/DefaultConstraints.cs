using System;
using UnityEngine;
using UnityEditor;

namespace IxC.TransformConstraints
{
    [Serializable]
    public class CopyPositionSolver : ConstraintSolver
    {
        public Transform target;

        public CopyPositionSolver() => target = null;
        public CopyPositionSolver(Transform t) : base(t) => target = null;

        public override void Solve(Transform t)
        {
            if (!target) return;
            t.position = Vector3.Lerp(pos, target.position, show ? fac : .0f);
        }

        public override string MenuPath() => "IxC/Copy Position";

        public override void OnInspectorGUI(bool calledByBase = false)
        {
            base.OnInspectorGUI(false);
            EditorGUILayout.EndHorizontal();


            if (foldout)
                target = EditorGUILayout.ObjectField
                    ("Target Object", target, typeof(Transform), true) as Transform;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    [Serializable]
    public class CopyRotationSolver : ConstraintSolver
    {
        public Transform target;

        public CopyRotationSolver() => target = null;
        public CopyRotationSolver(Transform t) : base(t) => target = null;

        public override void Solve(Transform t)
        {
            if (!target) return;
            t.rotation = Quaternion.Slerp(rot, target.rotation, show ? fac : .0f);
        }

        public override string MenuPath() => "IxC/Copy Rotation";

        public override void OnInspectorGUI(bool calledByBase = false)
        {
            base.OnInspectorGUI(false);
            EditorGUILayout.EndHorizontal();


            if (foldout)
                target = EditorGUILayout.ObjectField
                    ("Target Object", target, typeof(Transform), true) as Transform;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    public class CopyScaleSolver : ConstraintSolver
    {
        public Transform target;

        public CopyScaleSolver() => target = null;
        public CopyScaleSolver(Transform t) : base(t) => target = null;

        public override void Solve(Transform t)
        {
            if (!target) return;
            t.localScale = Vector3.Lerp(scale, target.localScale, show ? fac : .0f);
        }

        public override string MenuPath() => "IxC/Copy Scale";

        public override void OnInspectorGUI(bool calledByBase = false)
        {
            base.OnInspectorGUI(false);
            EditorGUILayout.EndHorizontal();


            if (foldout)
                target = EditorGUILayout.ObjectField
                    ("Target Object", target, typeof(Transform), true) as Transform;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    [Serializable]
    public class LookAtSolver : ConstraintSolver
    {
        public Transform target;

        public LookAtSolver() => target = null;
        public LookAtSolver(Transform t) : base(t) => target = null;

        public override void Solve(Transform t)
        {
            if (!target) return;
            t.rotation = Quaternion.Slerp(rot,
                Quaternion.LookRotation((target.position - t.position).normalized, Vector3.up)
                , show ? fac : .0f);
        }

        public override string MenuPath() => "IxC/Look At";

        public override void OnInspectorGUI(bool calledByBase = false)
        {
            base.OnInspectorGUI(false);
            
            EditorGUILayout.EndHorizontal();
            
            
            if (foldout)
                target = EditorGUILayout.ObjectField
                    ("Target Object", target, typeof(Transform), true) as Transform;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
