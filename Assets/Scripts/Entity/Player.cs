using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{

    [RequireComponent(typeof(Gun), typeof(GunVisual))]
    public class Player : Character
    {
        private Gun Gun { get; set; }
        private CartridgePickup AvailablePickup { get; set; }

        public Vector3 AimPosition { get; set; }

        protected override void Awake()
        {
            base.Awake();

            gameObject.tag = "Player";
            Gun = GetComponent<Gun>();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Gun?.Shoot();
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                AddStatusEffect(new IceEffect());
            }

            if(Input.GetKeyDown(KeyCode.E)) // All Keycodes should be keybound via unity
            {
                AvailablePickup?.ApplyModuleTo(Gun);
            }

            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane floorPlane = new Plane(Vector3.up, transform.position);

            if (floorPlane.Raycast(cameraRay, out float rayLength))
            {
                var rayHit = cameraRay.GetPoint(rayLength);

                var aimPos = new Vector3(rayHit.x, Rigidbody.transform.position.y, rayHit.z);
                var aimDir = ((aimPos - transform.position) * 100.0f).normalized;
                aimDir.y = 0.0f;

                AimPosition = aimPos;

                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20.0f);
            }

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = Quaternion.Euler(0.0f, Camera.main.transform.localEulerAngles.y, 0.0f) * new Vector3(x, 0.0f, z).normalized;

            Rigidbody.velocity = moveDirection * CurrentMovementSpeed;
        }


        protected void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Pickup"))
            {
                AvailablePickup = other.GetComponent<CartridgePickup>();
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pickup"))
            {
                AvailablePickup = null;
                return;
            }
        }

    }
}
