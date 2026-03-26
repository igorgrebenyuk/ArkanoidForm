namespace ArkanoidGame
{
    /// <summary>
    /// Константы игровой логики, скоростей и параметров баланса.
    /// </summary>
    public static class GameConstants
    {
        /// <summary> Количество рядов блоков. </summary>
        public const int Rows = 5;

        /// <summary> Количество колонок блоков. </summary>
        public const int Columns = 10;

        /// <summary> Ширина одного блока. </summary>
        public const int BlockWidth = 70;

        /// <summary> Высота одного блока. </summary>
        public const int BlockHeight = 25;

        /// <summary> Расстояние между соседними блоками. </summary>
        public const int BlockSpacing = 5;

        /// <summary> Отступ сетки блоков от левого края формы. </summary>
        public const int BlockLeftOffset = 30;

        /// <summary> Отступ сетки блоков от верхнего края формы. </summary>
        public const int BlockTopOffset = 50;


        /// <summary> Максимальное здоровье блока (верхний ряд). </summary>
        public const int MaxBlockHealth = 5;

        /// <summary> Здоровье блоков четвертого ряда. </summary>
        public const int HighHealth = 4;

        /// <summary> Здоровье блоков третьего ряда. </summary>
        public const int MediumHealth = 3;

        /// <summary> Здоровье блоков второго ряда. </summary>
        public const int LowHealth = 2;


        /// <summary> Начальный урон мяча. </summary>
        public const int InitialBallDamage = 1;

        /// <summary> Максимальный урон мяча при сборе усилителей. </summary>
        public const int MaxBallDamage = 3;

        /// <summary> Начальная скорость мяча по оси X. </summary>
        public const int InitialSpeedX = 4;

        /// <summary> Начальная скорость мяча по оси Y. </summary>
        public const int InitialSpeedY = -4;


        /// <summary> Шанс выпадения усилителя (в процентах). </summary>
        public const int BoosterChance = 20;

        /// <summary> Скорость падения усилителя. </summary>
        public const int BoosterSpeed = 4;

        /// <summary> Минимальное значение диапазона случайных чисел. </summary>
        public const int RandomRangeMin = 1;

        /// <summary> Максимальное значение диапазона случайных чисел. </summary>
        public const int RandomRangeMax = 101;


        /// <summary> Делитель для расчета угла отскока от ракетки. </summary>
        public const int PaddleHitDivisor = 15;
    }
}