using System;
using System.Runtime.CompilerServices;

using Core;
using Core.Model;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class NeighborsFinderTests
    {

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(-1)]
        public void NeighborsFinderException(int dimention)
        {
            Assert.Throws<ArgumentException>(() => new CellNeighborsHelper(dimention));
        }

        [TestCase(2)]
        public void NeighborsFinderForTwo(int dimention)
        {
            var finder = new CellNeighborsHelper(dimention);

            Assert.That(finder[0], Is.EquivalentTo(new[] { 1 }));
            Assert.That(finder[1], Is.EquivalentTo(new[] { 0 }));
        }

        [TestCase(8)]
        public void NeighborsFinderForEight(int dimention)
        {
            var finder = new CellNeighborsHelper(dimention);

            // Zero row
            Assert.That(finder[0], Is.EquivalentTo(new[] { 4 }));
            Assert.That(finder[3], Is.EquivalentTo(new[] { 6, 7 }));

            // First row
            Assert.That(finder[4], Is.EquivalentTo(new[] { 0, 1, 8, 9 }));
            Assert.That(finder[7], Is.EquivalentTo(new[] { 3, 11 }));

            // Second row
            Assert.That(finder[8], Is.EquivalentTo(new[] { 4, 12 }));
            Assert.That(finder[11], Is.EquivalentTo(new[] { 6, 7, 14, 15 }));

            // Third row
            Assert.That(finder[13], Is.EquivalentTo(new[] { 9, 10, 17, 18 }));

            // Fourth row
            Assert.That(finder[18], Is.EquivalentTo(new[] { 13, 14, 21, 22 }));

            // Sixth row
            Assert.That(finder[24], Is.EquivalentTo(new[] { 20, 28 }));
            Assert.That(finder[27], Is.EquivalentTo(new[] { 22, 23, 30, 31 }));

            // Seventh row
            Assert.That(finder[28], Is.EquivalentTo(new[] { 24, 25 }));
            Assert.That(finder[31], Is.EquivalentTo(new[] { 27 }));
        }
    }
}
