using System;
using System.Collections.Generic;

using Core.Enums;
using Core.Helpers;
using Core.Model;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.Core
{
    class ModelsCreatorTests
    {
        private const int Dimension = 8;

        private GameField _gameField;

        [OneTimeSetUp]
        public void GenerateField()
        {
            //  0(-)| 1(+)| 2(-)| 3(+)| 4(-)| 5(+)| 6(-)| 7(+)
            //  8(+)| 9(-)|10(+)|11(-)|12(+)|13(-)|14(+)|15(-)
            // 16(-)|17(+)|18(-)|19(+)|20(-)|21(+)|22(-)|23(+)
            // 24(+)|25(-)|26(+)|27(-)|28(+)|29(-)|30(+)|31(-)
            // 32(-)|33(+)|34(-)|35(+)|36(-)|37(+)|38(-)|39(+)
            // 40(+)|41(-)|42(+)|43(-)|44(+)|45(-)|46(+)|47(-)
            // 48(-)|49(+)|50(-)|51(+)|52(-)|53(+)|54(-)|55(+)
            // 56(+)|57(-)|58(+)|59(-)|60(+)|61(-)|62(+)|63(-)

            var field = new List<CellState>();
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    field.Add(CellState.Empty);
                }
            }

            field[1] = field[17] = field[49] = CellState.BlackMen;
            field[3] = field[19] = field[51] = CellState.BlackKing;

            field[21] = field[53] = field[60] = CellState.WhiteMen;
            field[23] = field[55] = field[62] = CellState.WhiteKing;

            field[14] = CellState.WhiteMen;

            field[24] = CellState.WhiteKing;
            field[26] = CellState.WhiteMen;
            field[28] = CellState.BlackKing;
            field[30] = CellState.BlackMen;

            _gameField = new GameField(field, new NeighborsHelper(Dimension), Dimension);
        }

        [Test]
        public void ModelsCreator_CreateNewGameField()
        {
            Assert.Throws<ArgumentException>(() => ModelsCreator.CreateGameField(-1));
            Assert.Throws<ArgumentException>(() => ModelsCreator.CreateGameField(0));

            var field_1 = ModelsCreator.CreateGameField(1);
            Assert.AreEqual(field_1.Field.Count, 1);
            Assert.AreEqual(field_1.Field[0], CellState.Empty);

            var field_2 = ModelsCreator.CreateGameField(2);
            Assert.AreEqual(field_2.Field.Count, 4);
            Assert.AreEqual(field_2.Field[0], CellState.Empty);
            Assert.AreEqual(field_2.Field[1], CellState.Empty);
            Assert.AreEqual(field_2.Field[2], CellState.Empty);
            Assert.AreEqual(field_2.Field[3], CellState.Empty);

            var field_3 = ModelsCreator.CreateGameField(3);
            Assert.AreEqual(field_3.Field.Count, 9);
            Assert.AreEqual(field_3.Field[1], CellState.Empty);
            Assert.AreEqual(field_3.Field[3], CellState.Empty);
            Assert.AreEqual(field_3.Field[7], CellState.Empty);

            var field_4 = ModelsCreator.CreateGameField(4);
            Assert.AreEqual(field_4.Field.Count, 16);
            Assert.AreEqual(field_4.Field[1],  CellState.BlackMen);
            Assert.AreEqual(field_4.Field[4],  CellState.Empty);
            Assert.AreEqual(field_4.Field[9],  CellState.Empty);
            Assert.AreEqual(field_4.Field[12], CellState.WhiteMen);

            var field_8 = ModelsCreator.CreateGameField(8);
            Assert.AreEqual(field_8.Field.Count, 64);
            Assert.AreEqual(field_8.Field[1],  CellState.BlackMen);
            Assert.AreEqual(field_8.Field[8],  CellState.BlackMen);
            Assert.AreEqual(field_8.Field[17], CellState.BlackMen);
            Assert.AreEqual(field_8.Field[24], CellState.Empty);
            Assert.AreEqual(field_8.Field[33], CellState.Empty);
            Assert.AreEqual(field_8.Field[40], CellState.WhiteMen);
            Assert.AreEqual(field_8.Field[49], CellState.WhiteMen);
            Assert.AreEqual(field_8.Field[56], CellState.WhiteMen);
        }

        [Test]
        public void ModelsCreator_CreateSimpleGameTurn()
        {
            // incorrect
            foreach (var playerSide in new[] { PlayerSide.Black, PlayerSide.White })
            {
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide,  1,  1), null);
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide, -1,  1), null);
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide,  1, -1), null);
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide,  1, -1), null);
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide,  40, 33), null);
                Assert.AreEqual(ModelsCreator.CreateGameTurn(_gameField, playerSide,  33, 40), null);
            }

            // Black pieces
            PlayerSide side = PlayerSide.Black;

            CreateSimpleTurns(_gameField, side, 1,  8, mustNull: false);
            CreateSimpleTurns(_gameField, side, 1, 10, mustNull: false);

            CreateSimpleTurns(_gameField, side, 3, 10, mustNull: false);
            CreateSimpleTurns(_gameField, side, 3, 12, mustNull: false);

            CreateSimpleTurns(_gameField, side, 17,  8, mustNull: true);
            CreateSimpleTurns(_gameField, side, 17, 10, mustNull: true);
            CreateSimpleTurns(_gameField, side, 17, 24, mustNull: true);
            CreateSimpleTurns(_gameField, side, 17, 26, mustNull: true);

            CreateSimpleTurns(_gameField, side, 19, 10, mustNull: false);
            CreateSimpleTurns(_gameField, side, 19, 12, mustNull: false);
            CreateSimpleTurns(_gameField, side, 19, 26, mustNull: true);
            CreateSimpleTurns(_gameField, side, 19, 28, mustNull: true);

            CreateSimpleTurns(_gameField, side, 49, 56, mustNull: false, isLevelUp: true);
            CreateSimpleTurns(_gameField, side, 49, 58, mustNull: false, isLevelUp: true);

            CreateSimpleTurns(_gameField, side, 51, 58, mustNull: false, isLevelUp: false);

            CreateSimpleTurns(_gameField, PlayerSide.White, 19, 10, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.White, 19, 12, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.White, 19, 26, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.White, 19, 28, mustNull: true);

            // White pieces
            side = PlayerSide.White;

            CreateSimpleTurns(_gameField, side, 24, 17, mustNull: true);
            CreateSimpleTurns(_gameField, side, 24, 33, mustNull: false);

            CreateSimpleTurns(_gameField, side, 26, 17, mustNull: true);
            CreateSimpleTurns(_gameField, side, 26, 19, mustNull: true);
            CreateSimpleTurns(_gameField, side, 26, 33, mustNull: true);
            CreateSimpleTurns(_gameField, side, 26, 35, mustNull: true);

            CreateSimpleTurns(_gameField, PlayerSide.Black, 26, 17, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.Black, 26, 19, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.Black, 26, 33, mustNull: true);
            CreateSimpleTurns(_gameField, PlayerSide.Black, 26, 35, mustNull: true);

            CreateSimpleTurns(_gameField, side, 14, 5, mustNull: false, isLevelUp: true);
            CreateSimpleTurns(_gameField, side, 14, 7, mustNull: false, isLevelUp: true);

            CreateSimpleTurns(_gameField, side, 53, 44, mustNull: false);
            CreateSimpleTurns(_gameField, side, 53, 46, mustNull: false);
            CreateSimpleTurns(_gameField, side, 53, 60, mustNull: true);
            CreateSimpleTurns(_gameField, side, 53, 62, mustNull: true);

            CreateSimpleTurns(_gameField, side, 55, 46, mustNull: false);
            CreateSimpleTurns(_gameField, side, 55, 62, mustNull: true);
        }

        private void CreateSimpleTurns(GameField field, PlayerSide side, int start, int end, bool mustNull, bool isLevelUp = false)
        {
            var srcTurn = ModelsCreator.CreateGameTurn(field, side, start, end);

            if (mustNull) Assert.AreEqual(srcTurn, null);
            else CompareTurns(srcTurn, new GameTurn(side, isLevelUp, new[] { start, end }));
        }

        private void CompareTurns(GameTurn first, GameTurn second)
        {
            Assert.NotNull(first);
            Assert.NotNull(second);

            Assert.AreEqual(first.Side, second.Side);
            Assert.AreEqual(first.IsSimple, second.IsSimple);
            Assert.AreEqual(first.IsLevelUp, second.IsLevelUp);
            Assert.AreEqual(first.Turns, second.Turns);
        }
    }
}
