using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAble  {

    void DoAct(int turn = 0);
    void EnterEvent();
}
