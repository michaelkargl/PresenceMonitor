namespace Extensions;

public static class RandomExtensions
{
    public static byte NextByte(this Random random) => Convert.ToByte(
        random.Next(byte.MaxValue + 1)
    );

    public static Task<byte> NextByteAsync(this Random random) => Task.FromResult(random.NextByte());
}