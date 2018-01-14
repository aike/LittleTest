using UnityEngine;

using LittleTest;


public class ObjectATest : MonoBehaviour {

	void Start () {
        Tester.DoTest(this);
    }

    void Update () {

	}

    public void Test()
    {
        Tester.Test("Object A Test");
        Tester.AreEqual(transform.position.x >= 0, true);
    }

}
