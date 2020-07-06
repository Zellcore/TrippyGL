﻿using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using TrippyGL;
using Silk.NET.OpenGL;
using TrippyTestBase;

namespace ComplexVertexFormats
{
    // Renders two triangles on a black background using a highly unusual vertex format.
    // You should see two right triangles that look like a rectangle got split up diagonally.
    // The bottom-left and top-right vertices should be red, the top-left vertices should be
    // blue and the bottom-right vertices should be green.

    // The ComplexVertex type requires 16 vertex attrib indices, so if your GPU has less than
    // that this will fail when creating the VertexArray.
    // My GTX 765m supports no more than 16 vertex attrib indices in case you're wondering.

    class ComplexVertexFormats : TestBase
    {
        VertexBuffer<ComplexVertex> vertexBuffer;
        ShaderProgram shaderProgram;

        protected override void OnLoad()
        {
            Span<ComplexVertex> vertices = stackalloc ComplexVertex[]
            {
                new ComplexVertex(new Vector3(-0.6f, -0.6f, 0), new Color4b(255, 0, 0, 255)),
                new ComplexVertex(new Vector3(0.4f, -0.6f, 0), new Color4b(0, 255, 0, 255)),
                new ComplexVertex(new Vector3(-0.6f, 0.4f, 0), new Color4b(0, 0, 255, 255)),

                new ComplexVertex(new Vector3(0.6f, -0.4f, 0), new Color4b(0, 255, 0, 255)),
                new ComplexVertex(new Vector3(-0.4f, 0.6f, 0), new Color4b(0, 0, 255, 255)),
                new ComplexVertex(new Vector3(0.6f, 0.6f, 0), new Color4b(255, 0, 0, 255)),
            };

            vertexBuffer = new VertexBuffer<ComplexVertex>(graphicsDevice, vertices, BufferUsageARB.StaticDraw);

            ShaderProgramBuilder programBuilder = new ShaderProgramBuilder();
            programBuilder.VertexShaderCode = File.ReadAllText("vs1.glsl");
            programBuilder.FragmentShaderCode = File.ReadAllText("fs1.glsl");
            programBuilder.SpecifyVertexAttribs<ComplexVertex>(new string[] { "sixtyThree", "X", "nothing0", "colorR", "matrix1", "colorG", "sixtyFour", "Y", "colorB", "Z", "oneTwoThreeFour", "alwaysZero", "alsoZero" });
            shaderProgram = programBuilder.Create(graphicsDevice, true);
            Console.WriteLine("VS Log: " + programBuilder.VertexShaderLog);
            Console.WriteLine("FS Log: " + programBuilder.FragmentShaderLog);
            Console.WriteLine("Program Log: " + programBuilder.ProgramLog);

            shaderProgram.Uniforms["Projection"].SetValueMat4(Matrix4x4.Identity);

            graphicsDevice.BlendingEnabled = false;
            graphicsDevice.DepthTestingEnabled = false;
        }

        protected override void OnRender(double dt)
        {
            graphicsDevice.ClearColor = new Vector4(0, 0, 0, 1);
            graphicsDevice.Clear(ClearBufferMask.ColorBufferBit);

            graphicsDevice.VertexArray = vertexBuffer;
            graphicsDevice.ShaderProgram = shaderProgram;
            graphicsDevice.DrawArrays(PrimitiveType.Triangles, 0, vertexBuffer.StorageLength);

            Window.SwapBuffers();
        }

        protected override void OnResized(Size size)
        {
            if (size.Width == 0 || size.Height == 0)
                return;

            graphicsDevice.SetViewport(0, 0, (uint)size.Width, (uint)size.Height);
        }

        protected override void OnUnload()
        {
            vertexBuffer.Dispose();
            shaderProgram.Dispose();
            graphicsDevice.Dispose();
        }
    }
}