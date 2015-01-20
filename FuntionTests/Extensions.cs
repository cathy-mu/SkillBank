using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkillBank.FunctionTests
{
    public static class Extensions
    {
        public static void AssertIsNotNullOrEmpty(this string str)
        {
            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str);
        }

        public static void AssertIsNullOrEmpty(this string str)
        {
            if (str == null || string.Empty.Equals(str))
                return;

            ThrowAssertFailedException(str, "NullOrEmpty");
        }

        private static void ThrowAssertFailedException(object obj, string exceptionMessage)
        {
            throw new AssertFailedException(string.Format("Expected:{0}; Actual:{1}", exceptionMessage, obj));
        }

        public static void AssertIsGreaterThan(this int val, int expected)
        {
            if (val > expected)
                return;

            ThrowAssertFailedException(val, string.Format("Greater than {0}", expected));
        }

        public static void AssertIsLesserThan(this int val, int expected)
        {
            if (val < expected)
                return;

            ThrowAssertFailedException(val, string.Format("Less than {0}", expected));
        }

        public static void AssertIsEqual(this int val, int expected)
        {
            Assert.AreEqual(expected, val);
        }



    }
}