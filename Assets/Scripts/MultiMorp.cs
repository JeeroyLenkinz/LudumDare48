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
            //morp.GetComponent<AlienBase>().SetAttached();
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
        connectorList.Remove(connector);
        bool morpOneDisconnected = true;
        bool morpTwoDisconnected = true;

        foreach (GameObject remainingConnector in connectorList)
        {
            MorpConnector connectorLogic = remainingConnector.GetComponent<MorpConnector>();
            GameObject[] connectorsConnectedMorps = connectorLogic.GetConnectedMorps();
            if (connectorsConnectedMorps[0] == morpOne)
            {
                morpOneDisconnected = false;
                continue;
            } else if(connectorsConnectedMorps[1] == morpOne)
            {
                morpOneDisconnected = false;
                continue;
            }
            if (connectorsConnectedMorps[0] == morpTwo)
            {
                morpTwoDisconnected = false;
                continue;
            }
            else if (connectorsConnectedMorps[1] == morpTwo)
            {
                morpTwoDisconnected = false;
                continue;
            }
        }

        if (morpOneDisconnected)
        {
            morpOne.transform.parent = null;
            morpOne.GetComponent<AlienBase>().SetParentNull();
            // Set parent null here
            morpsConnectedList.Remove(morpOne);
        }
        if (morpTwoDisconnected)
        {
            morpTwo.GetComponent<AlienBase>().SetParentNull();
            morpTwo.transform.parent = null;
            morpsConnectedList.Remove(morpTwo);
        }

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

    public void ForceBreak()
    {
        foreach(GameObject childMorp in morpsConnectedList)
        {
            childMorp.GetComponent<AlienBase>().ForceBreak();
            childMorp.transform.parent = null;
        }

        foreach (GameObject childConnector in connectorList)
        {
            Destroy(childConnector);
        }

        Destroy(this.transform.gameObject);
    }
}
