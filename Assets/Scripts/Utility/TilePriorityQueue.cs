using System.Collections.Generic;
namespace WorldGenerator
{
    public class TilePriorityQueue
    {

        List<TerrainTile> list = new List<TerrainTile>();

        int count = 0;
        int minimum = int.MaxValue;

        public int Count
        {
            get
            {
                return count;
            }
        }

        public void Enqueue(TerrainTile cell)
        {
            count += 1;
            int priority = cell.SearchPriority;
            if (priority < minimum)
            {
                minimum = priority;
            }
            while (priority >= list.Count)
            {
                list.Add(null);
            }
            cell.NextWithSamePriority = list[priority];
            list[priority] = cell;
        }

        public TerrainTile Dequeue()
        {
            count -= 1;
            for (; minimum < list.Count; minimum++)
            {
                TerrainTile cell = list[minimum];
                if (cell != null)
                {
                    list[minimum] = cell.NextWithSamePriority;
                    return cell;
                }
            }
            return null;
        }

        public void Change(TerrainTile cell, int oldPriority)
        {
            TerrainTile current = list[oldPriority];
            TerrainTile next = current.NextWithSamePriority;
            if (current == cell)
            {
                list[oldPriority] = next;
            }
            else
            {
                while (next != cell)
                {
                    current = next;
                    next = current.NextWithSamePriority;
                }
                current.NextWithSamePriority = cell.NextWithSamePriority;
            }
            Enqueue(cell);
            count -= 1;
        }

        public void Clear()
        {
            list.Clear();
            count = 0;
            minimum = int.MaxValue;
        }
    }
}

