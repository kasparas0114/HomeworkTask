using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkTask.Models
{
    public class Node: ICloneable
    {
        public int Value { get; set; }
        public int Index { get; set; }

        public Node (int val, int index)
        {
            this.Value = val;
            this.Index = index;
        }

        public object Clone()
        {
            return new Node(this.Value, this.Index);
        }
    }
}