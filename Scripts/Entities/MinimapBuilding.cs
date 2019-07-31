using System.Linq;
using AdvancedMiniMap.Utilities;
using Ensage;
using Ensage.SDK.Helpers;

namespace AdvancedMiniMap.Scripts.Entities
{
    public class MinimapBuilding
    {
        public Entity Building { get; set; }

        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public bool IsAlive { get; set; } = true;

        public float Time { get; set; }
        public float GameTime { get; set; }

        public Team Team { get; set; }

        public MinimapBuilding(Database.BuildingName entityName, bool isLeft = true)
        {
            Time = 0;
            GameTime = Game.GameTime;

            if (entityName == Database.BuildingName.goodguys_tower4 || entityName == Database.BuildingName.badguys_tower4)
            {
                var data = EntityManager<Building>.Entities.Where(x => x.Name == entityName.GetName()).ToArray();
                if (data.Length > 0)
                {
                    float towerPosX = data[0].Position.X;
                    foreach (var build in data)
                    {
                        if (isLeft == true)
                        {
                            if (build.Position.X <= towerPosX)
                            {
                                Building = build;
                                towerPosX = build.Position.X;
                            }
                        }
                        else
                        {
                            if (build.Position.X >= towerPosX)
                            {
                                Building = build;
                                towerPosX = build.Position.X;
                            }
                        }
                    }
                }
                

            }
            else
            {
                Building = EntityManager<Building>.Entities.FirstOrDefault(x => x.Name == entityName.GetName());
            }

            if (Building == null)
            {
                MaxHealth = 1;
                Health = 0;
                IsAlive = false;
                return;
            }

            MaxHealth = Building.MaximumHealth;
            Health = Building.Health;
            Team = Building.Team;
        }

        public void OnUpdate()
        {
            try
            {
                if (Building != null)
                {
                    Health = Building.Health;
                    IsAlive = Building.IsAlive;
                }
                else
                {
                    Health = 0;
                    IsAlive = false;
                }
            }
            catch (EntityNotFoundException)
            {
                Health = 0;
                IsAlive = false;
            }
            
        }
    }
}