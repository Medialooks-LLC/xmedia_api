using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static Avalonia.OpenGL.GlConsts;
using static Medialooks.XMedia.FrameFunctions;
using static Medialooks.XMedia.Structures;

// Not sure where to place it
namespace OpenGLAvalonia.Views
{
    public class OpenGlVideoControl : OpenGlControlBase
    {
        private int _textureId;
        private int _program;
        private int _vertexBuffer;
        private int _uvBuffer;
        private int _vertexArray;
        public int frameCounter = 0;

        public const int GL_TEXTURE_WRAP_S = 0x2802;
        public const int GL_TEXTURE_WRAP_T = 0x2803;
        public const int GL_REPEAT = 0x2901;
        public const int GL_CLAMP_TO_EDGE = 0x812F;

        private int _textureWidth = 1280;
        private int _textureHeight = 720;
        public long start_time = 0;
        private XmFrame? _frame;
        private XmFrame? _oldFrame;

        public double DpiScale { get; set; }

        public void UpdateFrame(XmFrame? xmFrame)
        {
            _oldFrame = _frame;
            _frame = xmFrame;
            RequestNextFrameRendering();
        }

        private static void CheckError(GlInterface gl, int line = 0)
        {
            int err;
            while ((err = gl.GetError()) != GL_NO_ERROR)
                Debug.WriteLine($"{line} check err: {err}");
        }

        private string GetShader(bool fragment, string shader)
        {
            //get something like 410
            var version = GlVersion.Major.ToString() + GlVersion.Minor.ToString() + "0";

            var data = $"#version {version} {(GlVersion.Type == GlProfileType.OpenGL ? "core" : "es")}\n";
            if (GlVersion.Type == GlProfileType.OpenGLES)
                data += "precision highp float;\n";
            if ((GlVersion.Major * 100 + GlVersion.Minor * 10) < 150)
            {
                throw new Exception("Shaders was created for newer version of OpenGL (1.5 and higher)");
            }

            data += shader;

            return data;
        }

