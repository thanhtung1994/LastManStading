using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPoolObject  {

    void OnCreateObject();
    void OnDestroyObject();
}
