using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class ClothChange : MonoBehaviour
    {
        public SkinnedMeshRenderer mesh;

        public Texture2D texture;
        public string shaderIDName = "_EmissionMap";

        private Texture2D _defaultTexture;

        private void Awake()
        {
            _defaultTexture = (Texture2D) mesh.materials[0].GetTexture(shaderIDName);
        }

        [NaughtyAttributes.Button]
        private void ChangeTexture()
        {
            mesh.materials[0].SetTexture(shaderIDName, texture); //nao usar mesh.sharedMaterial pq qdo troca em tempo de execução, ele cria nova instancia e NÃO volta ao material original
        }

        public void ChangeTexture(ClothSetup setup)
        {
            mesh.materials[0].SetTexture(shaderIDName, setup.texture);
        }

        public void ResetTexture()
        {
            mesh.materials[0].SetTexture(shaderIDName, _defaultTexture);
        }
    }
}

