using LazyTitan.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum LogLevel
{
    BYPASS,
    ERROR,
    WARN,
    INFO,
    ALL,
    NONE
}

/// <summary>
/// DevConsole - Ingame devevloper console that uses custom attributes to call commands.
/// </summary>
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
    Dropdown autoComplete, logLevelSelect;
    [SerializeField]
    Toggle logFileCheck;
    [SerializeField]
    GameObject loggedMessage;

    static DevConsole devConsole;

    bool canShift;
    int shiftIndex;
    LogLevel logLevel;
    List<DevCommand> commands;
    List<GameObject> logHistory;
    List<MethodInfo> devCommands;

    public static DevConsole GetDevConsole() { return devConsole; }

    public bool GetIsActive() { return devConsoleParent.activeSelf; }
    public List<DevCommand> GetCommands() { return commands; }
    public List<MethodInfo> GetDevCommands() { return devCommands; }

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

        canShift = true;
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

    void LateUpdate()
    {
        ActivateConsole();
        EnterText();
        ShowAutoCompleteOptions();
        HideAutoCompleteOptions();
        NavigateHistory();
        commandInput.caretPosition = commandInput.text.Length;
        commandInput.selectionAnchorPosition = commandInput.text.Length;
        commandInput.selectionFocusPosition = commandInput.text.Length;
    }

    void OnApplicationQuit()
    {
        if (logFileCheck)
        {
            string outputPath = Application.streamingAssetsPath + "/Logs";
            Directory.CreateDirectory(outputPath);
            outputPath = outputPath + "/";

            for (int i = 5; i > -1; i--)
            {
                if (i == 0)
                {
                    List<string> logOutput = new List<string>();

                    for (int j = 0; j < logHistory.Count; j++)
                    {
                        string currentLog = logHistory[j].GetComponent<Text>().text;

                        if (currentLog.Contains("<"))
                        {
                            currentLog = RemoveHTML(currentLog);
                        }

                        logOutput.Add(currentLog);
                    }

                    File.WriteAllLines(outputPath + "Latest.txt", logOutput.ToArray());
                }

                else
                {
                    if (!File.Exists(outputPath + "Log" + i.NumberNames() + ".txt"))
                    {
                        File.Create(outputPath + "Log" + i.NumberNames() + ".txt");
                    }

                    if (File.Exists(outputPath + "Log" + (i - 1).NumberNames() + ".txt"))
                    {
                        File.Copy(outputPath + "Log" + (i - 1).NumberNames() + ".txt", outputPath + "Log" + i.NumberNames() + ".txt", true);
                    }

                    else if (i == 1 && File.Exists(outputPath + "Latest.txt"))
                    {
                        File.Copy(outputPath + "Latest.txt", outputPath + "Log" + i.NumberNames() + ".txt", true);
                    }
                }
            }
        }
    }

    // The console hide/show method.
    void ActivateConsole()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && (Application.isEditor || Debug.isDebugBuild))
        {
            if (Debug.isDebugBuild)
            {
                Debug.developerConsoleVisible = devConsoleParent.activeSelf;
            }

            devConsoleParent.SetActive(devConsoleParent.activeSelf ? false : true);

            if (devConsoleParent.activeSelf)
            {
                commandInput.Select();
                commandInput.ActivateInputField();
            }
        }
    }

    // The commandline execution.
    void EnterText()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (canShift)
            {
                Text newMessage = NewMessage(commandInput.text, true, LogLevel.BYPASS);
                
                string[] commandSplit = newMessage.text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                bool foundFlag = false;
                bool correctParamsFlag = false;

                for (int i = 0; i < commands.Count; i++)
                {
                    if (commands[i].GetCommand().ToLower() == commandSplit[3].ToLower())
                    {
                        if (devCommands[i].GetParameters().Length == commandSplit.Length - 4)
                        {
                            object[] paramaters = new object[commandSplit.Length - 4];
                            Array.Copy(commandSplit, 4, paramaters, 0, paramaters.Length);

                            for (int j = 0; j < paramaters.Length; j++)
                            {
                                paramaters[j] = Convert.ChangeType(paramaters[j], devCommands[i].GetParameters()[j].ParameterType);
                            }

                            object classInstance;

                            if (devCommands[i].DeclaringType.BaseType == typeof(MonoBehaviour) && FindObjectOfType(devCommands[i].DeclaringType) != null)
                            {
                                classInstance = FindObjectOfType(devCommands[i].DeclaringType);
                            }

                            else
                            {
                                classInstance = Activator.CreateInstance(devCommands[i].DeclaringType);
                            }

                            devCommands[i].Invoke(classInstance, paramaters);

                            if (commands[i].GetCallbackMessage() != "" && commands[i].GetCallbackMessage() != null)
                            {
                                NewMessage(commands[i].GetCallbackMessage(), false, LogLevel.BYPASS);
                            }

                            correctParamsFlag = true;
                        }

                        foundFlag = true;
                    }

                    if (i == commands.Count - 1)
                    {
                        if (!foundFlag)
                        {
                            NewMessage(InternalMessages.CommandNotFound(commandSplit[3]), false, LogLevel.ERROR);
                        }

                        else
                        {
                            if (!correctParamsFlag)
                            {
                                NewMessage(InternalMessages.IncorrectArguments(commandSplit[3]), false, LogLevel.ERROR);
                            }
                        }
                    }
                }
            }

            else
            {
                AutoCompleteCurrent();
                canShift = true;
            }
            
            commandInput.Select();
            commandInput.ActivateInputField();
        }
    }

    // Creates new message.
    public Text NewMessage(string message, bool clearCommandLine, LogLevel logLevel)
    {
        if (GetIsActive())
        {
            if (this.logLevel != LogLevel.NONE && this.logLevel <= logLevel)
            {
                ShiftHistoryToZero();
                logHistory.Add(Instantiate(loggedMessage, panel));
                Text newMessage = logHistory[logHistory.Count - 1].GetComponent<Text>();
                newMessage.text = "[" + DateTime.Now.ToShortTimeString() + "] " + logLevel.LogLevelFormat() + message;

                if (clearCommandLine)
                {
                    commandInput.text = "";
                }

                return newMessage;
            }
        }

        return null;
    }

    // Navigate the history.
    void NavigateHistory()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canShift)
        {
            ShiftHistoryDown();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && canShift)
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
        autoComplete.ClearOptions();

        if (commandInput.text != "")
        {
            List<string> autoCompleteResults = new List<string>();

            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].GetCommand().ToLower().Contains(commandInput.text.ToLower()))
                {
                    autoCompleteResults.Add(commands[i].GetCommand());
                }
            }

            autoComplete.AddOptions(autoCompleteResults);
        }

        autoComplete.RefreshShownValue();
    }

    // Auto complete the current command.
    public void AutoCompleteCurrent()
    {
        commandInput.text = autoComplete.options[autoComplete.value].text;
        commandInput.Select();
        commandInput.ActivateInputField();
    }

    // Define log level.
    public void ChangeLogLevel()
    {
        logLevel = (LogLevel)logLevelSelect.value;
    }

    // Show auto complete options.
    void ShowAutoCompleteOptions()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            autoComplete.Show();
            canShift = false;
        }
    }

    // Hide auto complete options.
    void HideAutoCompleteOptions()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            autoComplete.Hide();
            canShift = true;
        }
    }

    string RemoveHTML(string currentLog)
    {
        for (int k = 0; k < currentLog.Length; k++)
        {
            if (currentLog[k] == '<')
            {
                for (int l = k + 1; l < currentLog.Length; l++)
                {
                    if (currentLog[l] == '>')
                    {
                        string newLog = currentLog.Remove(k, (l + 1) - k);

                        return RemoveHTML(newLog);
                    }
                }
            }
        }

        return currentLog;
    }
}

