﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace KA_Rumble
{
    internal class Functions
    {
        public static bool ShouldOverload(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return !SpellManager.W.IsReady() && !SpellManager.E.IsReady() && !SpellManager.R.IsReady();
                case SpellSlot.W:
                    return !SpellManager.Q.IsReady() && !SpellManager.E.IsReady() && !SpellManager.R.IsReady();
                case SpellSlot.E:
                    return !SpellManager.Q.IsReady() && !SpellManager.W.IsReady() && !SpellManager.R.IsReady();
                case SpellSlot.R:
                    return !SpellManager.Q.IsReady() && !SpellManager.W.IsReady() && !SpellManager.E.IsReady();
            }
            return false;
        }

        public static void CastR(AIHeroClient target, int minimunE)
        {
            
            if (target != null && target.CountEnemiesInRange(1000) == 1)
            {
                if (target.IsMoving)
                {
                    var initPos = target.Position.To2D() + 500 * target.Direction.To2D().Perpendicular();
                    var endPos = target.Position.Extend(initPos.To3D(), 25);

                    var pred = SpellManager.R.GetPrediction(target);

                    if (pred.HitChance >= HitChance.High)
                    {
                        Player.CastSpell(SpellSlot.R, initPos.To3D(), endPos.To3D());
                    }
                }
                else
                {
                    var initPos = target.Position.To2D() + 500 * target.Direction.To2D().Perpendicular();
                    var endPos = target.Position.Extend(initPos.To3D(), -250);

                    var pred = SpellManager.R.GetPrediction(target);

                    if (pred.HitChance >= HitChance.High)
                    {
                        Player.CastSpell(SpellSlot.R, initPos.To3D(), endPos.To3D());
                    }
                }

            }

            if (target != null && target.CountEnemiesInRange(1000) > 1)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget()).Select(enemy => enemy.Position.To2D()).ToList();

                var endPos = GetBestEnPos(enemies, SpellManager.R.Width, 1000, minimunE);

                Player.CastSpell(SpellSlot.R, endPos.Shorten(Player.Instance.Position.To2D(), 1000).To3D(), endPos.To3D());
            }
        }

        private static Vector2 GetBestEnPos(List<Vector2> enemies, float width, float range, int minenemies)
        {
            var enemyCount = 0;
            var startPos = Player.Instance.Position.To2D();
            var result = new Vector2();

            var posiblePositions = new List<Vector2>();
            posiblePositions.AddRange(enemies);

            var max = enemies.Count;
            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (enemies[j] != enemies[i])
                    {
                        posiblePositions.Add((enemies[j] + enemies[i]) / 2);
                    }
                }
            }

            foreach (var pos in posiblePositions)
            {
                if (pos.Distance(startPos, true) <= range * range)
                {
                    var endPos = startPos + range * (pos - startPos).Normalized();

                    var count =
                        enemies.Count(pos2 => pos2.Distance(startPos, endPos, true, true) <= width * width);

                    if (count >= enemyCount && count >= minenemies)
                    {
                        result = endPos;
                        enemyCount = count;
                    }
                }
            }

            return new Vector2(result.X, result.Y);
        }
    }
}