using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LittleTest
{
    public class Log
    {
        public Tester.ATTR attr = Tester.ATTR.OK;
        public string timestamp = "";
        public string line = "";
        public List<Log> child = new List<Log>();

        public void Clear()
        {
            line = "";
            timestamp = "";
            attr = Tester.ATTR.OK;
            child = new List<Log>();
        }

        public void Set(Tester.ATTR inAttr, string inTimestamp, string inLine)
        {
            attr = inAttr;
            timestamp = inTimestamp;
            line = inLine;
        }

        public Log AddChild(Tester.ATTR inAttr, string inTimestamp, string inLine)
        {
            Log newLog = new Log();
            newLog.attr = inAttr;
            newLog.timestamp = inTimestamp;
            newLog.line = inLine;

            child.Add(newLog);

            return newLog;
        }

        public void ChangeAttr(Tester.ATTR inAttr)
        {
            if (attr != Tester.ATTR.NG)
            {
                attr = inAttr;
            }
        }
    }

    // =====================================================================

    public class Tester
    {
        public enum ATTR { TITLE, INFO, OK, NG, UNKNOWN };

        static Tester Instance;
        string testerName = "Test";
        bool initTester = false;
        static bool enabled = true;
        bool gui = false;

        GameObject scrollView;
        GameObject contentArea;
        ScrollRect scrollRect;

        Log logTree = new Log();
        Log classLog;
        Log testcaseLog;

        // ==============================================================================
        // public methods
        // ==============================================================================

        // define testcase
        public static void Test(string title)
        {
            GetInstance().TestImpl(title);
        }

        // invoke Test() function
        public static void DoTest(Object obj)
        {

            if (!enabled)
            {
                return;
            }
            GetInstance().DoTestImpl(obj);
        }

        // enable/disable all tests
        public static void SetActive(bool flag)
        {
            enabled = flag;
            if (GetInstance().scrollView != null)
            {
                GetInstance().scrollView.SetActive(flag);
            }
        }

        // define test name
        public static void SetName(string name)
        {
            GetInstance().testerName = name;
        }

        // set view and enable gui logging
        public static void SetScrollView(GameObject view)
        {
            GetInstance().SetScrollViewImpl(view);
            if (!enabled)
            {
                view.SetActive(false);
            }
            GetInstance().scrollRect = view.GetComponent<ScrollRect>();
        }

        public static void Log(string text)
        {
            Tester tester = GetInstance();
            string time = tester.GetTime();

            tester.AddGuiText(ATTR.INFO, time + text);
            tester.AddEditorText(ATTR.INFO, time, text);
        }

        // --------------------------------------------------------------

        static Tester GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Tester();
            }
            return Instance;
        }

        void SetScrollViewImpl(GameObject view)
        {
            gui = true;
            scrollView = view;
            Image img = scrollView.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 0.8f);

            contentArea = scrollView.transform.GetChild(0).GetChild(0).gameObject;
            ContentSizeFitter fitter = contentArea.AddComponent<ContentSizeFitter>();
            VerticalLayoutGroup layout = contentArea.AddComponent<VerticalLayoutGroup>();

            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlHeight = false;
            layout.childControlWidth = false;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
        }

        void AddGuiText(ATTR attr, string text)
        {
            if (!gui) return;

            int fontSize = 13;

            GameObject obj = new GameObject();
            obj.transform.SetParent(contentArea.transform);
            obj.name = "text";
            RectTransform rt = obj.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.sizeDelta = new Vector2(400, fontSize + 2);
            rt.pivot = new Vector2(0.5f, 0.5f);

            obj.AddComponent<CanvasRenderer>();
            Text textComponent = obj.AddComponent<Text>();
            textComponent.text = text;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = fontSize;
            textComponent.alignment = TextAnchor.UpperLeft;
            switch (attr)
            {
                case ATTR.TITLE:
                    textComponent.color = Color.white;
                    break;
                case ATTR.OK:
                    textComponent.color = new Color(0.7f, 0.7f, 1f);
                    break;
                case ATTR.NG:
                    textComponent.color = Color.red;
                    break;
                case ATTR.INFO:
                    textComponent.color = new Color(0.7f, 0.7f, 0.7f);
                    break;
            }

            scrollRect.verticalNormalizedPosition = 0;
        }

        void AddEditorText(ATTR attr, string timestamp, string text)
        {
            string col = "";
            switch (attr)
            {
                case ATTR.TITLE:
                    col = "white";
                    break;
                case ATTR.OK:
                    col = "#aaf";
                    break;
                case ATTR.NG:
                    col = "red";
                    break;
                case ATTR.INFO:
                    col = "#ccc";
                    break;
            }
            string s = "<color=" + col + ">" + timestamp + text + "</color>\n";
            Debug.Log(s);
        }

        void DoTestImpl(Object obj)
        {
            if (!initTester)
            {
                logTree.Set(ATTR.TITLE, GetTime(), testerName + " ===========================");
                PrintLog(logTree);
                initTester = true;
            }

            System.Type t = obj.GetType();
            string className = t.Name;
            System.Reflection.MethodInfo info = t.GetMethod("Test");

            logTree.child = new List<Log>();
            if (info != null)
            {
                // オブジェクトがTest()メソッドを持っているとき
                classLog = logTree.AddChild(ATTR.UNKNOWN, GetTime(), "      Class [" + className + "]");
                info.Invoke(obj, null);
                PrintChildLog(logTree);
            }
            else
            {
                // オブジェクトがTest()メソッドを持っていないとき
                classLog = logTree.AddChild(ATTR.INFO, GetTime(), "      Class [" + className + "] has no test.");
                PrintChildLog(logTree);
            }

        }


        void TestImpl(string title)
        {
            testcaseLog = classLog.AddChild(ATTR.UNKNOWN, GetTime(),  "            Case [" + title + "]");
        }

        void PrintLog(Log log)
        {
            if (log.attr == ATTR.OK)
            {
                log.line += "    OK";
            }
            if (log.attr == ATTR.NG)
            {
                log.line += "    NG";
            }

            AddGuiText(log.attr, log.line);
            AddEditorText(log.attr, log.timestamp, log.line);
        }

        void PrintChildLog(Log log)
        {
            {
                for (int i = 0; i < log.child.Count; i++)
                {
                    PrintLog(log.child[i]);
                    PrintChildLog(log.child[i]);
                }
            }
        }

        string GetTime()
        {
            return "[" + System.DateTime.Now.ToString("HH:mm:ss") + "] ";
        }

        void SetOK(string s1, string s2)
        {
            string message = s1 + " == " + s2;
            testcaseLog.AddChild(ATTR.OK, GetTime(), "                  " + message);
            testcaseLog.ChangeAttr(ATTR.OK);
            classLog.ChangeAttr(ATTR.OK);

        }
        void SetNG(string s1, string s2)
        {
            string message = s1 + " != " + s2;
            testcaseLog.AddChild(ATTR.NG, GetTime(), "                  " + message);
            testcaseLog.ChangeAttr(ATTR.NG);
            classLog.ChangeAttr(ATTR.NG);

        }

        // ==============================================================================
        // matcher
        // ==============================================================================

        public static void AreEqual(Object o1, Object o2)
        {
            Tester t = GetInstance();

            if (o1 == o2)
                t.SetOK(o1.name, o2.name);
            else
                t.SetNG(o1.name, o2.name);
        }

        public static void AreEqual(string o1, string o2)
        {
            Tester t = GetInstance();

            string q = "\"";
            if (o1 == o2)
                t.SetOK(q + o1 + q, q + o2 + q);
            else
                t.SetNG(q + o1 + q, q + o2 + q);
        }

        public static void AreEqual(int o1, int o2)
        {
            Tester t = GetInstance();

            if (o1 == o2)
                t.SetOK(o1.ToString(), o2.ToString());
            else
                t.SetNG(o1.ToString(), o2.ToString());
        }

        public static void AreEqual(float o1, float o2)
        {
            Tester t = GetInstance();

            if (o1 == o2)
                t.SetOK(o1.ToString(), o2.ToString());
            else
                t.SetNG(o1.ToString(), o2.ToString());
        }

        public static void AreEqual(bool o1, bool o2)
        {
            Tester t = GetInstance();

            if (o1 == o2)
                t.SetOK(o1.ToString(), o2.ToString());
            else
                t.SetNG(o1.ToString(), o2.ToString());
        }

        public static void IsNull(Object o)
        {
            Tester t = GetInstance();

            string name = "(null)";
            if (o == null)
            {
                t.SetOK(name, "(null)");
            }
            else
            {
                name = o.name;
                t.SetNG(name, "(null)");
            }
        }

        public static void IsNotNull(Object o)
        {
            Tester t = GetInstance();

            string name = "(null)";
            if (o != null)
            {
                name = o.name;
                t.SetOK(name, "(not null)");
            }
            else
            {
                t.SetNG(name, "(not null)");
            }
        }
    }

}
