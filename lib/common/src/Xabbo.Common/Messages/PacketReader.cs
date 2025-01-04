using System;
using System.Buffers.Binary;
using System.Text;

namespace Xabbo.Messages;

/// <summary>
/// Provides primitive packet read operations.
/// </summary>
public readonly ref struct PacketReader(IPacket packet, ref int pos, IParserContext? context = null)
{
    private readonly IPacket Packet = packet;
    public readonly ref int Pos = ref pos;
    public IParserContext? Context => context;
    public Header Header => Packet.Header;
    public ClientType Client => Packet.Client;
    public ReadOnlySpan<byte> Span => Packet.Buffer.Span;
    public int Length => Packet.Length;
    public int Available => Packet.Length - Pos;
    public Encoding Encoding => Encoding.Latin1;

    public PacketReader(IPacket packet) : this(packet, ref packet.Position) { }

    /// <summary>
    /// Reads a <see cref="Span{T}"/> of bytes of length <paramref name="n"/> from the current position and advances it.
    /// </summary>
    /// <param name="n">The number of bytes to read.</param>
    /// <exception cref="IndexOutOfRangeException">If the current position plus the specified length exceeds the length of the packet.</exception>
    public ReadOnlySpan<byte> ReadSpan(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        if (Pos + n > Span.Length)
            throw new IndexOutOfRangeException($"Attempted to read past the packet length: {n} bytes from position {Pos} when length is {Length}.");
        Pos += n;
        return Span[(Pos - n)..Pos];
    }

    /// <summary>
    /// Reads a <see cref="bool"/> from the current position and advances it.
    /// <para/>
    /// Decoded as a <see cref="VL64"/> on Shockwave, otherwise as a <see cref="byte"/> .
    /// </summary>
    public bool ReadBool() => Client switch
    {
        _ => ReadSpan(1)[0] != 0
    };

    /// <summary>
    /// Reads a <see cref="byte"/> from the current position and advances it.
    /// <para/>
    /// Not supported on Shockwave.
    /// </summary>
    /// <exception cref="UnsupportedClientException">If the <see cref="Client"/> is <see cref="ClientType.Shockwave"/>.</exception>
    public byte ReadByte() => Client switch
    {
        _ => ReadSpan(1)[0]
    };

    /// <summary>
    /// Reads a <see cref="short"/> from the current position and advances it.
    /// <para/>
    /// Decoded as a <see cref="B64"/> on Shockwave, otherwise as a 16-bit integer.
    /// </summary>
    public short ReadShort() => Client switch
    {
        _ => BinaryPrimitives.ReadInt16BigEndian(ReadSpan(2)),
    };

    /// <summary>
    /// Reads a short array from the current position and advances it.
    /// </summary>
    public short[] ReadShortArray()
    {
        short[] array = new short[ReadLength()];
        for (int i = 0; i < array.Length; i++)
            array[i] = ReadShort();
        return array;
    }

    /// <summary>
    /// Reads an <see cref="int"/> from the current position and advances it.
    /// <para/>
    /// Decoded as a <see cref="VL64"/> on Shockwave, otherwise as a 32-bit integer.
    /// </summary>
    public int ReadInt() => BinaryPrimitives.ReadInt32BigEndian(ReadSpan(4));

    /// <summary>
    /// Reads an int array from the current position and advances it.
    /// </summary>
    public int[] ReadIntArray()
    {
        int[] array = new int[ReadLength()];
        for (int i = 0; i < array.Length; i++)
            array[i] = ReadInt();
        return array;
    }

    /// <summary>
    /// Reads a <see cref="float"/> from the current position and advances it.
    /// <para/>
    /// Read as a <see cref="string"/> on Flash and Shockwave, otherwise decoded as a 32-bit floating point number.
    /// </summary>
    public float ReadFloat() => (float)(FloatString)ReadString();



    /// <summary>
    /// Reads a <see cref="string"/> from the current position and advances it.
    /// <para/>
    /// On Shockwave, when the header direction is incoming, it is decoded as a sequence of characters terminated by a <c>0x02</c> byte.
    /// <para/>
    /// Otherwise, it is decoded as a <see cref="short"/> length-prefixed UTF-8 <see cref="string"/>.
    /// </summary>
    public string ReadString()
    {
        return Encoding.GetString(ReadSpan((ushort)ReadShort()));
    }

    /// <summary>
    /// Reads a string array from the current position and advances it.
    /// </summary>
    public string[] ReadStringArray()
    {
        string[] array = new string[ReadLength()];
        for (int i = 0; i < array.Length; i++)
            array[i] = ReadString();
        return array;
    }



    /// <summary>
    /// Reads a <see cref="Length"/> from the current position and advances it.
    /// <para/>
    /// Read as a <see cref="short"/> on Unity, or an <see cref="int"/> on Flash and Shockwave.
    /// </summary>
    /// <exception cref="UnsupportedClientException">If the client type is invalid.</exception>
    public Length ReadLength() => (Length)ReadInt();


    /// <summary>
    /// Parses a <typeparamref name="T"/> from the current position and advances it.
    /// </summary>
    public T Parse<T>() where T : IParser<T> => T.Parse(in this);

    /// <summary>
    /// Parses an array of <typeparamref name="T"/> from the current position and advances it.
    /// </summary>
    public T[] ParseArray<T>() where T : IParser<T>
    {
        T[] array = new T[ReadLength()];
        for (int i = 0; i < array.Length; i++)
            array[i] = Parse<T>();
        return array;
    }
}
