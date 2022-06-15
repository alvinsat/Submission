using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerControl : MonoBehaviour
{
    bool isIntact;

    public void TooglePlatformer() {
        if (isIntact)
        {
            isIntact = true;
        }
        else {
            isIntact = false;
        }
    }
}
