using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labscape.Items
{
    public interface IItemUse
    {
        // 아이템을 사용했을 때 실행될 로직
        void Use(GameObject user);
    }
}

