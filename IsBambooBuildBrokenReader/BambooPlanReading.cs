using System;

namespace IsBambooBuildBrokenReader
{
    public class ResultCheckpoint
    {
        public readonly int BuildNumber;

        public ResultCheckpoint(int buildNumber)
        {
            this.BuildNumber = buildNumber;
        }

        public static bool operator <=(ResultCheckpoint left, ResultCheckpoint right)
        {
            return !(left > right);
        }

        public static bool operator >=(ResultCheckpoint left, ResultCheckpoint right)
        {
            return !(left < right);
        }

        public static bool operator <(ResultCheckpoint left, ResultCheckpoint right)
        {
            return left.BuildNumber < right.BuildNumber;
        }

        public static bool operator >(ResultCheckpoint left, ResultCheckpoint right)
        {
            return left.BuildNumber > right.BuildNumber;
        }

        public override string ToString()
        {
            return BuildNumber.ToString();
        }
    }


}