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
        public class ScreenManager
        {
            private readonly Program _program;
            private readonly string _title;
            private readonly string _version;
            private readonly string _subTitle;
            private readonly IMyTextSurface _surface;
            private float _width;


            /// <summary>
            /// Gets or sets background color
            /// </summary>
            public Color BackgroundColor
            {
                get { return _surface?.BackgroundColor ?? default(Color); }
                set { if (_surface != null) _surface.BackgroundColor = value; }
            }

            /// <summary>
            /// Gets or sets the font
            /// </summary>
            public string Font
            {
                get { return _surface?.Font; }
                set { if (_surface != null) _surface.Font = value; }
            }

            /// <summary>
            /// Gets or sets font size
            /// </summary>
            public float FontSize
            {
                get { return _surface?.FontSize ?? default(float); }
                set { if (_surface != null) _surface.FontSize = value; }
            }

            /// <summary>
            /// Gets or sets font color
            /// </summary>
            public Color FontColor
            {
                get { return _surface?.FontColor ?? default(Color); }
                set { if (_surface != null) _surface.FontColor = value; }
            }

            /// <summary>
            /// Gets or sets text padding from all sides of the panel
            /// </summary>
            public float TexturePadding
            {
                get { return _surface?.TextPadding ?? default(float); }
                set { if (_surface != null) _surface.TextPadding = value; }
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="program">Program instance</param>
            /// <param name="subTitle">Screen specific subtitle</param>
            /// <param name="surface">Surface to manage</param>
            public ScreenManager(Program program, string subTitle, IMyTextSurface surface)
            {
                _program = program;
                _title = _program.Title;
                _version = _program.Version;
                _subTitle = subTitle;
                _surface = surface;
                if (_surface != null)
                    InitSurface();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="program">Program instace</param>
            /// <param name="subTitle">Screen specific subtitle</param>
            /// <param name="surfaceProvider">Block providing the surface to write on</param>
            /// <param name="surfaceIndex">Surface index</param>
            public ScreenManager(Program program, string subTitle, IMyTextSurfaceProvider surfaceProvider, int surfaceIndex = 0)
            {
                _program = program;
                _title = _program.Title;
                _version = _program.Version;
                _subTitle = subTitle;
                _surface = surfaceProvider?.GetSurface(surfaceIndex);
                if (_surface != null)
                    InitSurface();
            }

            private void InitSurface()
            {
                _surface.ContentType = ContentType.TEXT_AND_IMAGE;
                _surface.BackgroundColor = Color.Black;
                _surface.FontColor = Color.White;
                _surface.FontSize = 0.8f;
                _width = _surface.SurfaceSize.X;
                ClearScreen();
            }


            /// <summary>
            /// Clear screen content and write default title
            /// </summary>
            public void ClearScreen() =>
                _surface?.WriteText($"{_title} {_version} - {DateTime.Now:HH:mm:ss} - {_subTitle}\n");

            /// <summary>
            /// Append <paramref name="text"/> to the content already on screen.
            /// </summary>
            /// <param name="text">String to append</param>
            public void AppendLine(string text)
            {
                if (_surface == null)
                    return;
                StringBuilder sb = new StringBuilder(text);
                float len = _surface.MeasureStringInPixels(sb, Font, FontSize).X;
                if (len > _width)
                    sb.Insert(sb.Length*3/4, "\n");
                _surface.WriteText($"{sb}\n", true);
            }

            /// <summary>
            /// Clears the screen and wirte the given <paramref name="text"/>
            /// </summary>
            /// <param name="text">Text to print</param>
            public void OverwriteContent(string text)
            {
                ClearScreen();
                AppendLine(text);
            }
        }
    }
}
