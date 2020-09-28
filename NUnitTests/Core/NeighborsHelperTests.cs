using System;

using Core.Helpers;
using Core.Models;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class NeighborsHelperTests
    {

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        public void NeighborsFinderException(int dimention)
        {
            Assert.Throws<ArgumentException>(() => new NeighborsHelper(dimention));
            Assert.Throws<ArgumentException>(() => new NeighborsHelper(dimention));
        }

        [TestCase(2)]
        public void NeighborsFinder_Dim_2(int dimention)
        {
            // 0(-)|1(+)
            // 2(+)|3(-)

            var finder = new NeighborsHelper(dimention);

            // Zero row
            Assert.AreEqual(finder[0], new CellNeighbors(-1, -1, -1,  3));
            Assert.AreEqual(finder[1], new CellNeighbors(-1,  2, -1, -1));

            // First row
            Assert.AreEqual(finder[2], new CellNeighbors(-1, -1,  1, -1));
            Assert.AreEqual(finder[3], new CellNeighbors( 0, -1, -1, -1));
        }

        [TestCase(3)]
        public void NeighborsFinder_Dim_3(int dimention)
        {
            // 0(-)|1(+)|2(-)
            // 3(+)|4(-)|5(+)
            // 6(-)|7(+)|8(-)

            Assert.Throws<ArgumentException>(() => new NeighborsHelper(dimention));

            //var finder = new NeighborsHelper(dimention);

            //// Zero row
            //Assert.AreEqual(finder[1], new CellNeighbors(-1, 3, -1, 5));

            //// First row
            //Assert.AreEqual(finder[3], new CellNeighbors(-1, -1, 1, 7));
            //Assert.AreEqual(finder[4], new CellNeighbors( 0,  6, 2, 8));

            //// Second row
            //Assert.AreEqual(finder[7], new CellNeighbors(3, -1, 5, -1));
        }

        [TestCase(8)]
        public void NeighborsFinder_Dim_8(int dimention)
        {
            //  0(-)| 1(+)| 2(-)| 3(+)| 4(-)| 5(+)| 6(-)| 7(+)
            //  8(+)| 9(-)|10(+)|11(-)|12(+)|13(-)|14(+)|15(-)
            // 16(-)|17(+)|18(-)|19(+)|20(-)|21(+)|22(-)|23(+)
            // 24(+)|25(-)|26(+)|27(-)|28(+)|29(-)|30(+)|31(-)
            // 32(-)|33(+)|34(-)|35(+)|36(-)|37(+)|38(-)|39(+)
            // 40(+)|41(-)|42(+)|43(-)|44(+)|45(-)|46(+)|47(-)
            // 48(-)|49(+)|50(-)|51(+)|52(-)|53(+)|54(-)|55(+)
            // 56(+)|57(-)|58(+)|59(-)|60(+)|61(-)|62(+)|63(-)

            var finder = new NeighborsHelper(dimention);

            // Zero row
            Assert.AreEqual(finder[0], new CellNeighbors(-1, -1, -1, 9));
            Assert.AreEqual(finder[7], new CellNeighbors(-1, 14, -1, -1));

            // First row
            Assert.AreEqual(finder[9], new CellNeighbors(0, 16, 2, 18));
            Assert.AreEqual(finder[14], new CellNeighbors(5, 21, 7, 23));

            // Second row
            Assert.AreEqual(finder[18], new CellNeighbors(9, 25, 11, 27));
            Assert.AreEqual(finder[21], new CellNeighbors(12, 28, 14, 30));

            // Third row
            Assert.AreEqual(finder[27], new CellNeighbors(18, 34, 20, 36));
            Assert.AreEqual(finder[28], new CellNeighbors(19, 35, 21, 37));

            // Third row
            Assert.AreEqual(finder[3, 3], new CellNeighbors(18, 34, 20, 36));
            Assert.AreEqual(finder[3, 4], new CellNeighbors(19, 35, 21, 37));
        }

        [TestCase(8)]
        public void NeighborsFinder_IsNeighbors(int dimention)
        {
            //  0(-)| 1(+)| 2(-)| 3(+)| 4(-)| 5(+)| 6(-)| 7(+)
            //  8(+)| 9(-)|10(+)|11(-)|12(+)|13(-)|14(+)|15(-)
            // 16(-)|17(+)|18(-)|19(+)|20(-)|21(+)|22(-)|23(+)
            // 24(+)|25(-)|26(+)|27(-)|28(+)|29(-)|30(+)|31(-)
            // 32(-)|33(+)|34(-)|35(+)|36(-)|37(+)|38(-)|39(+)
            // 40(+)|41(-)|42(+)|43(-)|44(+)|45(-)|46(+)|47(-)
            // 48(-)|49(+)|50(-)|51(+)|52(-)|53(+)|54(-)|55(+)
            // 56(+)|57(-)|58(+)|59(-)|60(+)|61(-)|62(+)|63(-)

            var finder = new NeighborsHelper(dimention);

            Assert.IsFalse(finder.IsNeighbors(8, 7));
            Assert.IsFalse(finder.IsNeighbors(0, 0));
            Assert.IsFalse(finder.IsNeighbors(24, 8));
            Assert.IsFalse(finder.IsNeighbors(-1, 0));
            Assert.IsFalse(finder.IsNeighbors(0, -1));
            Assert.IsFalse(finder.IsNeighbors(-1, -1));

            Assert.IsTrue(finder.IsNeighbors(35, 26));
            Assert.IsTrue(finder.IsNeighbors(35, 28));
            Assert.IsTrue(finder.IsNeighbors(35, 42));
            Assert.IsTrue(finder.IsNeighbors(35, 44));

            Assert.IsFalse(finder.IsNeighbors(35, 27));
            Assert.IsFalse(finder.IsNeighbors(35, 34));
            Assert.IsFalse(finder.IsNeighbors(35, 36));
            Assert.IsFalse(finder.IsNeighbors(35, 43));
        }

        [TestCase(8)]
        public void NeighborsFinder_GetSmthCell(int dimention)
        {
            //  0(-)| 1(+)| 2(-)| 3(+)| 4(-)| 5(+)| 6(-)| 7(+)
            //  8(+)| 9(-)|10(+)|11(-)|12(+)|13(-)|14(+)|15(-)
            // 16(-)|17(+)|18(-)|19(+)|20(-)|21(+)|22(-)|23(+)
            // 24(+)|25(-)|26(+)|27(-)|28(+)|29(-)|30(+)|31(-)
            // 32(-)|33(+)|34(-)|35(+)|36(-)|37(+)|38(-)|39(+)
            // 40(+)|41(-)|42(+)|43(-)|44(+)|45(-)|46(+)|47(-)
            // 48(-)|49(+)|50(-)|51(+)|52(-)|53(+)|54(-)|55(+)
            // 56(+)|57(-)|58(+)|59(-)|60(+)|61(-)|62(+)|63(-)

            var finder = new NeighborsHelper(dimention);

            // default deep
            Assert.AreEqual(finder.GetLeftTopCell(26),  17);
            Assert.AreEqual(finder.GetLeftBotCell(26),  33);
            Assert.AreEqual(finder.GetRightTopCell(26), 19);
            Assert.AreEqual(finder.GetRightBotCell(26), 35);

            // deep = -1
            Assert.AreEqual(finder.GetLeftTopCell(26, -1), -1);

            // deep = 2
            Assert.AreEqual(finder.GetLeftTopCell(26, 2),  8);
            Assert.AreEqual(finder.GetLeftBotCell(26, 2),  40);
            Assert.AreEqual(finder.GetRightTopCell(26, 2), 12);
            Assert.AreEqual(finder.GetRightBotCell(26, 2), 44);

            // deep = 3
            Assert.AreEqual(finder.GetLeftTopCell(26, 3),  -1);
            Assert.AreEqual(finder.GetLeftBotCell(26, 3),  -1);
            Assert.AreEqual(finder.GetRightTopCell(26, 3), 5);
            Assert.AreEqual(finder.GetRightBotCell(26, 3), 53);
        }

    }
}
