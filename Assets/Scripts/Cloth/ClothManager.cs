using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;


namespace Cloth
{
    public enum ClothType
    {
        SPEED,
        STRONG,
    }
    
    public class ClothManager : Singleton<ClothManager>
    {
        public List<ClothSetup> clothSetups;

        public Texture2D currCloth;

        public ClothSetup GetSetupByType(ClothType clothType)
        {
            return clothSetups.Find(i => i.clothType == clothType);
        }
    }

    [System.Serializable] //para fazer essa classe aparecer no inspector
    public class ClothSetup
    {
        public ClothType clothType;
        public Texture2D texture;
    }
}