#region DevCommand Attribute

/// <summary>
/// Apply this attribute to a method to have it as a command in the developer console.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class DevCommand : Attribute
{
    string command;
    string helpMessage;
    string callbackMessage;

    public string GetCommand() { return command; }
    public string GetHelpMessage() { return helpMessage; }
    public string GetCallbackMessage() { return callbackMessage; }

    /// <summary>
    /// Create a command for the developer console.
    /// </summary>
    /// <param name="command"> The command's name used to call it. </param>
    public DevCommand(string command, string helpMessage)
    {
        this.command = command;
        this.helpMessage = helpMessage;
    }

    /// <summary>
    /// Create a command for the developer console with a callback message.
    /// </summary>
    /// <param name="command"> The command's name used to call it. </param>
    /// <param name="callbackMessage"> The command's callback message upon executing command. Supports rich text. </param>
    public DevCommand(string command, string helpMessage, string callbackMessage)
    {
        this.command = command;
        this.helpMessage = helpMessage;
        this.callbackMessage = callbackMessage;
    }
}

#endregion

#region Internal Commands

/// <summary>
/// Class for internal commands used by DevConsole.
/// </summary>
public class InternalCommands
{
    [DevCommand("DevConsole", "DevConsole information.")]
    public static void DevConsoleInfo()
    {// Get nicer colours.
        DevConsole.GetDevConsole().NewMessage("DevConsole".Colour("#bd59db") + " was created by".Colour(Color.black) + " Scott Tatham".Colour("#407a4e") + ".".Colour(Color.black), false, LogLevel.BYPASS);
        DevConsole.GetDevConsole().NewMessage("Use as you see fit.", false, LogLevel.BYPASS);
        DevConsole.GetDevConsole().NewMessage("For more help and/or information contact me at " + "scottytatham@gmail.com".Colour("#689dd6").Bold() + ".".Colour(Color.black), false, LogLevel.BYPASS);
    }

