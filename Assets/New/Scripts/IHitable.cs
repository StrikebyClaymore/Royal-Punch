﻿using UnityEngine;

namespace New
{
    public interface IHitable
    {
        void GetHit(Vector3 hitPoint);
    }
}