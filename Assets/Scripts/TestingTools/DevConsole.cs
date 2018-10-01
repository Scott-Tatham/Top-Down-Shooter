using ScottyCode.Extensions;
using ScottyCode.Extensions.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DevConsole : MonoBehaviour
{
    [SerializeField, Range(0, 50)]
    int logOffset;
    [SerializeField, Range(0, 100)]
    int shiftAmount;
    [SerializeField]
    GameObject devConsoleParent;
    [SerializeField]
    RectTransform panel;
    [SerializeField]
    InputField commandInput;
    [SerializeField]
    GameObject loggedMessage;

    static DevConsole devConsole;

    int shiftIndex;
    string currentInput;
    List<DevCommand> commands;
    List<GameObject> logHistory;
    List<MethodInfo> devCommands;

    public static DevConsole GetDevConsole() { return devConsole; }

    public bool GetIsActive() { return devConsoleParent.activeSelf; }

    void Start()
    {
        if (devConsole == null)
        {
            devConsole = this;
        }

        else if (devConsole != this)
        {
            Destroy(gameObject);
        }

        currentInput = "";
        logHistory = new List<GameObject>();
        devCommands = new List<MethodInfo>();

        #region LINQ Varient (Slow But Compact)

        //devCommands.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass).SelectMany(x => x.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)).Where(x => x.GetCustomAttributes(typeof(DevCommand), false).FirstOrDefault() != null));

        #endregion

        #region For Loop Varient (Extremely Slow)

        //for (int i = 0; i < AppDomain.CurrentDomain.GetAssemblies().Length; i++)
        //{
        //    for (int j = 0; j < AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes().Length; j++)
        //    {
        //        if (AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes()[j].IsClass)
        //        {
        //            for (int k = 0; k < AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes()[j].GetMethods(BindingFlags.NonPublic | BindingFlags.Public | //BindingFlags.Static | BindingFlags.Instance).Length; k++)
        //            {
        //                if (AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes()[j].GetMethods(BindingFlags.NonPublic | BindingFlags.Public | //BindingFlags.Static | BindingFlags.Instance)[k].GetCustomAttributes(typeof(DevCommand), false).Length > 0)
        //                {
        //                    devCommands.Add(AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes()[j].GetMethods(BindingFlags.NonPublic | BindingFlags.Public | //BindingFlags.Static | BindingFlags.Instance)[k]);
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Foreach Loop Varient (Fastest)

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName == "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass)
                    {
                        foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
                        {
                            if (methodInfo.GetCustomAttributes(typeof(DevCommand), false).Length > 0)
                            {
                                devCommands.Add(methodInfo);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        commands = new List<DevCommand>();

        for (int i = 0; i < devCommands.Count; i++)
        {
            commands.AddRange((DevCommand[])devCommands[i].GetCustomAttributes(typeof(DevCommand), false));
        }
    }

    void Update()
    {
        ActivateConsole();
        EnterText();
        AutoCompletCurrent();
        NavigateHistory();
    }

    // The console hide/show method.
    void ActivateConsole()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && (Application.isEditor || Debug.isDebugBuild))
        {
            if (Debug.isDebugBuild)
            {
                Debug.developerConsoleVisible = Debug.developerConsoleVisible ? false : true;
            }

            devConsoleParent.SetActive(devConsoleParent.activeSelf ? false : true);
        }
    }

    // The commandline execution.
    void EnterText()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Text newMessage = NewMessage(commandInput.text, true);

            for (int i = 0; i < devCommands.Count; i++)
            {
                string[] commandSplit = newMessage.text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < commands.Count; j++)
                {
                    if (commands[j].GetCommand() == commandSplit[0])
                    {
                        if (devCommands[j].GetParameters().Length == commandSplit.Length - 1)
                        {
                            object[] paramaters = new object[commandSplit.Length - 1];
                            Array.Copy(commandSplit, 1, paramaters, 0, paramaters.Length);

                            for (int k = 0; k < paramaters.Length; k++)
                            {
                                paramaters[k] = Convert.ChangeType(paramaters[k], devCommands[j].GetParameters()[k].ParameterType);
                            }

                            if (devCommands[i].IsStatic)
                            {
                                devCommands[i].Invoke(null, paramaters);
                            }

                            else
                            {
                                object classInstance = Activator.CreateInstance(devCommands[i].DeclaringType);
                                devCommands[i].Invoke(classInstance, paramaters);
                            }

                            if (commands[j].GetCallbackMessage() != "")
                            {
                                NewMessage(commands[j].GetCallbackMessage());
                            }
                        }
                    }
                }
            }
        }
    }

    // Creates new message.
    Text NewMessage(string message, bool clearCommandLine = false)
    {
        ShiftHistoryToZero();
        logHistory.Add(Instantiate(loggedMessage, panel));
        Text newMessage = logHistory[logHistory.Count - 1].GetComponent<Text>();
        newMessage.text = message;

        if (clearCommandLine)
        {
            commandInput.text = "";
        }

        return newMessage;
    }

    // Navigate the history.
    void NavigateHistory()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShiftHistoryDown();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShiftHistoryUp();
        }
    }

    // Shift history up.
    void ShiftHistoryUp()
    {
        if (logHistory.Count > logOffset)
        {
            if (shiftIndex <= 0)
            {
                shiftIndex = 0;

                return;
            }

            shiftIndex--;

            for (int i = 0; i < logHistory.Count; i++)
            {
                logHistory[i].transform.position += Vector3.up * shiftAmount;
            }
        }
    }

    // Shift history down.
    void ShiftHistoryDown()
    {
        if (logHistory.Count > logOffset)
        {
            if (shiftIndex >= (logHistory.Count - logOffset))
            {
                shiftIndex = logHistory.Count - logOffset;

                return;
            }

            shiftIndex++;

            for (int i = 0; i < logHistory.Count; i++)
            {
                logHistory[i].transform.position += Vector3.down * shiftAmount;
            }
        }
    }

    // Shift to the first message.
    void ShiftHistoryToZero()
    {
        if (logHistory.Count > logOffset)
        {
            if (shiftIndex != 0)
            {
                if (shiftIndex > 0)
                {
                    ShiftHistoryDown();
                }

                else
                {
                    ShiftHistoryUp();
                }

                ShiftHistoryToZero();
            }
        }

        shiftIndex = 0;
        ShiftNewHistoryEntry();
    }

    // Shift to the current message.
    void ShiftNewHistoryEntry()
    {
        for (int i = 0; i < logHistory.Count; i++)
        {
            logHistory[i].transform.position += Vector3.up * shiftAmount;
        }
    }

    // Suggest command for auto complete.
    public void AutoCompleteSuggest()
    {
        //for (int i = 0; i < devCommands.Count; i++)
        //{
        //    currentInput = currentInput.Length > 0 ? commandInput.text.Remove(currentInput.Length, commandInput.text.Length - currentInput.Length) : //commandInput.text;
        //
        //    if (commands[i].GetCommand().Contains(currentInput))
        //    {
        //        commandInput.text = currentInput + commands[i].GetCommand().Remove(0, currentInput.Length).Colour(Color.red);
        //        commandInput.caretPosition = currentInput.Length;
        //    }
        //
        //    else
        //    {
        //        commandInput.text = currentInput;
        //        commandInput.caretPosition = commandInput.text.Length;
        //    }
        //}
    }

    // Auto complete the current command.
    public void AutoCompletCurrent()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    for (int i = 0; i < devCommands.Count; i++)
        //    {
        //        if (commands[i].GetCommand().Contains(commandInput.text))
        //        {
        //            commandInput.text = commands[i].GetCommand();
        //            commandInput.caretPosition = commandInput.text.Length;
        //
        //            return;
        //        }
        //    }
        //}
    }
}