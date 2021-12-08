using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    [CreateAssetMenu(menuName = "FoodChain/OrganismTemplate")]
    public class OrganismTemplate : ScriptableObject
    {
        [Range(0f, 1f)] public float energyPercentValue;
        public float[] phaseLengths = new float[3];
        public Vector3[] phaseScales = new Vector3[3];
        public Material[] phaseMaterials = new Material[3];
        public int mainColorSlot;
    }
}