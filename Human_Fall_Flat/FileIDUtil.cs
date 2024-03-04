using System;
using System.Security.Cryptography;
using System.Text;

public static class FileIDUtil
{
	public static int Compute(Type t)
	{
		string s = "s\0\0\0" + t.Namespace + t.Name;
		using HashAlgorithm hashAlgorithm = new MD4();
		byte[] array = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
		int num = 0;
		for (int num2 = 3; num2 >= 0; num2--)
		{
			num <<= 8;
			num |= array[num2];
		}
		return num;
	}
}
