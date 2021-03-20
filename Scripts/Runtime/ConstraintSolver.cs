using System.IO;
using UnityEngine;
using UnityEditor;

namespace IxC.TransformConstraints
{
    /// <summary>
    /// Base class for every constraint, by inheriting this class
    /// you can make your own constraints; keep in mind these are
    /// made only for dealing with <see cref="Transform"/>s, though
    /// you can indeed get other components, since <see cref="Transform"/>s
    /// have a property to reference the <see cref="GameObject"/>,
    /// but the system is not made to account for such a thing, you can
    /// try to do this, but it is not recommended.
    /// </summary>
    public abstract class ConstraintSolver
    {
        /// <summary>
        /// Original world space position of the <see cref="Transform"/> without
        /// applying any constraint.
        /// </summary>
        public Vector3 pos;
        /// <summary>
        /// Original local space scale of the <see cref="Transform"/> without
        /// applying any constraint.
        /// </summary>
        public Vector3 scale;
        /// <summary>
        /// Original world space rotation of the <see cref="Transform"/> without
        /// applying any constraint.
        /// </summary>
        public Quaternion rot;
        /// <summary>
        /// Value for the influence slider.
        /// </summary>
        public float fac = 1.0f;
        /// <summary>
        /// Whether or not the constraint is expanded in the inspector.
        /// </summary>
        public bool foldout = true;
        /// <summary>
        /// Whether or not the constraint is active.
        /// </summary>
        public bool show = true;

        /// <summary>
        /// Default constructor, not used for anything (?); for some reason
        /// calling constructors with parameters using <see cref="System.Activator.CreateInstance(System.Type, object[])"/>
        /// does not work.
        /// </summary>
        public ConstraintSolver() { }
        /// <summary>
        /// Constructor that takes in a <see cref="Transform"/> to set <see cref="pos"/>, <see cref="scale"/> and <see cref="rot"/>.
        /// </summary>
        /// <param name="t"><see cref="Transform"/> of this <see cref="GameObject"/></param>
        public ConstraintSolver(Transform t) => Setup(t);
        /// <summary>
        /// Constructor to set <see cref="pos"/>, <see cref="scale"/> and <see cref="rot"/> manually.
        /// </summary>
        /// <param name="position">Given position.</param>
        /// <param name="localScale">Given scale.</param>
        /// <param name="rotation">Given rotation.</param>
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
