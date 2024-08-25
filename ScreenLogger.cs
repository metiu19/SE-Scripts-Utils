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
            public bool Debug { get; set; } = false;

            public ScreenLogger(Program program, string subTitle, IMyTextSurface surface) : base(program, subTitle, surface)
            {
                FontSize = _fontSize;
            }

            public ScreenLogger(Program program, string subTitle, IMyTextSurfaceProvider provider, int surfaceIndex = 0) : base(program, subTitle, provider, surfaceIndex)
            {
                FontSize = _fontSize;
            }

            public void LogDebug(string message) {
                if (Debug)
                    AppendLine($"[DEBUG] {message}");
            }

            public void LogInfo(string message) =>
                AppendLine($"[INFO] {message}");

            public void LogWaring(string message) =>
                AppendLine($"[WARN] {message}");

            public void LogError(string message) =>
                AppendLine($"[ERR] {message}");

            public void LogCritical(string message) =>
                AppendLine($"[CRT] {message}");
        }
    }
}
