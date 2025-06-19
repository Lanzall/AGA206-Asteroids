using UnityEngine;

public class HelloWorld : MonoBehaviour
{

    public string Message = "Hello World!";
    string name1 = "Obi-Wan";
    string name2 = "General Greivous";
    string myName = "Fentanyl";

    void Start()
    {
        Debug.Log("My name is" + myName);
        myName = name1;
        Debug.Log("My name is" + myName);
        myName = name2;
        Debug.Log("My name is" + myName);
        name2 = name1;
        myName = name2;
        Debug.Log("My name is" + myName);
    }

    void Update()
    {
        

    }
}
