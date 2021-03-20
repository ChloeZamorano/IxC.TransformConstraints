using System.IO;
using UnityEngine;
using UnityEditor;

namespace IxC.TransformConstraints
{
    public abstract class ConstraintSolver
    {
        public Vector3
            pos,
            scale;
        public Quaternion rot;
        public float fac = 1.0f;
        public bool
            foldout = true,
            show = true;

        public ConstraintSolver() { }
        public ConstraintSolver(Transform t) => Setup(t);
        public ConstraintSolver(Vector3 position, Vector3 localScale, Quaternion rotation) =>
            Setup(position, localScale, rotation);

        public virtual void Setup(Vector3 position, Vector3 localScale, Quaternion rotation)
        {
            pos = position;
            scale = localScale;
            rot = rotation;
        }
        public virtual void Setup(Transform t)
        {
            pos = t.position;
            scale = t.localScale;
            rot = t.rotation;
        }

        public virtual void Solve(Transform t) { }

        public virtual void Cancel(Transform t)
        {
            t.position = pos;
            t.localScale = scale;
            t.rotation = rot;
        }
        public virtual void Cancel(out Vector3 position, out Vector3 localScale, out Quaternion rotation)
        {
            position = pos;
            localScale = scale;
            rotation = rot;
        }

        public virtual string MenuPath() => "Unnamed Constraint";

        public virtual void OnInspectorGUI(bool calledByBase = true)
        {
            EditorGUILayout.BeginHorizontal();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, Path.GetFileName(MenuPath()));
            fac = GUILayout.HorizontalSlider(fac, .0f, 1.0f);

            string showBtn = show ? "Hide" : "Show";
            if (GUILayout.Button(showBtn, GUILayout.ExpandWidth(false)))
                show = !show;

            if (calledByBase)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
