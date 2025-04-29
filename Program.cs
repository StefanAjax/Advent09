using System.Collections;

namespace Advent09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("C:\\Users\\stefan.ajax\\source\\repos\\Advent09\\Advent09\\trueData.txt");
            var numbers = new List<long>();
            foreach (var line in lines)
            {
                numbers.Add(long.Parse(line));
            }
            var badNumber = getBadNumber(numbers, 25);
            Console.WriteLine($"The first erronous number is {badNumber}");
            var contiguousSet = getContiguousSet(numbers, badNumber);
            Console.WriteLine($"The contiguous set is {string.Join(", ", contiguousSet)}");
            var min = contiguousSet.Min();
            var max = contiguousSet.Max();
            Console.WriteLine($"The min is {min}");
            Console.WriteLine($"The max is {max}");
            var result = min + max;
            Console.WriteLine($"The result is {result}");
        }

        public static List<long> getContiguousSet(List<long> numbers, long target)
        {
            var result = new List<long>();
            for (int i = 0; i < numbers.Count; i++)
            {
                var sum = numbers[i];

                for (int j = i + 1; j < numbers.Count; j++)
                {
                    sum += numbers[j];
                    if (sum > target)
                    {
                        sum = 0L;
                        break;
                    }
                    else if (sum == target)
                    {
                        result = numbers[i..(j + 1)].ToList();
                    }
                }
            }
            return result;
        }

        public static long getBadNumber(List<long> numbers, int preamble)
        {
            long result = 0;
            var buffer = new CircularBuffer(preamble);
            for (int i = 0; i < preamble; i++)
            {
                buffer.Add(numbers[i]);
            }
            foreach (var number in numbers[preamble..])
            {
                if (buffer.PossibleSums.Contains(number) == false)
                {
                    result = number;
                    break;
                }
                else
                {
                    buffer.Add(number);
                }
            }
            return result;

        }

        class CircularBuffer
        {
            public int Count { get; private set; }
            public int Capacity { get; private set; }
            public int Head { get; private set; }
            public long[] Buffer { get; private set; }

            private bool possibleSumsCalculated = false;

            public List<long> PossibleSums { get; private set; }
            public List<long> GetBuffer()
            {
                var result = new List<long>();
                for (int i = 0; i < Count; i++)
                {
                    int index = (Head + Capacity - Count + i) % Capacity;
                    result.Add(Buffer[index]);
                }
                return result;
            }

            public CircularBuffer(int capacity)
            {
                Capacity = capacity;
                Buffer = new long[capacity];
                PossibleSums = new List<long>();
                Count = 0;
                Head = 0;

            }
            public void Add(long item)
            {
                Buffer[Head] = item;
                if (Count < Capacity)
                    Count++;
                Head = (Head + 1) % Capacity;

                // Recalculate the possible sums
                if (Count == Capacity)
                {
                    if (possibleSumsCalculated == false)
                    {
                        for (int i = 0; i < Capacity; i++)
                        {
                            for (int j = i + 1; j < Capacity; j++)
                            {
                                PossibleSums.Add(Buffer[i] + Buffer[j]);
                            }
                        }
                        possibleSumsCalculated = true;

                    }
                    else
                    {   //TODO
                        //  0   1   2   3   4   5   6   7   8   9
                        // 0+1 0+2 0+3 0+4 1+2 1+3 1+4 2+3 2+4 3+4
                        //0 x   x   x   x
                        //1 x               x   x   x
                        //2     x           x           x   x
                        //3         x           x       x       x 
                        //4             x           x       x   x
                        // Should only recalculate the sums that are affected by the new item
                        // If neccesary, do that. Indexing is big brain stuff so here I just clear the list and recalulate all
                        // Idea: use a hashset to stor the sums and identify outgoing numbers.
                        PossibleSums.Clear();
                        for (int i = 0; i < Capacity; i++)
                        {
                            for (int j = i + 1; j < Capacity; j++)
                            {
                                PossibleSums.Add(Buffer[i] + Buffer[j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
