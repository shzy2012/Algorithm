namespace Graph
{
    using System;
    using System.Collections.Generic;

    public class AdjacencyList<T>
    {
        List<Vertex<T>> items; //图的顶点集合
        public AdjacencyList() : this(10) { }
        public AdjacencyList(int capacity) //指定容量的构造方法
        {
            items = new List<Vertex<T>>(capacity);
        }

        //添加一个顶点
        public void AddVertex(T item)
        {
            //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("顶点已存在.");
            }

            items.Add(new Vertex<T>(item));
        }

        //添加无向边
        public void AddEdge(T from, T to)
        {
            Vertex<T> fromVer = Find(from);
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点不存在.");
            }

            Vertex<T> toVer = Find(to);
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点不存在.");
            }

            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer);
            AddDirectedEdge(toVer, fromVer);
        }

        //查找图中是否包含某项
        public bool Contains(T item)
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        //查找指定项并返回
        public Vertex<T> Find(T item)
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return v;
                }
            }
            return null;
        }

        //添加有向边
        public void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
        {
            if (fromVer.firstEdge == null) //无邻接点时
            {
                fromVer.firstEdge = new Node(toVer);
            }
            else
            {
                Node tmp, node = fromVer.firstEdge;
                do
                {
                    //检查是否添加了重复边
                    if (node.adjvex.data.Equals(toVer.data))
                    {
                        throw new ArgumentException("添加了重复的边.");
                    }

                    tmp = node;
                    node = node.Next;
                } while (node != null);

                tmp.Next = new Node(toVer); //添加到链表未尾
            }
        }

        public override string ToString() //仅用于测试
        {   //打印每个节点和它的邻接点
            string s = string.Empty;
            foreach (Vertex<T> v in items)
            {
                s += v.data.ToString() + ":";
                if (v.firstEdge != null)
                {
                    Node tmp = v.firstEdge;
                    while (tmp != null)
                    {
                        s += tmp.adjvex.data.ToString();
                        tmp = tmp.Next;
                    }
                }
                s += "\r\n";
            }
            return s;
        }

        #region 嵌套类

        //嵌套类，表示链表中的表结点
        public class Node
        {
            /// <summary>
            /// 邻接点域
            /// </summary>
            public Vertex<T> adjvex;
            /// <summary>
            /// 下一个邻接点指针域
            /// </summary>
            public Node Next;
            public Node(Vertex<T> value)
            {
                adjvex = value;
            }
        }

        //嵌套类，表示存放于数组中的表头结点
        public class Vertex<TValue>
        {
            /// <summary>
            /// 数据
            /// </summary>
            public TValue data;
            /// <summary>
            /// 邻接点链表头指针
            /// </summary>
            public Node firstEdge;
            /// <summary>
            /// 访问标志,遍历时使用
            /// </summary>
            public bool visited;
            public Vertex(TValue value)
            {
                data = value;
            }
        }
        #endregion

        #region 遍历

        //深度优先搜索遍历 DFSTraverse.cs
        public void DFSTraverse()
        {
            InitVisited();//将visited标志全部置为false
            foreach (Vertex<T> v in items)
            {
                //如果未被访问
                if (!v.visited)
                {
                    DFS(v); //深度优先遍历
                }
            }
        }

        //使用递归进行深度优先遍历
        private void DFS(Vertex<T> v)
        {
            v.visited = true; //将访问标志设为true
            Console.WriteLine(v.data + " ");
            Node node = v.firstEdge;
            while (node != null) //访问此顶点的所有邻接点
            {
                if (!node.adjvex.visited)
                {
                    DFS(node.adjvex);//递归
                }

                node = node.Next;//访问下一个邻接点
            }
        }

        //初始化visited标志
        private void InitVisited()
        {
            foreach (Vertex<T> item in items)
            {
                item.visited = false; //全部置为false
            }
        }

        //广度优先搜索遍历
        public void BFSTraverse()
        {
            InitVisited(); //将visited标志全部置为false
            foreach (Vertex<T> v in items)
            {
                if (!v.visited) //如果未被访问
                {
                    BFS(v); //广度优先遍历
                }
            }
        }
        //使用队列进行广度优先遍历
        private void BFS(Vertex<T> v)
        {
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            Console.WriteLine(v.data + "");
            v.visited = true;//设置访问标志
            queue.Enqueue(v);//进队
            while (queue.Count > 0)
            {
                Vertex<T> w = queue.Dequeue();
                Node node = w.firstEdge;
                while (node != null) //访问此顶点的所有邻接点
                {
                    //如果邻接点未被访问，则递归访问它的边
                    if (!node.adjvex.visited)
                    {
                        Console.Write(node.adjvex.data + " ");//访问
                        node.adjvex.visited = true;//设置访问标志
                        queue.Enqueue(node.adjvex);//进队
                    }

                    node = node.Next; //访问下一个邻接点
                }
            }

        }
        #endregion
    }
}

static void Main(string[] args)
        {
            AdjacencyList<char> link = new AdjacencyList<char>();
            //添加顶点
            link.AddVertex('A');
            link.AddVertex('B');
            link.AddVertex('C');
            link.AddVertex('D');
            //添加边
            link.AddEdge('A', 'B');
            link.AddEdge('A', 'C');
            link.AddEdge('A', 'D');
            link.AddEdge('B', 'D');
            Console.WriteLine(link.ToString());

            //深度优先搜索遍历
            AdjacencyList<string> graph = new AdjacencyList<string>();
            graph.AddVertex("V1");
            graph.AddVertex("V2");
            graph.AddVertex("V3");
            graph.AddVertex("V4");
            graph.AddVertex("V5");
            graph.AddVertex("V6");
            graph.AddVertex("V7");
            graph.AddVertex("V8");
            graph.AddEdge("V1", "V2");
            graph.AddEdge("V1", "V3");
            graph.AddEdge("V2", "V4");
            graph.AddEdge("V2", "V5");
            graph.AddEdge("V3", "V6");
            graph.AddEdge("V3", "V7");
            graph.AddEdge("V4", "V8");
            graph.AddEdge("V5", "V8");
            graph.AddEdge("V6", "V8");
            graph.AddEdge("V7", "V8");
            graph.DFSTraverse();

            //广度优先搜索遍历
            AdjacencyList<string> graph2 = new AdjacencyList<string>();
            graph2.AddVertex("V1");
            graph2.AddVertex("V2");
            graph2.AddVertex("V3");
            graph2.AddVertex("V4");
            graph2.AddVertex("V5");
            graph2.AddVertex("V6");
            graph2.AddVertex("V7");
            graph2.AddVertex("V8");
            graph2.AddEdge("V1", "V2");
            graph2.AddEdge("V1", "V3");
            graph2.AddEdge("V2", "V4");
            graph2.AddEdge("V2", "V5");
            graph2.AddEdge("V3", "V6");
            graph2.AddEdge("V3", "V7");
            graph2.AddEdge("V4", "V8");
            graph2.AddEdge("V5", "V8");
            graph2.AddEdge("V6", "V8");
            graph2.AddEdge("V7", "V8");
            graph2.BFSTraverse();
        }
