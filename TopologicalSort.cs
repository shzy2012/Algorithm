using System;
using System.Collections.Generic;
using System.Linq;
public class TopologicalSort
{
    public IEnumerable<string> OrderBy(IEnumerable<TopologicNode> nodes)
    {
        if (nodes == null) yield break;

        List<TopologicNode> list = new List<TopologicNode>();
        foreach (var item in nodes)
        {
            TopologicNode node = new TopologicNode() { Key = item.Key };
            if (item.Dependences != null)
            {
                node.Dependences = new List<string>(item.Dependences);
            }
            list.Add(node);
        }

        while (list.Count > 0)
        {
            //查找依赖项为空的节点
            var item = list.FirstOrDefault(x => x.Dependences == null || x.Dependences.Count == 0);
            if (item != null)
            {
                yield return item.Key;
                //移除用过的节点，以及与其相关的依赖关系
                list.Remove(item);
                foreach (var otherNode in list)
                {
                    if (otherNode.Dependences != null)
                    {
                        otherNode.Dependences.Remove(item.Key);
                    }
                }
            }
            else if (list.Count > 0)
            {
                //如果发现有向环，则抛出异常
                throw new InvalidOperationException("存在双向引用或循环引用");
            }
        }
    }
}

public class TopologicNode
{
    /// <summary>
    /// 获取或设置节点的键值
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 获取或设置依赖节点的键值列表
    /// </summary>
    public List<string> Dependences { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        List<TopologicNode> nodes = new List<TopologicNode>()
            {
                new TopologicNode(){ Key = "XMedia", 
                    Dependences = new List<string>(){ "XMedia.Controllers", "XMedia.Models", "XMedia.Logics", "XMedia.Commons" } },
                new TopologicNode(){ Key = "XMedia.Controllers",
                    Dependences = new List<string>(){"XMedia.Models","XMedia.Logics","XMedia.Commons"}},
                new TopologicNode(){ Key = "XMedia.Logics", 
                    Dependences = new List<string>(){ "XMedia.Models","XMedia.Commons"}},
                new TopologicNode(){ Key = "XMedia.Models" },
                new TopologicNode(){ Key = "XMedia.Commons" }
            };

        //输出拓扑排序的结果
        TopologicalSort sort = new TopologicalSort();
        foreach (var key in sort.OrderBy(nodes))
        {
            Console.WriteLine(key);
        }
        Console.ReadLine();
    }
}

