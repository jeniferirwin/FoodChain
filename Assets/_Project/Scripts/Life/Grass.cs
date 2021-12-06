using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class Grass : Organism
    {
        protected override void Die()
        {
            OrganismDatabase.RemoveMember(gameObject);
            var cell = GridOperations.CellFromWorldPos(transform.position);
            GridOperations.ClearCell(cell);
        }
    }
}
