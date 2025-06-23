using UnityEngine;

namespace BullBrukBruker
{
    public class PredictionTrajectoryController : MonoBehaviour
    {
        [SerializeField] private Transform contactBall;
        [SerializeField] private LineRenderer lineRenderer;

        private void Awake()
        {
            SetUpLineRenderer();
        }

        private void SetUpLineRenderer() => lineRenderer.positionCount = 3;

        public void UpdateLineRenderer(Vector3 contactPoint, Vector3 bouncingPoint)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, contactPoint);
            lineRenderer.SetPosition(2, bouncingPoint);
            contactBall.position = contactPoint;
        }
    }
}