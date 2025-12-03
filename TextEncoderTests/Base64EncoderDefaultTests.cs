using System;
using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests
{
	[TestFixture]
	public class Base64EncoderDefaultTests : GenericTestBase<Base64Encoder>
	{
		public Base64EncoderDefaultTests()
		{
			this.Subject = Base64Encoder.Default;
		}

		public static IEnumerable TestStrings
		{
			get
			{
				for (int i = 0; i < Count; i++) yield return new TestCaseData(Convert.ToBase64String(RandomData[i]));
			}
		}

		public static IEnumerable TestData
		{
			get
			{
				for (int i = 0; i < Count; i++) yield return new TestCaseData(RandomData[i]);
			}
		}

		[Test, TestCaseSource(nameof(TestData))]
		public void RoundTrip(byte[] d)
		{
			CollectionAssert.AreEqual(d, this.Subject!.FromBase(this.Subject!.ToBase(d)));
		}

		[Test, TestCaseSource(nameof(TestData))]
		public void ToBase64(byte[] d)
		{
			Assert.AreEqual(Convert.ToBase64String(d), this.Subject!.ToBase(d));
		}

		[Test, TestCaseSource(nameof(TestStrings))]
		public void FromBase64(string s)
		{
			CollectionAssert.AreEqual(Convert.FromBase64String(s), this.Subject!.FromBase(s));
		}
	}
}
