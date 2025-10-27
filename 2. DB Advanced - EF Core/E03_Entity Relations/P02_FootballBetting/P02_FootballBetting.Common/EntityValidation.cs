namespace P02_FootballBetting.Common
{
    public static class EntityValidation
    {
        public static class Team
        {
            public const int NameMaxLength = 100;

            public const int LogoUrlMaxLength = 2048;

            public const int InitialsMaxLength = 5;

            public const string BudgetColumnType = "DECIMAL(13,5)";
        }
        public static class Color
        {
            public const int NameMaxLength = 30;
        }

        public static class Town
        {
            public const int NameMaxLength = 85;
        }

        public static class Country
        {
            public const int NameMaxLength = 56;
        }

        public static class Player
        {
            public const int NameMaxLength = 60;
        }

        public static class Position
        {
            public const int NameMaxLength = 30;
        }

        public static class Game
        {
            public const string BetRateColumnType = "DECIMAL(8,3)"; // 99 999.999
            public const int ResultMaxLength = 7;
        }

        public static class Bet
        {
            public const string AmountColumnType = "DECIMAL(12,5)";
        }

        public static class User
        {
            public const int UsernameMaxLength = 30;

            public const int NameMaxLength = 80;

            public const int PasswordMaxLength = 512;

            public const int EmailMaxLength = 320;

            public const string BalanceColumnType = "DECIMAL(13,5)";
        }
    }
}
