using UnityEngine;

namespace SplineMesh
{

    [CreateAssetMenu(fileName = "Mesh Preset", menuName = "SplineMeshTiling/Tiling Preset")]
    public sealed class MeshPreset : ScriptableObject
    {

        [Tooltip("Mesh to bend along the spline.")]
        public Mesh mesh;

        [Tooltip("Material to apply on the bent mesh.")]
        public Material material;

        [Tooltip("Physic material to apply on the bent mesh.")]
        public PhysicsMaterial physicMaterial;

        [Tooltip("Translation to apply on the mesh before bending it.")]
        public Vector3 translation;

        [Tooltip("Rotation to apply on the mesh before bending it.")]
        public Vector3 rotation;

        [Tooltip("Scale to apply on the mesh before bending it.")]
        public Vector3 scale = Vector3.one;

        [Tooltip("If true, a mesh collider will be generated.")]
        public bool generateCollider = true;

        [Tooltip("If true, the mesh will be bent on play mode. If false, the bent mesh will be kept from the editor mode, allowing lighting baking.")]
        public bool updateInPlayMode;

        [Tooltip("If true, a mesh will be placed on each curve of the spline. If false, a single mesh will be placed for the whole spline.")]
        public bool curveSpace;

        [Tooltip("The mode to use to fill the choosen interval with the bent mesh.")]
        public MeshBender.FillingMode mode = MeshBender.FillingMode.StretchToInterval;

    }

}