        private string VertexShaderSource => GetShader(false, @"
                    layout(location = 0) in vec2 aPosition;
                    layout(location = 1) in vec2 aTexCoord;
                    out vec2 vTexCoord;
                    void main() {
                        gl_Position = vec4(aPosition, 0.0, 1.0);
                        vTexCoord = aTexCoord;
                    }
        ");

        private string FragmentShaderSource => GetShader(true, @"
                    in vec2 vTexCoord;
                    out vec4 FragColor;
                    uniform sampler2D uTexture;
                    uniform float uAspectRatio; // texture/viewport aspect ratio
                    uniform float uMode; // 0 - fill, 1 - fit, 2 - cover

                    void main() {
                        vec2 texCoord = vTexCoord;

                        if (uMode == 1.0) { // Fit
                            if (uAspectRatio > 1.0) {
                                // Wide texture
                                texCoord.x = (texCoord.x - 0.5) * uAspectRatio + 0.5;
                                if (texCoord.x < 0.0 || texCoord.x > 1.0) {
                                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                                    return;
                                }
                            } else {
                                // Tall texture
                                texCoord.y = (texCoord.y - 0.5) / uAspectRatio + 0.5;
                                if (texCoord.y < 0.0 || texCoord.y > 1.0) {
                                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                                    return;
                                }
                            }
                        }
                        else if (uMode == 2.0) { // Cover
                            if (uAspectRatio > 1.0) {
                                // Wide texture
                                texCoord.y = (texCoord.y - 0.5) / uAspectRatio + 0.5;
                            } else {
                                // Tall texture
                                texCoord.x = (texCoord.x - 0.5) * uAspectRatio + 0.5;
                            }
                        }
                        // uMode == 0.0 - Fill (by default)

                        FragColor = texture(uTexture, texCoord);
                    }
        ");

        protected override unsafe void OnOpenGlInit(GlInterface gl)
        {
            base.OnOpenGlInit(gl);

            CheckError(gl);

            string info = $"Renderer: {gl.GetString(GL_RENDERER)} Version: {gl.GetString(GL_VERSION)}";
            Debug.WriteLine(info);

            var vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
            var error = gl.CompileShaderAndGetError(vertexShader, VertexShaderSource);
            Debug.WriteLine(error);

            var fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
            error = gl.CompileShaderAndGetError(fragmentShader, FragmentShaderSource);
            Debug.WriteLine(error);

            _program = gl.CreateProgram();
            gl.AttachShader(_program, vertexShader);
            gl.AttachShader(_program, fragmentShader);

            error = gl.LinkProgramAndGetError(_program);
            Debug.WriteLine(error);
            CheckError(gl, 5);

            _textureId = gl.GenTexture();
            gl.BindTexture(GL_TEXTURE_2D, _textureId);

            gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
            gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            float[] vertices = [
                -1.0f, -1.0f,
                1.0f, -1.0f,
                -1.0f, 1.0f,

                1.0f, -1.0f,
                1.0f, 1.0f,
                -1.0f, 1.0f ];
            float[] uvs = [
                0.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 0.0f,

                1.0f, 1.0f,
                1.0f, 0.0f,
                0.0f, 0.0f
            ];

            _vertexArray = gl.GenVertexArray();
            gl.BindVertexArray(_vertexArray);

            _vertexBuffer = gl.GenBuffer();
            gl.BindBuffer(GL_ARRAY_BUFFER, _vertexBuffer);
            fixed (float* pVertices = vertices)
                gl.BufferData(GL_ARRAY_BUFFER, (nint)(vertices.Length * sizeof(float)), (nint)pVertices, GL_STATIC_DRAW);
            gl.EnableVertexAttribArray(0);
            gl.VertexAttribPointer(0, 2, GL_FLOAT, 0/*false*/, 0, IntPtr.Zero);

            _uvBuffer = gl.GenBuffer();
            gl.BindBuffer(GL_ARRAY_BUFFER, _uvBuffer);
            fixed (float* pUvs = uvs)
                gl.BufferData(GL_ARRAY_BUFFER, (nint)(uvs.Length * sizeof(float)), (nint)pUvs, GL_STATIC_DRAW);
            gl.EnableVertexAttribArray(1);
            gl.VertexAttribPointer(1, 2, GL_FLOAT, 0/*false*/, 0, IntPtr.Zero);

            gl.BindVertexArray(0);
            Debug.WriteLine($"Texture size: {_textureWidth}x{_textureHeight}");
        }

        protected override void OnOpenGlDeinit(GlInterface gl)
        {
            gl.DeleteTexture(_textureId);
            gl.DeleteProgram(_program);
            gl.DeleteBuffer(_vertexBuffer);
            gl.DeleteBuffer(_uvBuffer);
            gl.DeleteVertexArray(_vertexArray);
            base.OnOpenGlDeinit(gl);
        }

        protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
        {
            //For debug purposes
            //TODO: move to 180 line, clean if frameData not null
            gl.ClearColor(0.3f, 0.3f, 0.8f, 1.0f);
            gl.Clear(GL_COLOR_BUFFER_BIT);

            if (_frame != null)
            {
                var frameData = XmFrameVideoDataGet(_frame);
                if (frameData == null)
                {
                    return;
                }
                _textureWidth = frameData.format.width;
                _textureHeight = frameData.format.height;

                //gl.Viewport(0, 0, _textureWidth, _textureHeight);
                gl.Viewport(0, 0, (int)(Bounds.Width * DpiScale), (int)(Bounds.Height * DpiScale));

                if (frameData == null) return;

                gl.BindTexture(GL_TEXTURE_2D, _textureId);

                nint firstPlanePtr = Marshal.ReadIntPtr(frameData.p_planes, 0);

                if (frameCounter % 100 == 0)
                {
                    long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds() - start_time;
                    var expected_time = 1000 * frameCounter * frameData.format.frame_rate.den / frameData.format.frame_rate.num;
                    Debug.WriteLine($"Frame {frameCounter} at {milliseconds} but should be {expected_time}. Delta: {milliseconds - expected_time}");
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    gl.TexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8,
                                 _textureWidth, _textureHeight, 0,
                                 GL_BGRA, GL_UNSIGNED_BYTE, firstPlanePtr);
                }
                else
                {
                    gl.TexImage2D(GL_TEXTURE_2D, 0, GL_BGRA,
                                 _textureWidth, _textureHeight, 0,
                                 GL_BGRA, GL_UNSIGNED_BYTE, firstPlanePtr);
                }

                gl.UseProgram(_program);

                double textureAspect = (double)_textureWidth / (double)_textureHeight;
                double viewportAspect = Bounds.Width / Bounds.Height;
                float aspectRatio = (float)(viewportAspect / textureAspect);
                float mode = 1.0f; // 0 - fill, 1 - fit, 2 - cover
                var aspectRatioStr = Marshal.StringToHGlobalAnsi("uAspectRatio");
                var modeStr = Marshal.StringToHGlobalAnsi("uMode");

                try
                {
                    int aspectRatioLocation = gl.GetUniformLocation(_program, aspectRatioStr);
                    int modeLocation = gl.GetUniformLocation(_program, modeStr);

                    if (aspectRatioLocation != -1)
                    {
                        gl.Uniform1f(aspectRatioLocation, aspectRatio);
                    }

                    if (modeLocation != -1)
                    {
                        gl.Uniform1f(modeLocation, mode);
                    }
                }
                finally
                {
                    // Освобождаем память
                    Marshal.FreeHGlobal(aspectRatioStr);
                    Marshal.FreeHGlobal(modeStr);
                }

                gl.BindVertexArray(_vertexArray);
                gl.DrawArrays(GL_TRIANGLES, 0, 6);
                gl.BindVertexArray(0);

                frameCounter++;
                XmFrameRelease(_oldFrame);
            }
            CheckError(gl, 19);
        }
    }
}