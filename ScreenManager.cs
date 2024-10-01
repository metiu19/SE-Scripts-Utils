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
            private readonly string _title;
            private readonly string _version;
            private readonly string _subTitle;
            private readonly IMyTextSurface _surface;
            private readonly StringBuilder _sb = new StringBuilder();
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
            /// Text based screens utility class
            /// </summary>
            /// <param name="title">Script name</param>
            /// <param name="version">Script version</param>
            /// <param name="subTitle">Screen specific subtitle</param>
            /// <param name="surface">Surface to manage</param>
            public ScreenManager(string title, string version, string subTitle, IMyTextSurface surface)
            {
                _title = title;
                _version = version;
                _subTitle = subTitle;
                _surface = surface;
                if (_surface != null)
                    InitSurface();
            }

            /// <summary>
            /// Text based screens utility class
            /// </summary>
            /// <param name="title">Script name</param>
            /// <param name="version">Script version</param>
            /// <param name="subTitle">Screen specific subtitle</param>
            /// <param name="surfaceProvider">Block providing the surface to write on</param>
            /// <param name="surfaceIndex">Surface index</param>
            public ScreenManager(string title, string version, string subTitle, IMyTextSurfaceProvider surfaceProvider, int surfaceIndex = 0)
            {
                _title = title;
                _version = version;
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
            /// Clears screen content and write default header
            /// </summary>
            public void ClearScreen()
            {
                if (_surface == null)
                    return;

                _surface?.WriteText($"{_title} {_version} - {DateTime.Now:HH:mm:ss} - {_subTitle}\n");
            }

            /// <summary>
            /// <para>Clears the screen and writes the given <paramref name="text"/></para>
            /// <para>IMPORTANT: Does not print header!</para>
            /// </summary>
            /// <param name="text">Text to print</param>
            public void OverwriteContent(string text)
            {
                if (_surface == null)
                    return;

                _surface.WriteText($"{text}\n", false);
            }

            /// <summary>
            /// Appends <paramref name="text"/> to the content already on screen.
            /// </summary>
            /// <param name="text">Text to append</param>
            public void AddContent(string text)
            {
                if (_surface == null)
                    return;
                _surface.WriteText($"{text}\n", true);
            }

            /// <summary>
            /// <para>Appends <paramref name="text"/> to the content already on screen.</para>
            /// <para>IMPORTANT: If the <paramref name="text"/> is too long it gets splitted</para>
            /// </summary>
            /// <param name="text">Text to append</param>
            public void AppendLine(string text)
            {
                if (_surface == null)
                    return;

                _sb.Clear();
                _sb.Append(text);
                float len = _surface.MeasureStringInPixels(_sb, Font, FontSize).X;
                if (len > _width)
                    _sb.Insert(_sb.Length * 3 / 4, "\n");
                _surface.WriteText($"{_sb}\n", true);
            }

            /// <summary>
            /// <para>Appends <paramref name="text"/> to the content already on screen.</para>
            /// <para>IMPORTANT: If the <paramref name="text"/> is too long it gets splitted</para>
            /// </summary>
            /// <param name="text">Text to append</param>
            public void AppendLine(StringBuilder sb)
            {
                if (_surface == null)
                    return;

                float len = _surface.MeasureStringInPixels(sb, Font, FontSize).X;
                if (len > _width)
                    _sb.Insert(_sb.Length * 3 / 4, "\n");
                _surface.WriteText($"{sb}\n", true);
            }

            /// <summary>
            /// Checks if a given <paramref name="text"/> fits in the screen
            /// </summary>
            /// <param name="text">Text to check</param>
            /// <returns><see cref="true"/> if <paramref name="text"/> fits, <see cref="false"/> otherwise</returns>
            public bool CheckIfTextFits(string text)
            {
                _sb.Clear();
                _sb.Append(text);
                float len = _surface.MeasureStringInPixels(_sb, Font, FontSize).X;
                return len < _width;
            }

            /// <summary>
            /// <para>Checks if a given <paramref name="text"/> fits in the screen</para>
            /// <para>Use provided <see cref="StringBuilder"/> instead of an internal one to save on conversions</para>
            /// </summary>
            /// <param name="text">Text to check</param>
            /// <returns><see cref="true"/> if <paramref name="text"/> fits, <see cref="false"/> otherwise</returns>
            public bool CheckIfTextFits(StringBuilder sb)
            {
                float len = _surface.MeasureStringInPixels(sb, Font, FontSize).X;
                return len < _width;
            }
        }
    }
}
