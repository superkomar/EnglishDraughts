using System;
using System.Collections.Generic;

using Core.Enums;
using Core.Models;
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

            field[1] = field[17] = field[49] = CellState.BlackMan;
            field[3] = field[19] = field[51] = CellState.BlackKing;

            field[21] = field[53] = field[60] = CellState.WhiteMan;
            field[23] = field[55] = field[62] = CellState.WhiteKing;

            field[14] = CellState.WhiteMan;

            field[24] = CellState.WhiteKing;
            field[26] = CellState.WhiteMan;
            field[28] = CellState.BlackKing;
            field[30] = CellState.BlackMan;

            _gameField = new GameField(field, new NeighborsFinder(Dimension), Dimension);
        }

        [Test]
        public void ModelsCreator_CreateNewGameField()
        {
            Assert.Throws<ArgumentException>(() => FieldUtils.CreateField(-1));
            Assert.Throws<ArgumentException>(() => FieldUtils.CreateField(0));

            var field_1 = FieldUtils.CreateField(1);
            Assert.AreEqual(field_1.CellCount, 1);
            Assert.AreEqual(field_1[0], CellState.Empty);

            var field_2 = FieldUtils.CreateField(2);
            Assert.AreEqual(field_2.CellCount, 4);
            Assert.AreEqual(field_2[0], CellState.Empty);
            Assert.AreEqual(field_2[1], CellState.Empty);
            Assert.AreEqual(field_2[2], CellState.Empty);
            Assert.AreEqual(field_2[3], CellState.Empty);

            var field_3 = FieldUtils.CreateField(3);
            Assert.AreEqual(field_3.CellCount, 9);
            Assert.AreEqual(field_3[1], CellState.Empty);
            Assert.AreEqual(field_3[3], CellState.Empty);
            Assert.AreEqual(field_3[7], CellState.Empty);

            var field_4 = FieldUtils.CreateField(4);
            Assert.AreEqual(field_4.CellCount, 16);
            Assert.AreEqual(field_4[1],  CellState.BlackMan);
            Assert.AreEqual(field_4[4],  CellState.Empty);
            Assert.AreEqual(field_4[9],  CellState.Empty);
            Assert.AreEqual(field_4[12], CellState.WhiteMan);

            var field_8 = FieldUtils.CreateField(8);
            Assert.AreEqual(field_8.CellCount, 64);
            Assert.AreEqual(field_8[1],  CellState.BlackMan);
            Assert.AreEqual(field_8[8],  CellState.BlackMan);
            Assert.AreEqual(field_8[17], CellState.BlackMan);
            Assert.AreEqual(field_8[24], CellState.Empty);
            Assert.AreEqual(field_8[33], CellState.Empty);
            Assert.AreEqual(field_8[40], CellState.WhiteMan);
            Assert.AreEqual(field_8[49], CellState.WhiteMan);
            Assert.AreEqual(field_8[56], CellState.WhiteMan);
        }

        [Test]
        public void ModelsCreator_CreateSimpleGameTurn()
        {
            // incorrect
            foreach (var playerSide in new[] { PlayerSide.Black, PlayerSide.White })
            {
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide,  1,  1), null);
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide, -1,  1), null);
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide,  1, -1), null);
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide,  1, -1), null);
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide,  40, 33), null);
                Assert.AreEqual(TurnUtils.CreateTurnByCells(_gameField, playerSide,  33, 40), null);
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
            var srcTurn = TurnUtils.CreateTurnByCells(field, side, start, end);

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
            Assert.AreEqual(first.Steps, second.Steps);
        }
    }
}
