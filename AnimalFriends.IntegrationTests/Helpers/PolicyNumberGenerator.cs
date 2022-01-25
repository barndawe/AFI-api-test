using System;
using System.Linq;
using System.Text;

namespace AnimalFriends.IntegrationTests.Helpers;

public static class PolicyNumberGenerator
{
    private const string PolicyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string PolicyNums = "0123456789";
    
    public static string GenerateValidPolicyNumber()
    {
        return $"{GeneratePolicyAlphaPart(2)}-{GeneratePolicyNumericPart(6)}";
    }
    
    private static string GeneratePolicyAlphaPart(int length)
    {
        var random = new Random();
        return new string(Enumerable.Repeat(PolicyChars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string GeneratePolicyNumericPart(int length)
    {
        var random = new Random();
        return new string(Enumerable.Repeat(PolicyNums, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}