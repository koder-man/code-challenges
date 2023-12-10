namespace AoCCore;

internal class Calc
{
    public static long GCF(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long LCM(long a, long b)
    {
        return a / GCF(a, b) * b;
    }

    public static long LCM(params long[] input)
    {
        var a = input[0];
        for (int i = 1; i < input.Length; i++)
        {
            a = LCM(a, input[i]);
        }
        return a;
    }
}