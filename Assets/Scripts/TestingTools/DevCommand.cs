using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Apply this attribute to a method to have it as a command in the developer console.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class DevCommand : Attribute
{
    string command;
    string callbackMessage;

    public string GetCommand() { return command; }
    public string GetCallbackMessage() { return callbackMessage; }

    /// <summary>
    /// Create a command for the developer console.
    /// </summary>
    /// <param name="command"> The command's name used to call it. </param>
    public DevCommand(string command)
    {
        this.command = command;
    }

    /// <summary>
    /// Create a command for the developer console with a callback message.
    /// </summary>
    /// <param name="command"> The command's name used to call it. </param>
    /// <param name="callbackMessage"> The command's callback message upon executing command. Supports rich text. </param>
    public DevCommand(string command, string callbackMessage)
    {
        this.command = command;
        this.callbackMessage = callbackMessage;
    }
}