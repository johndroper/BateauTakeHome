using ConsoleApp3;
using NUnit.Framework;
using System;
using System.Linq;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Base case given
        [Test]
        public void GetMinimumChunksTest1()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("C", 50, 30),
                new Chunk("D", 75, 25),
                new Chunk("E", 30, 10),
                new Chunk("F", 65, 10) };
            int totalSize = 100;
            var selectedChunks = Chunk
                .GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "A", "B", "C", "D" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }

        //Base case reordered
        [Test]
        public void GetMinimumChunksTestReorder()
        {
            var chunks = new Chunk[] {
                new Chunk("F", 65, 10),
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("D", 75, 25),
                new Chunk("E", 30, 10),
                new Chunk("C", 50, 30)};
            int totalSize = 100;
            var selectedChunks = Chunk
                .GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "A", "B", "C", "D" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }

        //Base case reordered 2
        [Test]
        public void GetMinimumChunksTestReorder2()
        {
            var chunks = new Chunk[] {
                new Chunk("A1", 0, 8),
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("C", 50, 30),
                new Chunk("C1", 50, 23),
                new Chunk("D", 75, 25),
                new Chunk("D1", 75, 20),
                new Chunk("E", 30, 10),
                new Chunk("F", 65, 10) };
            int totalSize = 100;
            var selectedChunks = Chunk
                .GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "A", "B", "C", "D" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }

        //One chunk full converage
        [Test]
        public void GetMinimumChunksTestOneItemCoverage()
        {
            var chunks = new Chunk[] {
                new Chunk("A1", 0, 8),
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("C", 50, 30),
                new Chunk("C1", 50, 23),
                new Chunk("D", 75, 25),
                new Chunk("Z", 0, 99),
                new Chunk("E", 30, 10),
                new Chunk("F", 65, 10) };
            int totalSize = 100;
            var selectedChunks = Chunk
                .GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "Z" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }


        //No combination of chunks has full coverage 
        [Test]
        public void GetMinimumChunksTest2()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50) };
            int totalSize = 100;
            var selectedChunks = Chunk
                .GetMinimumChunks(totalSize, chunks);

            Assert.IsTrue(selectedChunks == null);
        }

        //One chunk has full coverage 
        [Test]
        public void GetMinimumChunksTest3()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 99) };
            int totalSize = 100;
            var selectedChunks = Chunk.GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "A" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }

        //Two chunks have full coverage 
        [Test]
        public void GetMinimumChunksTest4()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", 49, 50),
            };
            int totalSize = 100;
            var selectedChunks = Chunk.GetMinimumChunks(totalSize, chunks);
            var correctIds = new string[] { "A", "B" };
            Assert.IsTrue(selectedChunks
                .Select(chunk => chunk.Id)
                .SequenceEqual(correctIds));
        }

        //Negative totalSize should throw
        [Test]
        public void GetMinimumChunksTestArgumentOutOfRangeException1()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", 49, 50),
            };
            int totalSize = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => Chunk.GetMinimumChunks(totalSize, chunks));
        }

        //Negative chunk.start should throw
        [Test]
        public void GetMinimumChunksTestArgumentOutOfRangeException2()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", -49, 50),
            };
            int totalSize = 100;
            Assert.Throws<ArgumentOutOfRangeException>(() => Chunk.GetMinimumChunks(totalSize, chunks));
        }

        //Negative chunk.size should throw
        [Test]
        public void GetMinimumChunksTestArgumentOutOfRangeException3()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", 49, -50),
            };
            int totalSize = 100;
            Assert.Throws<ArgumentOutOfRangeException>(() => Chunk.GetMinimumChunks(totalSize, chunks));
        }

        //Base case given
        [Test]
        public void IsMissingDataBase1()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("C", 50, 30),
                new Chunk("D", 75, 25)};
            int totalSize = 100;
            Assert.IsTrue(!Chunk.IsMissingData(totalSize, chunks));
        }

        //No coverage
        [Test]
        public void IsMissingDataBase2()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50),
                new Chunk("C", 50, 30)};
            int totalSize = 100;
            Assert.IsTrue(Chunk.IsMissingData(totalSize, chunks));
        }

        //Chunks do not have coverage
        [Test]
        public void IsMissingData1()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 20),
                new Chunk("B", 10, 50) };
            int totalSize = 100;
            Assert.IsTrue(Chunk.IsMissingData(totalSize, chunks));
        }

        //A single chunk has full coverage
        [Test]
        public void IsMissingData2()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 99) };
            int totalSize = 100;
            Assert.IsTrue(!Chunk.IsMissingData(totalSize, chunks));
        }

        //A two chunks have full coverage with no overlap
        [Test]
        public void IsMissingData3()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", 49, 50),
            };
            int totalSize = 100;
            Assert.IsTrue(!Chunk.IsMissingData(totalSize, chunks));
        }

        //A two chunks have full coverage with overlap
        [Test]
        public void IsMissingData4()
        {
            var chunks = new Chunk[] {
                new Chunk("A", 0, 50),
                new Chunk("B", 1, 98),
            };
            int totalSize = 100;
            Assert.IsTrue(!Chunk.IsMissingData(totalSize, chunks));
        }
    }
}