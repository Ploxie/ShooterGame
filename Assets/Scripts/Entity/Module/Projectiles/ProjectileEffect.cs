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
        public GameObject BulletPrefab { get; protected set; }

        public abstract Projectile CreateProjectile(Vector3 barrelPosition);
    }

    public class RicochetEffect : ProjectileEffect
    {
        public RicochetEffect()
        {
            BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile_Crystal");
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
            BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile_Crystal");
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
            BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile_Crystal");
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<ClusterProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }

    public class BlackHoleEffect : ProjectileEffect
    {
        public BlackHoleEffect()
        {
            BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile_Crystal");
        }

        public override Projectile CreateProjectile(Vector3 barrelPosition)
        {
            GameObject parent = GameObject.Instantiate(BulletPrefab, barrelPosition, Quaternion.identity);
            Projectile projectile = parent.AddComponent<BlackHoleProjectile>();
            projectile.ProjectileEffect = this;
            return projectile;
        }
    }
}
