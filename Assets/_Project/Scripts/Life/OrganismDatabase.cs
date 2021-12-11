using System.Collections.Generic;
using UnityEngine;

namespace FoodChain.Life
{
    public static class OrganismDatabase
    {
        private static List<Organism> members = new List<Organism>();
        
        public static void RemoveMember(Organism member) => members.Remove(member);

        public static void AddMember(Organism member)
        {
            if (!members.Contains(member)) members.Add(member);
        }
        
        public static void RemoveMember(GameObject member)
        {
            Organism org;
            if (member.TryGetComponent<Organism>(out org))
            {
                RemoveMember(org);
            }
        }
        
        public static void AddMember(GameObject member)
        {
            Organism org;
            if (member.TryGetComponent<Organism>(out org))
            {
                AddMember(org);
            }
        }
        
        public static List<Organism> FindAvailableMembersByPhase(string tag, int phase)
        {
            var tagged = FindMembersByTag(tag);
            return tagged.FindAll(x => x.CurrentLifePhase == phase && x.Aggressor == null);
        }

        public static List<Organism> FindAllMembersByPhase(string tag, int phase)
        {
            var tagged = FindMembersByTag(tag);
            return tagged.FindAll(x => x.CurrentLifePhase == phase);
        }
        
        public static List<Organism> FindMembersByTag(string tag)
        {
            return members.FindAll(x => x.CompareTag(tag));
        }
    }
}