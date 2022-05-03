using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothChange : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;

    public Texture2D texture;
    public string shaderIDName = "_EmissionMap";

    [NaughtyAttributes.Button]
    private void ChangeTexture()
    {
        mesh.materials[0].SetTexture(shaderIDName, texture); //nao usar mesh.sharedMaterial pq qdo troca em tempo de execução, ele cria nova instancia e NÃO volta ao material original
    }
}
