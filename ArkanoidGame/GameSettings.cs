using System.Drawing;

namespace ArkanoidGame
{
    /// <summary>
    /// Класс для хранения всех глобальных настроек и констант игры.
    /// Позволяет централизованно менять баланс сложности.
    /// </summary>
    public static class GameSettings
    {
        /// <summary> Количество рядов блоков на уровне. </summary>
        public const int Rows = 5;

        /// <summary> Количество колонок блоков на уровне. </summary>
        public const int Columns = 10;

        /// <summary> Ширина одного блока в пикселях. </summary>
        public const int BlockWidth = 70;

        /// <summary> Высота одного блока в пикселях. </summary>
        public const int BlockHeight = 25;

        /// <summary> Расстояние между соседними блоками. </summary>
        public const int BlockSpacing = 5;

        /// <summary> Отступ сетки блоков от левого края формы. </summary>
        public const int BlockLeftOffset = 30;

        /// <summary> Отступ сетки блоков от верхнего края формы. </summary>
        public const int BlockTopOffset = 50;

        /// <summary> Максимальное здоровье самого прочного блока (верхний ряд). </summary>
        public const int MaxBlockHealth = 5;

        /// <summary> Уровень здоровья для блоков четвертого ряда. </summary>
        public const int HighHealth = 4;

        /// <summary> Уровень здоровья для блоков третьего ряда. </summary>
        public const int MediumHealth = 3;

        /// <summary> Уровень здоровья для блоков второго ряда. </summary>
        public const int LowHealth = 2;

        /// <summary> Начальный урон, который наносит мяч при старте игры. </summary>
        public const int InitialBallDamage = 1;

        /// <summary> Максимально возможный урон мяча после сбора усилителей. </summary>
        public const int MaxBallDamage = 3;

        /// <summary> Начальная скорость мяча по горизонтали (X). </summary>
        public const int InitialSpeedX = 4;

        /// <summary> Начальная скорость мяча по вертикали (Y). </summary>
        public const int InitialSpeedY = -4;

        /// <summary> Вероятность выпадения усилителя из разрушенного блока (в процентах). </summary>
        public const int BoosterChance = 20;

        /// <summary> Верхняя граница для генератора случайных чисел (1-100 включительно). </summary>
        public const int RandomRangeMax = 101;

        /// <summary> Скорость падения усилителя вниз к ракетке. </summary>
        public const int BoosterSpeed = 4;

        /// <summary> Физический размер графического объекта усилителя. </summary>
        public static readonly Size BoosterSize = new Size(20, 20);
    }
}