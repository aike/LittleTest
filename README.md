LittleTest
===
A simple test framework for Unity

## FEATURES
 - builded standalone application test
 - test for runtime resources, component attaching and object instantiation
 - output test result in console window
 - output test result in scroll view if you need


## TEST YOUR CLASS

```cs
using UnityEngine;

using LittleTest;

public class MyTest : MonoBehaviour {
	void Start () {
        Tester.DoTest(this);
	}

    public void Test()
    {
        Tester.Test("Test 1");
        Tester.AreEqual("aa", "aa");

        Tester.Test("Test 2");
        Tester.AreEqual(foo() > 0, true);
    }
}

```

 1. Add LittleTest.cs in your Assets
 1. Create `public void Test()` method
 1. Descrive test name as `Tester.Test("testname");`
 1. Descrive test using matchers;


## SCROLL VIEW OUTPUT
The following code shows test results in scroll view and console window;

```cs
    void Awake()
    {
        Tester.SetScrollView(GameObject.Find("/Canvas/Scroll View"));
    }
```

## DISABLE TEST
The following code disable tests and hide scroll view.

```cs
        Tester.SetActive(false);
```

## MATCHER
 - public static void AreEqual(Object o1, Object o2)
 - public static void AreEqual(string o1, string o2)
 - public static void AreEqual(int o1, int o2)
 - public static void AreEqual(float o1, float o2)
 - public static void AreEqual(bool o1, bool o2)
 - public static void IsNull(Object o)
 - public static void IsNotNull(Object o)

## API
 - public static void SetName(string name)<br>define test name
 - public static void Test(string title)<br>define test case
 - public static void DoTest(Object obj)<br>execute Test() method of the class
 - public static void SetActive(bool flag)<br>enable/disable all tests
 - public static void SetScrollView(GameObject view)<br>set view and enable gui logging
 - public static void Log(string text)<br>show log text

## Credit
LittleTest program is licenced under MIT License.  
Copyright 2018, aike (@aike1000)
