using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
                throw new ArgumentException("File name is a required argument.");

            string fileName = args[0];

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader= new StreamReader(fileStream))
            {
                if (streamReader.EndOfStream || !int.TryParse(streamReader.ReadLine(), out int totalSize))
                    throw new Exception("Unable to read 'totalSize' from line 1.");

                List<Chunk> chunks = new List<Chunk>();
                int lineNumber = 2;
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var cols = line.Split('\t');
                    if (cols.Count() != 3)
                    {
                        Console.WriteLine($"Warning! Line number '{lineNumber}' has an invalid number of columns.");
                        continue;
                    }

                    //add parse check for cols[1] and cols[2]

                    chunks.Add(
                        new Chunk(
                            cols[0], 
                            int.Parse(cols[1]), 
                            int.Parse(cols[2])));

                    lineNumber++;
                }

                var bestChunks = Chunk.GetMinimumChunks(totalSize, chunks);
                if (bestChunks == null)
                    Console.WriteLine("No combination of chunks covers all the data.");
                else
                {
                    foreach(var chunk in bestChunks)
                    {
                        Console.WriteLine(chunk.Id);
                    }
                }
            }
        }
    }

    public class Chunk
    {
        public static IEnumerable<Chunk> GetMinimumChunks(int totalSize, IEnumerable<Chunk> chunks)
        {
            if (chunks == null || !chunks.Any())
                return null;

            if (totalSize < 0)
                throw new ArgumentOutOfRangeException("totalSize");

            if (chunks.Any(chunk => chunk.Start < 0))
                throw new ArgumentOutOfRangeException("chunk.Start");

            if (chunks.Any(chunk => chunk.Size < 0))
                throw new ArgumentOutOfRangeException("chunk.Size");

            return TryChunks(totalSize, chunks, new List<Chunk>());
        }

        public static bool IsMissingData(int totalSize, IEnumerable<Chunk> chunks)
        {
            return Enumerable.Range(0, totalSize)
                .Where(index => 
                    !chunks.Any(chunk => index >= chunk.Start && index <= chunk.Start + chunk.Size))
                .Any();
        }

        public static IEnumerable<Chunk> TryChunks(int totalSize, IEnumerable<Chunk> remainingChunks, IEnumerable<Chunk> selectedChunks)
        {
            if (!IsMissingData(totalSize, selectedChunks))
                return selectedChunks;

            if (remainingChunks.Any())
            {
                IEnumerable<Chunk> bestChunks = null;
                foreach (Chunk selectedChunk in remainingChunks)
                {
                    var testChunks = TryChunks(
                        totalSize,
                        remainingChunks.SkipWhile(chunk => chunk != selectedChunk).Skip(1),
                        selectedChunks.Append(selectedChunk));

                    if (testChunks != null && (bestChunks == null || testChunks.Count() < bestChunks.Count()))
                        bestChunks = testChunks;
                }
                return bestChunks?.OrderBy(chunk => chunk.Id);
            }
            else
                return null;
        }

        public string Id;
        public int Start;
        public int Size;
        public Chunk(string id, int start, int size)
        {
            Id = id;
            Start = start;
            Size = size;
        }   
    }
}
