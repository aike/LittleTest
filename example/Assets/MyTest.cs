using UnityEngine;

using LittleTest;

public class MyTest : MonoBehaviour {

    public GameObject a;
    public GameObject b;
    float time = 0;

    void Awake()
    {
        Tester.SetActive(true);
        Tester.SetScrollView(GameObject.Find("/Canvas/Scroll View"));
        Tester.SetName("Unit Test");
    }

	void Start () {
        Tester.DoTest(this);
	}

	void Update () {
        time += Time.deltaTime;
        if (time >= 10)
        {
            Tester.Log("Plain Text Output Example " + Time.time.ToString());
            time = 0;
        }
    }

    public void Test()
    {
        Tester.Test("My Test 1");
        Tester.AreEqual("aa", "aa");
        string nullstring = null;
        Tester.AreEqual(nullstring, null);
        Tester.AreEqual((50 + 60 > 70), true);

        Tester.Test("Object Test");
        Tester.IsNotNull(a);
        Tester.AreEqual(a, b);
        Tester.IsNotNull(GameObject.Find("/Sphere"));
        Tester.IsNull(GameObject.Find("/Plane"));
    }
}
