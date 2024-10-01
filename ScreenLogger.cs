using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class ScreenLogger : ScreenManager
        {
            private const float _fontSize = 0.5f;
            private readonly StringBuilder _logsString = new StringBuilder();
            private uint _debugCount = 0;
            private uint _infoCount = 0;
            private uint _warnCount = 0;
            private uint _errorCount = 0;

            /// <summary>
            /// Enables Debug logs
            /// </summary>
            public bool Debug { get; set; } = false;


            /// <summary>
            /// Logger for text based screens
            /// </summary>
            /// <param name="title">Script Name</param>
            /// <param name="version">Script Version</param>
            /// <param name="subTitle">Screen Name</param>
            /// <param name="program">Reference to main <see cref="Program"/> instance</param>
            public ScreenLogger(string title, string version, string subTitle, Program program) : base(title, version, subTitle, program.Me.GetSurface(0))
            {
                FontSize = _fontSize;
            }

            /// <summary>
            /// Logger for text based screens
            /// </summary>
            /// <param name="title">Script Name</param>
            /// <param name="version">Script Version</param>
            /// <param name="subTitle">Screen Name</param>
            /// <param name="surface">Surce to write logs on</param>
            public ScreenLogger(string title, string version, string subTitle, IMyTextSurface surface) : base(title, version, subTitle, surface)
            {
                FontSize = _fontSize;
            }

            /// <summary>
            /// Logger for text based screens
            /// </summary>
            /// <param name="title">Script Name</param>
            /// <param name="version">Script Version</param>
            /// <param name="subTitle">Screen Name</param>
            /// <param name="provider">Provider of text surface to use</param>
            /// <param name="surfaceIndex">Index of text surface</param>
            public ScreenLogger(string title, string version, string subTitle, IMyTextSurfaceProvider provider, int surfaceIndex = 0) : base(title, version, subTitle, provider, surfaceIndex)
            {
                FontSize = _fontSize;
            }



            private void WriteLog(string message)
            {
                ClearScreen();
                AppendLine($"Info: {_infoCount:000} Warnings: {_warnCount:000} Errors: {_errorCount:00} {(Debug ? $"Debug: {_debugCount:0000}" : "")}");
                _logsString.AppendLine(CheckIfTextFits(message) ? message : message.Insert(message.Length * 3/4, "\n"));
                AddContent(_logsString.ToString());
            }

            /// <summary>
            /// Log Debug message
            /// </summary>
            /// <param name="message">Message to log</param>
            public void LogDebug(string message)
            {
                if (!Debug)
                    return;

                WriteLog($"[DEBUG] {message}");
                _debugCount++;
            }

            /// <summary>
            /// Log Info message
            /// </summary>
            /// <param name="message">Message to log</param>
            public void LogInfo(string message)
            {
                WriteLog($"[INFO] {message}");
                _infoCount++;
            }

            /// <summary>
            /// Log Warn message
            /// </summary>
            /// <param name="message">Message to log</param>
            public void LogWarning(string message)
            {
                WriteLog($"[WARN] {message}");
                _warnCount++;
            }

            /// <summary>
            /// Log Error message
            /// </summary>
            /// <param name="message">Message to log</param>
            public void LogError(string message)
            {
                WriteLog($"[ERR] {message}");
                _errorCount++;
            }

            /// <summary>
            /// Log Critical message and throw an exception
            /// </summary>
            /// <param name="message">Message to log</param>
            public void LogCritical(string message)
            {
                WriteLog($"[CRT] {message}");
                throw new Exception(message);
            }
        }
    }
}
