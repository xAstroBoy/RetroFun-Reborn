﻿using System;

namespace Xabbo.Messages;

/// <summary>
/// Represents a packet of data with a message header.
/// </summary>
public interface IPacket : IDisposable
{
    /// <summary>
    /// Gets or sets the message header of the packet.
    /// </summary>
    Header Header { get; set; }

    /// <summary>
    /// Gets the client type of the packet's header.
    /// </summary>
    ClientType Client { get; }

    /// <summary>
    /// Gets the context this packet was intercepted in.
    /// </summary>
    IParserContext? Context { get; }

    /// <summary>
    /// Gets the packet's buffer.
    /// </summary>
    PacketBuffer Buffer { get; }

    /// <summary>
    /// Gets a reference to the current position in the packet.
    /// </summary>
    ref int Position { get; }

    /// <summary>
    /// Gets the length of the packet.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Creates a new reader for this packet at the current position.
    /// </summary>
    PacketReader Reader();

    /// <summary>
    /// Creates a new reader for this packet at the specified position.
    /// </summary>
    PacketReader ReaderAt(ref int pos);

    /// <summary>
    /// Creates a new writer for this packet at the current position.
    /// </summary>
    PacketWriter Writer();

    /// <summary>
    /// Creates a new writer for this packet at the specified position.
    /// </summary>
    PacketWriter WriterAt(ref int pos);

    /// <inheritdoc cref="PacketWriter.Allocate(int)" />
    Span<byte> Allocate(int n);

    /// <inheritdoc cref="PacketReader.ReadSpan(int)" />
    ReadOnlySpan<byte> ReadSpan(int n);

    /// <inheritdoc cref="PacketWriter.WriteSpan(ReadOnlySpan{byte})" />
    void WriteSpan(ReadOnlySpan<byte> bytes);

    /// <summary>
    /// Clears the packet's buffer and resets its position.
    /// </summary>
    void Clear();

    /// <summary>
    /// Creates a copy of this packet.
    /// </summary>
    IPacket Copy();
}
