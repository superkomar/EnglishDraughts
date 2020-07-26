using System;

using Core.Helpers;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class NeighborsHelperTests
    {

        [TestCase(0)]
        [TestCase(-1)]
        public void NeighborsFinderException(int dimention)
        {
            Assert.Throws<ArgumentException>(() => new NeighborsHelper(dimention));
        }

        [TestCase(1)]
        public void NeighborsFinder_Dim_1(int dimention)
        {
            var finder = new NeighborsHelper(dimention);

            Assert.AreEqual(finder[0], new CellNeighbors(-1, -1, -1, -1));
        }

        [TestCase(2)]
        public void NeighborsFinder_Dim_2(int dimention)
        {
            // 0(w) 1(b)
            // 2(b) 3(w)

            var finder = new NeighborsHelper(dimention);

            // Zero row
            Assert.AreEqual(finder[0], new CellNeighbors(-1, -1, -1, 3));
            Assert.AreEqual(finder[1], new CellNeighbors(-1, 2, -1, -1));

            // First row
            Assert.AreEqual(finder[2], new CellNeighbors(-1, -1, 1, -1));
            Assert.AreEqual(finder[3], new CellNeighbors(0, -1, -1, -1));
        }

        [TestCase(3)]
        public void NeighborsFinder_Dim_3(int dimention)
        {
            // 0(w)|1(b)|2(w)
            // 3(b)|4(w)|5(b)
            // 6(w)|7(b)|8(w)

            var finder = new NeighborsHelper(dimention);

            // Zero row
            Assert.AreEqual(finder[1], new CellNeighbors(-1, 3, -1, 5));

            // First row
            Assert.AreEqual(finder[3], new CellNeighbors(-1, -1, 1, 7));
            Assert.AreEqual(finder[4], new CellNeighbors(0, 6, 2, 8));

            // Second row
            Assert.AreEqual(finder[7], new CellNeighbors(3, -1, 5, -1));
        }

        [TestCase(8)]
        public void NeighborsFinder_Dim_8(int dimention)
        {
            //  0(w)| 1(b)| 2(w)| 3(b)| 4(w)| 5(b)| 6(w)| 7(b)
            //  8(b)| 9(w)|10(b)|11(w)|12(b)|13(w)|14(b)|15(w)
            // 16(w)|17(b)|18(w)|19(b)|20(w)|21(b)|22(w)|23(b)
            // 24(b)|25(w)|26(b)|27(w)|28(b)|29(w)|30(b)|31(w)
            // 32(w)|33(b)|34(w)|35(b)|36(w)|37(b)|38(w)|39(b)
            // 40(b)|41(w)|42(b)|43(w)|44(b)|45(w)|46(b)|47(w)
            // 48(w)|49(b)|50(w)|51(b)|52(w)|53(b)|54(w)|55(b)
            // 56(b)|57(w)|58(b)|59(w)|60(b)|61(w)|62(b)|63(w)

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
            //  0(w)| 1(b)| 2(w)| 3(b)| 4(w)| 5(b)| 6(w)| 7(b)
            //  8(b)| 9(w)|10(b)|11(w)|12(b)|13(w)|14(b)|15(w)
            // 16(w)|17(b)|18(w)|19(b)|20(w)|21(b)|22(w)|23(b)
            // 24(b)|25(w)|26(b)|27(w)|28(b)|29(w)|30(b)|31(w)
            // 32(w)|33(b)|34(w)|35(b)|36(w)|37(b)|38(w)|39(b)
            // 40(b)|41(w)|42(b)|43(w)|44(b)|45(w)|46(b)|47(w)
            // 48(w)|49(b)|50(w)|51(b)|52(w)|53(b)|54(w)|55(b)
            // 56(b)|57(w)|58(b)|59(w)|60(b)|61(w)|62(b)|63(w)

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
            //  0(w)| 1(b)| 2(w)| 3(b)| 4(w)| 5(b)| 6(w)| 7(b)
            //  8(b)| 9(w)|10(b)|11(w)|12(b)|13(w)|14(b)|15(w)
            // 16(w)|17(b)|18(w)|19(b)|20(w)|21(b)|22(w)|23(b)
            // 24(b)|25(w)|26(b)|27(w)|28(b)|29(w)|30(b)|31(w)
            // 32(w)|33(b)|34(w)|35(b)|36(w)|37(b)|38(w)|39(b)
            // 40(b)|41(w)|42(b)|43(w)|44(b)|45(w)|46(b)|47(w)
            // 48(w)|49(b)|50(w)|51(b)|52(w)|53(b)|54(w)|55(b)
            // 56(b)|57(w)|58(b)|59(w)|60(b)|61(w)|62(b)|63(w)

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
