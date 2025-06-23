using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class BallCollision : ObjectCollision
    {
        protected BallCollisionConfigRecord ballConfig;
        protected ObjectDespawning despawning;

        protected override void Awake()
        {
            base.Awake();

            SetObjectDespawning();
        }

        protected override void SetObjectCollisionConfigRecord()
        {
            Config = ConfigsManager.Instance.ObjectCollisionConfig.GetRecordByKeySearch(GetComponentInParent<BallController>().ID);
            ballConfig = Config as BallCollisionConfigRecord;
        }

        protected virtual void SetObjectDespawning() => despawning = GetComponentInParent<BallController>().Despawning;

        protected override void InitializeCollisionHandles()
        {
            collisionHandles = new()
            {
                [ObjectID.Paddle]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideWithPaddle(objCollision)),

                [ObjectID.HorizontalLevelBound]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideStaticObjects(objCollision)),

                [ObjectID.VerticalLevelBound]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideStaticObjects(objCollision)),

                [ObjectID.DeathBound]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideDeathBound()),
            };
        }

        public override void CalculateCollision()
        {
            if (CheckCollideWithCell()) return;

            base.CalculateCollision();
        }

        protected virtual bool CheckCollideWithCell()
        {
            foreach (var cell in currentCells)
            {
                if (!cell.CanCollide()) continue;
                OnCollideBlock(cell);
                return true;
            }

            return false;
        }

        protected virtual void OnCollideBlock(GridCell cell)
        {
            var newDir = Utils.GetBouncingDir(transform.parent, GridManager.Instance.CellSize/2, cell.GetCellPos(), GridManager.Instance.CellSize, GridManager.Instance.CellSize);
            transform.parent.up = newDir;
        }

        protected virtual void OnCollideStaticObjects(ObjectCollision objCollision)
        {
            var newDir = Utils.GetBouncingDir(transform.parent, GridManager.Instance.CellSize/2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height);
            transform.parent.up = newDir;
        }

        protected virtual void OnCollideDeathBound()
        {
            despawning.Despawn();
        }

        protected virtual void OnCollideWithPaddle(ObjectCollision paddleCollision)
        {
            var paddleObj = paddleCollision.transform.parent;

            var dis = paddleObj.position.x - transform.parent.position.x;

            float addedAngle = (dis / paddleCollision.Config.Width) * ballConfig.AddedBouncePerAngle;

            var newDir = Quaternion.Euler(0, 0, addedAngle) * Utils.GetBouncingDir(transform.parent, GridManager.Instance.CellSize/2, paddleObj.position, paddleCollision.Config.Width, paddleCollision.Config.Height);
            transform.parent.up = newDir;
        }
    }
}