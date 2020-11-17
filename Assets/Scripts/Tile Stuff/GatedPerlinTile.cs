using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps 
{
    /// <summary>
    /// Gated Perlin Tiles and the editor were adapted from WeightedRandomTile
    /// It will take perlin noise, and provide the first sprite whose threshold / maxNoiseLevel is lower than generated noise.
    /// Stretch can be used to increase the step in noise between tiles.
    /// </summary>
    [Serializable]
    public class GatedPerlinTile : Tile 
    {
        private int xOffset, yOffset;

        /// <summary>
        /// Scales the step in noise between tiles.
        /// </summary>
        [SerializeField] public float stretch = 1.2f;

        /// <summary>
        /// Scales the noise output before comparison to the threshold.
        /// </summary>
        [SerializeField] public int maxNoiseLevel;

        /// <summary>
        /// The Sprites used for randomizing output.
        /// </summary>
        [SerializeField] public WeightedSprite[] Sprites;

        // adapted from
        // https://gamedev.stackexchange.com/questions/169700/how-can-i-change-a-sprite-used-for-a-unity-tilemap-tile-at-runtime
        /// <summary>
        /// Called when a tile gets the signal to refresh from outside sources.
        /// Strangely necessary for the map to be able to reresh itself.
        /// </summary>
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
            => tilemap.RefreshTile(position);

        /// <summary>
        /// Re-randomizes this scriptableobject instace of GatedPerlinTile
        /// </summary>
        public void Rerandomize()
        {
            // Perlin noise is symmetric across axes, so avoid 0 offsets.
            xOffset = StaticUtils.randomSign * Random.Range(100, 10000);
            yOffset = StaticUtils.randomSign * Random.Range(100, 10000);
        }

        /// <summary>
        /// Runs each time the tile is created.
        /// </summary>
        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            Rerandomize();
            return base.StartUp(location, tilemap, go);
        }

        /// <summary>
        /// Retrieves any tile rendering data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) 
        {
            base.GetTileData(position, tilemap, ref tileData);
            
            if (Sprites == null || Sprites.Length <= 0) return;
            
            float noiseValue = maxNoiseLevel * RawPerlinValue(position.x, position.y);
            noiseValue = Mathf.Min(maxNoiseLevel - 0.1f, Mathf.Max(noiseValue, 0.1f));

            foreach (var spriteInfo in Sprites)
            {
                if (spriteInfo.Weight <= noiseValue)
                {
                    tileData.sprite = spriteInfo.Sprite;  
                    return;
                }
            }
        }

        public float RawPerlinValue(int x, int y)
            => StaticUtils.PerlinNoise(stretch * (x + xOffset), stretch * (y + yOffset));
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GatedPerlinTile))]
    public class GatedPerlinTileEditor : Editor 
    {
        private SerializedProperty m_Color;
        private SerializedProperty m_ColliderType;

        private GatedPerlinTile Tile {
            get { return target as GatedPerlinTile; }
        }

        /// <summary>
        /// OnEnable for GatedPerlinTile.
        /// </summary>
        public void OnEnable()
        {
            m_Color = serializedObject.FindProperty("m_Color");
            m_ColliderType = serializedObject.FindProperty("m_ColliderType");
            Tile.Rerandomize();
        }

        /// <summary>
        /// Draws an Inspector for the GatedPerlinTile.
        /// </summary>
        public override void OnInspectorGUI() 
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            int count = EditorGUILayout.DelayedIntField("Number of Sprites", Tile.Sprites != null ? Tile.Sprites.Length : 0);
            if (count < 0) 
                count = 0;

            if (Tile.Sprites == null || Tile.Sprites.Length != count) 
            {
                Array.Resize(ref Tile.Sprites, count);
            }

            if (count == 0) 
                return;

            EditorGUILayout.Space();

            if (count > 1)
            {
                Tile.stretch = EditorGUILayout.FloatField("Stretch", Tile.stretch);
                Tile.stretch = Tile.stretch == 0 ? 1 : Tile.stretch;
                Tile.maxNoiseLevel = EditorGUILayout.IntField("Max Noise", Tile.maxNoiseLevel);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Place random sprites.");
                EditorGUILayout.Space();
            }

            for (int i = 0; i < count - 1; i++) 
            {
                Tile.Sprites[i].Sprite = (Sprite) EditorGUILayout.ObjectField("Sprite " + (i + 1), Tile.Sprites[i].Sprite, typeof(Sprite), false, null);
                Tile.Sprites[i].Weight = EditorGUILayout.IntField("Threshold " + (i + 1), Tile.Sprites[i].Weight);
            }

            var lastSprite = Tile.Sprites[count - 1];

            lastSprite.Sprite = (Sprite) EditorGUILayout.ObjectField("Default Sprite", lastSprite.Sprite, typeof(Sprite), false, null);
            if (count > 1)
            {
                EditorGUI.BeginDisabledGroup(true);
                lastSprite.Weight = EditorGUILayout.IntField("Threshold " + count, 0);
                EditorGUI.EndDisabledGroup();
            }

            Tile.Sprites[count - 1] = lastSprite;

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_Color);
            EditorGUILayout.PropertyField(m_ColliderType);

            if (EditorGUI.EndChangeCheck())
            {
                Tile.Rerandomize();
                EditorUtility.SetDirty(Tile);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    static internal partial class AssetCreation
    {
        [MenuItem("Assets/Create/2D/Tiles/Gated Perlin Tile",  priority = 201)]
        static void CreateGatedPerlinTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<GatedPerlinTile>(), "New Gated Perlin Tile.asset");
        }
    }
#endif
}
