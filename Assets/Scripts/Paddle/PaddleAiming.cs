using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PaddleAiming : MonoBehaviour
    {
        private Vector3 contactPoint;
        private Vector3 bouncingPoint;
        [SerializeField] private float boucingLength = 0.75f;
        [SerializeField] PredictionTrajectoryController predictionTrajectoryController;

        public void CalculatingPoints()
        {
            if (!predictionTrajectoryController.gameObject.activeInHierarchy)
                predictionTrajectoryController.gameObject.SetActive(true);

            var inputPos = InputManager.Instance.Position;
            var dir = (inputPos - transform.position).normalized;

            foreach (var bound in GridManager.Instance.LevelBounds)
            {
                var contactPoint = Utils.GetContactPointFromRay(transform.position,
                                                dir, Mathf.Infinity,
                                                bound.transform.position,
                                                bound.Collision.Config.Width,
                                                bound.Collision.Config.Height);

                if (contactPoint != Vector3.zero
                    && Mathf.Abs(contactPoint.x) < ScreenManager.Instance.ScreenWidth
                    && Mathf.Abs(contactPoint.y) < ScreenManager.Instance.ScreenHeight)
                {
                    this.contactPoint = contactPoint;

                    var bouncingDir = Utils.GetBouncingDir(transform.position,
                                                                    dir,
                                                                    bound.transform.position,
                                                                    bound.Collision.Config.Width,
                                                                    bound.Collision.Config.Height);

                    bouncingPoint = contactPoint + boucingLength * bouncingDir;
                    break;
                }
            }

            predictionTrajectoryController.UpdateLineRenderer(contactPoint, bouncingPoint);
        }

        public void Shooting()
        {
            var dir = (contactPoint - transform.position).normalized;
            BallSpawner.Instance.SpawnSingleBall(transform.position, dir);
            predictionTrajectoryController.gameObject.SetActive(false);
        }
    }
}