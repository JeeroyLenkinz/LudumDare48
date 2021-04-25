using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class MultiMorp : MonoBehaviour
{

    public GameObject[] morpsConnected;
    public GameObject[] connectors;
    private List<GameObject> connectorList = new List<GameObject>();
    private List<GameObject> morpsConnectedList = new List<GameObject>();
    [SerializeField]
    private GameEvent alienDropped;

    private Animate_Basic animBasic;

    // Start is called before the first frame update
    void Start()
    {

        animBasic = GetComponent<Animate_Basic>();
        foreach(GameObject morp in morpsConnected)
        {
            morp.GetComponent<AlienBase>().SetAttached();
        }

        foreach(GameObject connector in connectors)
        {
            connectorList.Add(connector);
        }

        foreach(GameObject morp in morpsConnected)
        {
            morpsConnectedList.Add(morp);
        }
    }

    public void SeperateMorps(GameObject morpOne, GameObject morpTwo, GameObject connector)
    {
        // Trigger smoke cloud

        // Detach Joint
        morpOne.GetComponent<AlienBase>().DetachJoint(morpTwo);

        // Detach from parents (i.e. this)
        morpOne.transform.parent = null;
        morpTwo.transform.parent = null;
        // TODO: Update the morpsConnectedList to remove any morps that are no longer part of the parent

        // Destroy this and the connector
        Destroy(connector);
        if(connectorList.Count == 0)
        {
            Destroy(this);
        }
    }

    public void checkChildStatuses() {
        bool dropAll = true;
        foreach (GameObject morp in morpsConnectedList) {
            AlienBase morpScript = morp.GetComponent<AlienBase>();
            if (morpScript.getIsHeld() || morpScript.getIsOnTable()) {
                dropAll = false;
                break;
            }
        }
        if (dropAll) {
            StartCoroutine(dropMultiMorp());
        }
    }

    private IEnumerator dropMultiMorp() {
        Tweener tweener = animBasic.Animate(AnimationTweenType.Scale, Vector2.zero, Vector2.zero);
        yield return new WaitForSeconds(tweener.Duration());
        alienDropped.Raise();
        Destroy(gameObject);

    }
}
