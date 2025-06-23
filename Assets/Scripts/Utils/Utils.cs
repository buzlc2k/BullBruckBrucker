using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BullBrukBruker
{
    public static class Utils
    {
        public static FieldInfo[] GetSortedFieldInfo(Type type)
        {
            List<FieldInfo> fieldInfos = new();

            var baseTypes = GetBaseTypes(type);
            for (int i = baseTypes.Count - 1; i >= 0; i--)
            {
                FieldInfo[] fields = baseTypes[i].GetFields(
                                                BindingFlags.Instance
                                                | BindingFlags.Public
                                                | BindingFlags.NonPublic
                                                | BindingFlags.DeclaredOnly);
                fieldInfos.AddRange(fields);
            }

            return fieldInfos.ToArray();
        }

        public static List<Type> GetBaseTypes(Type type)
        {
            List<Type> baseTypes = new() { type };

            while (type.BaseType != null)
            {
                type = type.BaseType;
                baseTypes.Add(type);
            }

            return baseTypes;
        }

        public static bool CheckBoundsOverlap(Vector3 pos1, float width1, float height1, Vector3 pos2, float width2, float height2)
        {
            var relativeVector = pos2 - pos1;

            bool overlapX = Mathf.Abs(relativeVector.x) <= width1 + height1;
            bool overlapY = Mathf.Abs(relativeVector.y) <= width2 + height2;

            return overlapX && overlapY;
        }

        public static bool CheckPointInsideBound(Vector3 position, Vector3 boundsCenter, float width, float height)
        {
            Vector3 relativePos = position - boundsCenter;

            return Mathf.Abs(relativePos.x) < width && Mathf.Abs(relativePos.y) < height;
        }

        public static bool CheckRayOverlap(Vector3 pos1, Vector3 dir, float length, Vector3 pos2, float halfWidth, float halfHeight)
        {
            var minBounds = pos2 - new Vector3(halfWidth, halfHeight, 0);
            var maxBounds = pos2 + new Vector3(halfWidth, halfHeight, 0);

            float tMinX, tMaxX, tMinY, tMaxY;

            if (Mathf.Abs(dir.x) < 1e-6f) // x ~ 0
            {
                if (pos1.x < minBounds.x || pos1.x > maxBounds.x)
                    return false;
                tMinX = float.NegativeInfinity;
                tMaxX = float.PositiveInfinity;
            }
            else
            {
                var t1 = (minBounds.x - pos1.x) / dir.x;
                var t2 = (maxBounds.x - pos1.x) / dir.x;
                tMinX = Math.Min(t1, t2);
                tMaxX = Math.Max(t1, t2);
            }

            if (Mathf.Abs(dir.y) < 1e-6f) // y ~ 0
            {
                if (pos1.y < minBounds.y || pos1.y > maxBounds.y)
                    return false;
                tMinY = float.NegativeInfinity;
                tMaxY = float.PositiveInfinity;
            }
            else
            {
                var t1 = (minBounds.y - pos1.y) / dir.y;
                var t2 = (maxBounds.y - pos1.y) / dir.y;
                tMinY = Math.Min(t1, t2);
                tMaxY = Math.Max(t1, t2);
            }

            var tMin = Math.Max(tMinX, tMinY);
            var tMax = Math.Min(tMaxX, tMaxY);

            return tMin <= tMax && tMin <= length && tMax >= 0;
        }

        public static Vector3 GetContactPointFromRay(Vector3 pos1, Vector3 dir, float length, Vector3 pos2, float halfWidth, float halfHeight)
        {
            var minBounds = pos2 - new Vector3(halfWidth, halfHeight, 0);
            var maxBounds = pos2 + new Vector3(halfWidth, halfHeight, 0);

            float tMinX, tMaxX, tMinY, tMaxY;

            if (Mathf.Abs(dir.x) < 1e-6f) // x ~ 0
            {
                if (pos1.x < minBounds.x || pos1.x > maxBounds.x)
                    return Vector3.zero;
                tMinX = float.NegativeInfinity;
                tMaxX = float.PositiveInfinity;
            }
            else
            {
                var t1 = (minBounds.x - pos1.x) / dir.x;
                var t2 = (maxBounds.x - pos1.x) / dir.x;
                tMinX = Math.Min(t1, t2);
                tMaxX = Math.Max(t1, t2);
            }

            if (Mathf.Abs(dir.y) < 1e-6f) // y ~ 0
            {
                if (pos1.y < minBounds.y || pos1.y > maxBounds.y)
                    return Vector3.zero;
                tMinY = float.NegativeInfinity;
                tMaxY = float.PositiveInfinity;
            }
            else
            {
                var t1 = (minBounds.y - pos1.y) / dir.y;
                var t2 = (maxBounds.y - pos1.y) / dir.y;
                tMinY = Math.Min(t1, t2);
                tMaxY = Math.Max(t1, t2);
            }

            var tMin = Math.Max(tMinX, tMinY);
            var tMax = Math.Min(tMaxX, tMaxY);

            if (!(tMin <= tMax && tMin <= length && tMax >= 0)) return Vector3.zero;

            return pos1 + tMin * dir;
        }

        static Vector3 GetCollisionNormal(Vector3 movingPos, Vector3 staticObjectPos, float staticObjectWidth, float staticObjectHeight)
        {
            Vector3 relativeVec = movingPos - staticObjectPos;

            float distToRight = staticObjectWidth - relativeVec.x;
            float distToLeft = staticObjectWidth + relativeVec.x;
            float distToTop = staticObjectHeight - relativeVec.y;
            float distToBottom = staticObjectHeight + relativeVec.y;

            float minDist = Mathf.Min(distToRight, distToLeft, distToTop, distToBottom);

            if (minDist == distToRight) return Vector3.right;
            if (minDist == distToLeft) return Vector3.left;
            if (minDist == distToTop) return Vector3.up;
            return Vector3.down;
        }

        public static Vector3 GetBouncingDir(Transform bounceObject, float pushbackDistance, Vector3 staticObjectPos, float staticObjectWidth, float staticObjectHeight)
        {
            if (CheckPointInsideBound(bounceObject.position, staticObjectPos, staticObjectWidth, staticObjectHeight))
                bounceObject.position -= pushbackDistance * Time.timeScale * bounceObject.up;

            return GetBouncingDir(bounceObject.position, bounceObject.up, staticObjectPos, staticObjectWidth, staticObjectHeight);
        }

        public static Vector3 GetBouncingDir(Vector3 bouncePos, Vector3 bounceDir, Vector3 staticObjectPos, float staticObjectWidth, float staticObjectHeight)
        {
            var inDirection = bounceDir;
            var inNormal = GetCollisionNormal(bouncePos, staticObjectPos, staticObjectWidth, staticObjectHeight);
            
            return Vector3.Reflect(inDirection, inNormal);
        }

        public static void StartSafeCourotine(MonoBehaviour starter, IEnumerator routine) {
            if (starter != null && starter.gameObject.activeInHierarchy)
                starter.StartCoroutine(routine);
        }
    }
}