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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_41_ricochet.png");

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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_41_crystal.png");
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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_41_cluster_edit.png");
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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_57Explosive.PNG");
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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_107BlackHole.PNG");
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
            Icon = Resources.Load<Sprite>("Assets/Resources/Sprites/Modules/Bullet/Tex_skill_41_piercing.png");
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
