using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public abstract class ProjectileEffect : Module
    {
        public const string BULLET_PREFAB_PATH = "Prefabs/Gun/Projectile_Crystal";

        public GameObject BulletPrefab { get; protected set; }

        public abstract Projectile CreateProjectile(Vector3 barrelPosition);

        public static Projectile CreateDefaultProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(Resources.Load<GameObject>(BULLET_PREFAB_PATH), barrelPosition, Quaternion.identity);
            return parent.AddComponent<Projectile>(); ;
        }
    }

    public class RicochetEffect : ProjectileEffect
    {
        public RicochetEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Ricochet";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_41_ricochet");
            Description = "Dynamic ballistics allow the bullet to bounce on impact.";

        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<RicochetProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class CrystalEffect : ProjectileEffect
    {
        public CrystalEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Crystal";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_41_crystal");
            Description = "After a moment, the bullet splits into two, moving directly away form one another.";
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<CrystalProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class ClusterEffect : ProjectileEffect
    {
        public ClusterEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Cluster";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_41_cluster_edit");
            Description = "THe bullet bursts open on impact, creating a circle of bullets";
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<ClusterProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class ExplosionEffect : ProjectileEffect
    {
        public ExplosionEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Explosive";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_57Explosive");
            Description = "The bullet explodes on impact, dealing damage to all enemies within range.";
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<ExplosiveProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class BlackHoleEffect : ProjectileEffect
    {
        public BlackHoleEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Black Hole";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_107BlackHole");
            Description = "Exotic particles in the bullet destabilizes on impact, briefly creating a singularity than sucks in all nearby enemies.";
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<BlackHoleProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class PiercingEffect : ProjectileEffect
    {
        public PiercingEffect()
        {
            BulletPrefab = Resources.Load<GameObject>(BULLET_PREFAB_PATH);
            Name = "Piercing";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Bullet/Tex_skill_41_piercing");
            Description = "Reinforced alloys allow the bullets to continue their path through objects.";
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<PiercingProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }
}
