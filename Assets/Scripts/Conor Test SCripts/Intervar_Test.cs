using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Intervar_Test : MonoBehaviour
{
   public DialogueRunner runner; 
   public VariableStorageBehaviour varStore;

    public int yarn_test_variable;

    [YarnCommand("hello_world")]
    public void Hello_World() {
        print("Hello World!");
    }

    [YarnCommand("Increase")]
    public void Increase_Variable(){
        print(yarn_test_variable);
        yarn_test_variable = yarn_test_variable + 30;
        varStore.SetValue("$yarn_test_variable", yarn_test_variable);
        print(yarn_test_variable);

    }

    // Start is called before the first frame update
    void Start()
    {
        varStore = runner.variableStorage;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
