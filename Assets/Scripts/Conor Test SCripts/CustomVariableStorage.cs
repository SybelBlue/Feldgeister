// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Yarn.Unity;

// public void ResetDialogue()
//         {
//             variableStorage.ResetToDefaults();
//             StartDialogue();
//         }
// public VariableStorageBehaviour variableStorage;
// public class CustomVariableStorage : Yarn.Unity.VariableStorageBehaviour {
        

//     // Store a value into a variable
//     public virtual void SetValue(string variableName, Yarn.Value value) {
//         // 'variableName' is the name of the variable that 'value' 
//         // should be stored in.
//     }

//     // Return a value, given a variable name
//     public virtual Yarn.Value GetValue(string variableName) {
//         // 'variableName' is the name of the variable to return a value for
//     }

//     // Return to the original state
//     public virtual void ResetToDefaults () {

//     }
// }

// public abstract class VariableStorageBehaviour : MonoBehaviour, Yarn.VariableStorage
//     {
//         /// <inheritdoc/>
//         public abstract Value GetValue(string variableName);

//         /// <inheritdoc/>
//         public virtual void SetValue(string variableName, float floatValue) => SetValue(variableName, new Yarn.Value(floatValue));

//         /// <inheritdoc/>
//         public virtual void SetValue(string variableName, bool boolValue) => SetValue(variableName, new Yarn.Value(boolValue));

//         /// <inheritdoc/>
//         public virtual void SetValue(string variableName, string stringValue) => SetValue(variableName, new Yarn.Value(stringValue));

//         /// <inheritdoc/>
//         public abstract void SetValue(string variableName, Value value);

//         /// <inheritdoc/>
//         /// <remarks>
//         /// The implementation in this abstract class throws a <see
//         /// cref="NotImplementedException"/> when called. Subclasses of
//         /// this class must provide their own implementation.
//         /// </remarks>
//         public virtual void Clear() => throw new NotImplementedException();

//         /// <summary>
//         /// Resets the VariableStorageBehaviour to its initial state.
//         /// </summary>
//         /// <remarks>
//         /// This is similar to <see cref="Clear"/>, but additionally allows
//         /// subclasses to restore any default values that should be
//         /// present.
//         /// </remarks>
//         public abstract void ResetToDefaults();

//     void Start()
//     {Assert.IsNotNull(variableStorage, "Variable storage was not set! Can't run the dialogue!");

//             // Ensure that the variable storage has the right stuff in it
//         variableStorage.ResetToDefaults();
//     }
            
//     }

