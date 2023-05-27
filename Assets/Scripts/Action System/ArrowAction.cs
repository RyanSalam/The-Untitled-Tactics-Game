using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.GridSystem;
using UnityEngine;

namespace Tactics.ActionSystem
{
    public class ArrowAction : AttackAction
    {
        [Header("Components")]
        [SerializeField] private Transform arrowVisual;
        [SerializeField] private Transform arrowProjectile;

        [Header("Attributes")]
        [SerializeField] private float arrowHeight = 2;
        [SerializeField] private float arrowTravelTime = 1.2f;
        [SerializeField] private float arrowRotTime = 0.8f;

        public void LaunchArrow()
        {
            arrowProjectile.position = arrowVisual.position;
            arrowProjectile.rotation = Quaternion.identity;

            arrowProjectile.gameObject.SetActive(true);
            arrowProjectile.gameObject.SetActive(true);
            arrowVisual.gameObject.SetActive(false);

            StartCoroutine(ArrowTrajectory());
        }

        private IEnumerator ArrowTrajectory()
        {
            float timeElapsed = 0.0f;

            Vector3 start = arrowProjectile.position;
            Vector3 end = target.transform.position;

            Quaternion startRotEuler = transform.localScale.x > 0 ? Quaternion.Euler(Vector3.forward * 90) : Quaternion.Euler(Vector3.forward * -90);
            Quaternion endRotEuler = transform.localScale.x > 0? Quaternion.Euler(Vector3.forward * -90) : Quaternion.Euler(Vector3.forward * 90);

            while (timeElapsed < 1.2f)
            {
                Vector3 arrowTrajectory = Parabola(start, end, arrowHeight, timeElapsed / arrowTravelTime);
                Vector3 arrowDirection = arrowTrajectory - arrowProjectile.position;

                arrowProjectile.transform.position = arrowTrajectory;
                arrowProjectile.rotation = Quaternion.Slerp(startRotEuler, endRotEuler, timeElapsed / arrowRotTime);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.GetHealthComponent().TakeDamage(damageAmount);
            yield return new WaitForSeconds(0.3f);

            arrowProjectile.gameObject.SetActive(false);
            

            OnActionComplete();
        }

        private Vector2 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector2.Lerp(start, end, t);

            return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
        }
    }
}


