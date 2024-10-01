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
        public enum ErrorsType
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        public class ErrorsManager
        {
            private ScreenLogger _logger;

            public ErrorsManager(ScreenLogger logger)
            {
                _logger = logger;
            }

            /// <summary>
            /// Internal method to register any error
            /// </summary>
            /// <param name="text">Error message</param>
            /// <param name="errType">Error severity <see cref="ErrorsType"/></param>
            protected void RegisterError(string text, ErrorsType errType)
            {
                switch (errType)
                {
                    case ErrorsType.Debug:
                        _logger.LogDebug(text);
                        break;
                    case ErrorsType.Info:
                        _logger.LogInfo(text);
                        break;
                    case ErrorsType.Warning:
                        _logger.LogWarning(text);
                        break;
                    case ErrorsType.Error:
                        _logger.LogError(text);
                        break;
                    case ErrorsType.Critical:
                        _logger.LogCritical(text);
                        break;
                }
            }

            /// <summary>
            /// Add an optional description to the last added error
            /// </summary>
            /// <param name="description">Description of the error</param>
            public void AddErrorDescription(string description) =>
                _logger.AppendLine($"- {description}");

            /// <summary>
            /// Register a new generic error
            /// </summary>
            /// <param name="error">Error message</param>
            public void AddGenericError(string error, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[GEN] {error}", errType);

            public void AddNotImplementedError(string methodName, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[MTH_NIMP] Method {methodName} not yet implemented!", errType);

            /// <summary>
            /// Register a block not found error
            /// </summary>
            /// <param name="blockName">Name used to search the block</param>
            public void AddBlockNotFoundError(string blockName, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[BLK_404] Could not find block '{blockName}'", errType);

            /// <summary>
            /// Register a group not found error
            /// </summary>
            /// <param name="groupName">Name used to search the group</param>
            public void AddGroupNotFoundError(string groupName, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[GRP_404] Could not find group '{groupName}'", errType);

            /// <summary>
            /// Register an error related to configs
            /// </summary>
            /// <param name="message">Error Message</param>
            public void AddConfigMissingError(string message, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[CONF_MISS] {message}", errType);

            /// <summary>
            /// Register a MyIni Parse Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="result">MyIni Parse Result</param>
            public void AddIniParseError(string blockName, MyIniParseResult result, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[INI_PARSE] Could not convert '{blockName}' Custom Data to MyIni!\n{result}", errType);

            /// <summary>
            /// Register a MyIni Missing Section Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="section">Expected Section</param>
            public void AddIniMissingSection(string blockName, string section, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[INI_MISS_SCT] Could not find section [{section}] in block '{blockName}'", errType);

            /// <summary>
            /// Register a MyIni Missing Key Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="section">Section in witch key was searched</param>
            /// <param name="key">Expected Key</param>
            public void AddIniMissingKey(string blockName, string section, string key, ErrorsType errType = ErrorsType.Error) =>
                RegisterError($"[INI_MISS_KEY] Could not find key '{key}'\nExpected in section [{section}] of block '{blockName}'", errType);
        }
    }
}
