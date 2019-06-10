﻿using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TrippyGL
{
    /// <summary>
    /// Encapsulates a simple way to store vertex data in a single buffer without needing to care abount a VertexArray
    /// </summary>
    /// <typeparam name="T">The type of vertex to use. Must be a struct and implement IVertex</typeparam>
    public class VertexBuffer<T> : GraphicsResource where T : struct, IVertex
    {
        /// <summary>The buffer object containing the vertex data</summary>
        public VertexDataBufferObject<T> DataBuffer { get; }

        /// <summary>The vertex array used for attrib specification</summary>
        public VertexArray VertexArray { get; }

        /// <summary>The length of the buffer object's storage, measured in elements</summary>
        public int StorageLength { get { return DataBuffer.StorageLength; } }

        /// <summary>The size of each element measured in bytes</summary>
        public int ElementSize { get { return DataBuffer.ElementSize; } }

        /// <summary>
        /// Creates a VertexBuffer with the specified storage length and initializes the storage data by copying it from a specified index of a given array.
        /// The entire storage must be filled with data, it cannot be only partly written by this constructor
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice this resource will use</param>
        /// <param name="storageLength">The length (measured in elements) of the buffer storage</param>
        /// <param name="dataOffset">The first element of the given data array to start reading from</param>
        /// <param name="data">An array containing the data to be uploaded to the buffer's storage. Can't be null</param>
        /// <param name="usageHint">The buffer hint is used by the graphics driver to optimize performance depending on the use that will be given to the buffer object</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, int storageLength, int dataOffset, T[] data, BufferUsageHint usageHint)
            : base(graphicsDevice)
        {
            DataBuffer = new VertexDataBufferObject<T>(graphicsDevice, storageLength, dataOffset, data, usageHint);
            VertexArray = new VertexArray(graphicsDevice, DataBuffer, data[0].AttribDescriptions);
        }

        /// <summary>
        /// Creates a VertexBuffer with the specified storage length and initializes the storage data by copying it from a given array.
        /// The entire storage must be filled with data, it cannot be only partly written by this constructor
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice this resource will use</param>
        /// <param name="storageLength">The length (measured in elements) of the buffer storage</param>
        /// <param name="data">An array containing the data to be uploaded to the buffer's storage. Can't be null</param>
        /// <param name="usageHint">The buffer hint is used by the graphics driver to optimize performance depending on the use that will be given to the buffer object</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, int storageLength, T[] data, BufferUsageHint usageHint)
            : base(graphicsDevice)
        {
            DataBuffer = new VertexDataBufferObject<T>(graphicsDevice, storageLength, data, usageHint);
            VertexArray = new VertexArray(graphicsDevice, DataBuffer, data[0].AttribDescriptions);
        }

        /// <summary>
        /// Creates a VertexBuffer and initializes the storage data by copying it from a given array.
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice this resource will use</param>
        /// <param name="data">An array with the data to be uploaded to the buffer's storage. Can't be null</param>
        /// <param name="usageHint">The buffer hint is used by the graphics driver to optimize performance depending on the use that will be given to the buffer object</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, T[] data, BufferUsageHint usageHint) : this(graphicsDevice, data.Length, data, usageHint)
        {

        }

        /// <summary>
        /// Creates a VertexBuffer with the specified storage length. The storage is created but the data has no specified initial value
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice this resource will use</param>
        /// <param name="storageLength">The length (measured in elements) of the buffer storage</param>
        /// <param name="usageHint">The buffer hint is used by the graphics driver to optimize performance depending on the use that will be given to the buffer object</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, int storageLength, BufferUsageHint usageHint)
            : base(graphicsDevice)
        {
            DataBuffer = new VertexDataBufferObject<T>(graphicsDevice, storageLength, usageHint);
            VertexArray = new VertexArray(graphicsDevice, DataBuffer, new T().AttribDescriptions);
        }

        /// <summary>
        /// Ensure the vertex data buffer object is the currently bound one to GL_ARRAY_TARGET
        /// </summary>
        public void EnsureBufferBound()
        {
            GraphicsDevice.BindBuffer(DataBuffer);
        }

        /// <summary>
        /// Binds the vertex data buffer object. Prefer using EnsureBufferBound() to avoid unnecessary binds
        /// </summary>
        public void BindBuffer()
        {
            GraphicsDevice.BindBuffer(DataBuffer);
        }

        /// <summary>
        /// Ensure that the VertexArray is the currently bound one, and binds it otherwise
        /// </summary>
        public void EnsureArrayBound()
        {
            GraphicsDevice.BindVertexArray(VertexArray);
        }

        /// <summary>
        /// Binds the VertexArray. Prefer using EnsureArrayBound() to avoid unnecessary binds
        /// </summary>
        public void BindArray()
        {
            GraphicsDevice.ForceBindVertexArray(VertexArray);
        }

        public override string ToString()
        {
            return String.Concat("DataBuffer: { ", DataBuffer.ToString(), " }, VertexArray={ ", VertexArray.ToString(), " }");
        }

        protected override void Dispose(bool isManualDispose)
        {
            if (isManualDispose)
            {
                DataBuffer.Dispose();
                VertexArray.Dispose();
            }
            base.Dispose(isManualDispose);
        }
    }
}
