using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BullBrukBruker
{
    public abstract class ObjectCollision : MonoBehaviour
    {
        protected List<GridCell> currentCells;
        protected List<GridCell> removedCells;
        protected Dictionary<ObjectID, (Func<ObjectCollision, bool> condition, Action<ObjectCollision> action)> collisionHandles;

        // Dependencies
        public ObjectCollisionConfigRecord Config { get; protected set; }

        protected virtual void Awake()
        {
            SetObjectCollisionConfigRecord();
            InitializeCollisionHandles();

            currentCells = new List<GridCell>();
            removedCells = new List<GridCell>();
        }

        protected virtual void OnDisable()
        {
            Utils.StartSafeCourotine(GridManager.Instance, UnregisterAllCells());
        }

        protected abstract void SetObjectCollisionConfigRecord();

        protected abstract void InitializeCollisionHandles();

        public void CalculateCellGroup()
        {
            var newCellGroup = GridManager.Instance.GetCurrentCells(transform.parent.position, Config.Width, Config.Height);
            RegisterNewCells(newCellGroup.Except(currentCells).ToList());

            removedCells = currentCells.Except(newCellGroup).ToList();
            UnregisterRemovedCells();

            currentCells = newCellGroup;
        }

        protected virtual void RegisterNewCells(List<GridCell> newCells)
        {
            foreach (var cell in newCells)
                cell.RegisterObject(Config.ID, this);
        }

        protected virtual void UnregisterRemovedCells() => StartCoroutine(C_UnregisterRemovedCells());

        protected virtual IEnumerator C_UnregisterRemovedCells()
        {
            yield return new WaitForEndOfFrame();

            foreach (var cell in removedCells)
                cell.UnRegisterObject(Config.ID, this);
        }

        protected virtual IEnumerator UnregisterAllCells()
        {
            yield return new WaitForEndOfFrame();

            foreach (var cell in currentCells)
                    cell.UnRegisterObject(Config.ID, this);

            removedCells.Clear();
        }

        public virtual void CalculateCollision()
        {
            foreach (var handle in collisionHandles)
            {
                var collisionableObjects = GetCollisionableObjectInCells(handle.Key);

                foreach (var collisionableObject in collisionableObjects)
                {
                    if (handle.Value.condition(collisionableObject))
                        handle.Value.action(collisionableObject);
                }
            }
        }

        protected virtual List<ObjectCollision> GetCollisionableObjectInCells(ObjectID collisionableID)
        {
            List<ObjectCollision> collisionableObjects = new();

            foreach (var cell in currentCells)
                if (cell.Objects.ContainsKey(collisionableID))
                    collisionableObjects.AddRange(cell.Objects[collisionableID]);

            return collisionableObjects;
        }
    }
}