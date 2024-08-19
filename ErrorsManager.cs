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
        public class ErrorsManager
        {
            private readonly Program _program;
            private readonly string _title;
            private readonly string _version;
            private readonly StringBuilder _errors = new StringBuilder();
            private readonly IMyTextSurface _surface;


            /// <summary>
            /// The number of Exceptions
            /// </summary>
            public int Count { get; private set; } = 0;

            /// <summary>
            /// A string containing all the exceptions messages.
            /// See also <seealso cref="ResetErrors"/>
            /// </summary>
            public string ErrorsMessage { get { return _errors.ToString(); } }

            /// <summary>
            /// PrintErrors logic, override defualt logic.
            /// To access errors data see <see cref="ErrorsMessage"/> and <seealso cref="Count"/>.
            /// To reset errors see <see cref="ResetErrors"/>
            /// </summary>
            public Func<bool> PrintErrorsLogic;



            /// <summary>
            /// Errors Manager with pb main screen as output surface. For use with other screens use overrides
            /// </summary>
            /// <param name="program">Program instance</param>
            public ErrorsManager(Program program)
            {
                _program = program;
                _title = _program.Title;
                _version = _program.Version;
                _surface = _program.Me.GetSurface(0);
                _surface.ContentType = ContentType.TEXT_AND_IMAGE;
                _surface.BackgroundColor = Color.Black;
                _surface.FontColor = Color.White;
                _surface.FontSize = 0.65f;
                _surface.TextPadding = 1f;
                PrintErrorsLogic = DefaultPrintLogic;
            }

            /// <summary>
            /// Errors Manager with configruable output surface
            /// </summary>
            /// <param name="program">Program instace</param>
            /// <param name="surface">Surface to print error messages to</param>
            public ErrorsManager(Program program, IMyTextSurface surface)
            {
                _program = program;
                _title = program.Title;
                _version = program.Version;
                _surface = surface;
                _surface.ContentType = ContentType.TEXT_AND_IMAGE;
                PrintErrorsLogic = DefaultPrintLogic;
            }

            /// <summary>
            /// Errors Manager with configruable output surface
            /// </summary>
            /// /// <param name="program">Program instace</param>
            /// <param name="surfaceProvider">Surface Provider of output screen</param>
            /// <param name="index">Surface index</param>
            public ErrorsManager(Program program, IMyTextSurfaceProvider surfaceProvider, int index = 0)
            {
                _program = program;
                _title = program.Title;
                _version = program.Version;
                _surface = surfaceProvider.GetSurface(index);
                _surface.ContentType = ContentType.TEXT_AND_IMAGE;
                PrintErrorsLogic = DefaultPrintLogic;
            }


            private bool DefaultPrintLogic()
            {
                _surface.WriteText($"{_title} {_version} - {DateTime.Now:HH:mm:ss} - Errors\n", false);

                if (Count == 0)
                {
                    _surface.WriteText("\nNo Errors!!!", true);
                    return false;
                }

                _surface.WriteText($"Count: {Count}\n\n", true);
                _surface.WriteText(_errors.ToString(), true);

                _errors.Clear();
                Count = 0;
                return true;
            }

            /// <summary>
            /// Internal Method to register an error, used by this class and child classes
            /// </summary>
            /// <param name="errText">Error Text to add</param>
            protected void RegisterError(string errText)
            {
                _errors.AppendLine(errText);
                Count++;
            }

            /// <summary>
            /// Prints any errors registered and reset the error list
            /// </summary>
            /// <returns>If any error message was displayed</returns>
            public bool PrintErrors() =>
                PrintErrorsLogic.Invoke();

            /// <summary>
            /// Prints any errors registered and throw an exception
            /// </summary>
            /// <param name="exceptionMessage">Exception message</param>
            /// <returns>If any error message was displayed</returns>
            public bool PrintErrorsAndThrowException(string exceptionMessage)
            {
                if (!PrintErrorsLogic.Invoke())
                    return false;

                throw new Exception(exceptionMessage);
            }

            /// <summary>
            /// Resets errors string and counter.
            /// Use with <see cref="ErrorsMessage"/> to have custom print logic
            /// </summary>
            public void ResetErrors()
            {
                _errors.Clear();
                Count = 0;
            }


            /// <summary>
            /// Add an optional description to the last added error
            /// </summary>
            /// <param name="description">Description of the error</param>
            public void AddErrorDescription(string description) =>
                _errors.AppendLine($"- {description}");

            /// <summary>
            /// Register a new generic error
            /// </summary>
            /// <param name="error">Error message</param>
            public void AddGenericError(string error) =>
                RegisterError($"[ERR_GEN] {error}");

            public void AddNotImplementedError(string methodName) =>
                RegisterError($"[ERR_MTH_NIMP] Method {methodName} not yet implemented!");

            /// <summary>
            /// Register a block not found error
            /// </summary>
            /// <param name="blockName">Name used to search the block</param>
            public void AddBlockNotFoundError(string blockName) =>
                RegisterError($"[ERR_BLK_404] Could not find block '{blockName}'");

            /// <summary>
            /// Register a group not found error
            /// </summary>
            /// <param name="groupName">Name used to search the group</param>
            public void AddGroupNotFoundError(string groupName) =>
                RegisterError($"[ERR_GRP_404] Could not find group '{groupName}'");

            /// <summary>
            /// Register an error related to configs
            /// </summary>
            /// <param name="message">Error Message</param>
            public void AddConfigMissingError(string message) =>
                RegisterError($"[ERR_CONF_MISS] {message}");

            /// <summary>
            /// Register a MyIni Parse Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="result">MyIni Parse Result</param>
            public void AddIniParseError(string blockName, MyIniParseResult result) =>
                RegisterError($"[ERR_INI_PARSE] Could not convert '{blockName}' Custom Data to MyIni!\n{result}");

            /// <summary>
            /// Register a MyIni Missing Section Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="section">Expected Section</param>
            public void AddIniMissingSection(string blockName, string section) =>
                RegisterError($"[ERR_INI_MISS_SCT] Could not find section [{section}] in block '{blockName}'");

            /// <summary>
            /// Register a MyIni Missing Key Error
            /// </summary>
            /// <param name="blockName">Name of the block from witch Custom Data was taken</param>
            /// <param name="section">Section in witch key was searched</param>
            /// <param name="key">Expected Key</param>
            public void AddIniMissingKey(string blockName, string section, string key) =>
                RegisterError($"[ERR_INI_MISS_KEY] Could not find key '{key}'\nExpected in section [{section}] of block '{blockName}'");
        }
    }
}