    [DevCommand("Help", "List of all commands. Type Help [CommandName] to get help for a specific command.")]
    public static void InitHelp()
    {
        string commandNames = "|";

        for (int i = 0; i < DevConsole.GetDevConsole().GetCommands().Count; i++)
        {
            commandNames = commandNames + DevConsole.GetDevConsole().GetCommands()[i].GetCommand() + "|";

            // Fix less than full row cases.
            if (i % 3 == 2 || (DevConsole.GetDevConsole().GetCommands().Count % 3 > 0 && i == DevConsole.GetDevConsole().GetCommands().Count - 1))
            {
                DevConsole.GetDevConsole().NewMessage(commandNames, false, LogLevel.BYPASS);
                commandNames = "|";
            }
        }
    }

    [DevCommand("Help", "Displays help for specified command")]
    public static void CommandHelp(string command)
    {
        for (int i = 0; i < DevConsole.GetDevConsole().GetCommands().Count; i++)
        {
            if (DevConsole.GetDevConsole().GetCommands()[i].GetCommand().ToLower() == command.ToLower())
            {
                string parameters = "|";

                for (int j = 0; j < DevConsole.GetDevConsole().GetDevCommands()[i].GetParameters().Length; j++)
                {
                    parameters = parameters + DevConsole.GetDevConsole().GetDevCommands()[i].GetParameters()[j].ToString() + "| ";
                }

                string helpMessage = command + " " + parameters + DevConsole.GetDevConsole().GetCommands()[i].GetHelpMessage();
                DevConsole.GetDevConsole().NewMessage(helpMessage, false, LogLevel.BYPASS);
            }
        }
    }

    [DevCommand("Log", "Simply logs the supplied string. Useful for recording information to the logfile.")]
    public static void Log(string message)
    {
        DevConsole.GetDevConsole().NewMessage(message, false, LogLevel.BYPASS);
    }
}

#endregion

#region Internal Messages

/// <summary>
/// Class for internal messages used by DevConsole.
/// </summary>
public static class InternalMessages
{
    public static string LogLevelFormat(this LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.BYPASS:

                return "[LOG] ";

            case LogLevel.ERROR:

                return ("[" + LogLevel.ERROR.ToString() + "] ").Colour(Color.red);

            case LogLevel.WARN:

                return ("[" + LogLevel.WARN.ToString() + "] ").Colour(Color.yellow);

            case LogLevel.INFO:

                return "[" + LogLevel.INFO.ToString() + "] ";

            case LogLevel.ALL:

                return "[" + LogLevel.ALL.ToString() + "] ";

            default:

                return "[N/A]".Colour(Color.blue);
        }
    }

    public static string CommandNotFound(string command)
    {
        return ("'" + command + "' is not a valid command.").Colour(Color.red);
    }
    
    public static string IncorrectArguments(string command)
    {
        return ("Incorrect arguments passed for '" + command + "'.").Colour(Color.red);
    }
}

#endregion