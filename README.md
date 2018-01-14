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
 1. Write `using LittleTest;`
 1. Add `public void Test()` method in your class
 1. Write test case as `Tester.Test("test case name");` in Test() method
 1. Write test using matchers as `Tester.AreEqual(actual, expect);` in Test() method
 1. excute test as `Tester.DoTest(this);`


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
 - Tester.AreEqual(Object o1, Object o2)
 - Tester.AreEqual(string o1, string o2)
 - Tester.AreEqual(int o1, int o2)
 - Tester.AreEqual(float o1, float o2)
 - Tester.AreEqual(bool o1, bool o2)
 - Tester.IsNull(Object o)
 - Tester.IsNotNull(Object o)

## API
| API | description |
|:----|:------------|
| Tester.SetName(string name) | define test name |
| Tester.Test(string title) | define test case |
| Tester.DoTest(Object obj) | execute Test() method of the class |
| Tester.SetActive(bool flag) | enable/disable all tests |
| Tester.SetScrollView(GameObject view) | set view and enable gui logging |
| Tester.Log(string text) | show log text |

## Credit
LittleTest program is licenced under MIT License.  
Copyright 2018, aike (@aike1000)
