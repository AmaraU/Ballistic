using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitUpgrade : instantUpgradeType
{
    public Orbiter orbiter;
    private GameObject orber;
    private void Awake() {
        reset();
    }

    public override void doUpgrade()
    {
        orber = Instantiate(orbiter,Vector3.zero,Quaternion.identity,globalVariables.Instance.ballShoot.transform).gameObject;
        orber.transform.localPosition = Vector3.zero;
        orber.transform.localScale = Vector3.one/globalVariables.Instance.ballShoot.transform.localScale.x;
    }

    public override void reset()
    {
        if(orber != null){
            Destroy(orber);
        }
        orber = null;
    }
}
